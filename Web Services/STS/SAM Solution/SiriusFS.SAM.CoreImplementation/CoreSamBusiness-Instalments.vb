'===================================================
'Module Name        : CoreSAMBusiness
'Project            : SAM.ServiceAgent
'Copyright          : © SSP Limited 2013. All rights reserved.

'Instalment and Related Functionality

'This code adhers to the SSP Coding Standards Document V2013-01

'THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
'EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
'WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

'===================================================



Option Strict On
Option Explicit On
#Region "Imports"
Imports dPMDAOBridge
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports SSP.Shared.gPMConstants
Imports SSP.Shared
#End Region

Partial Public Class CoreSAMBusiness
#Region "Public Methods"
#Region "GetHeaderAndSummariesPFPlanByKey"
    '''<summary>
    '''This function is used to GetHeaderAndSummaries Primimum Finance Plan By Key details 
    '''</summary>
    '''<param name="oGetHeaderAndSummariesPFPlanByKeyRequest">BaseGetHeaderAndSummariesPFPlanByKeyResponseType</param>
    '''<remarks></remarks>
    Public Overloads Function GetHeaderAndSummariesPFPlanByKey(ByVal oGetHeaderAndSummariesPFPlanByKeyRequest As BaseGetHeaderAndSummariesPFPlanByKeyRequestType) As BaseGetHeaderAndSummariesPFPlanByKeyResponseType
        Dim oBusiness As New CoreBusiness
        Dim oGetHeaderAndSummariesPFPlanByKeyResponseType As New BaseImplementationTypes.BaseGetHeaderAndSummariesPFPlanByKeyResponseType
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim bReturn As Boolean = True
        Dim nPFPremiumFinanceKey As Integer = oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey
        Dim nPFPremiumFinanceVersionKey As Integer = oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey

        ' validate the data provided in the structure
        oGetHeaderAndSummariesPFPlanByKeyRequest.Validate(CType(oSAMErrorCollection, Object))
        oSAMErrorCollection.CheckForErrors()

        ' validate the data provided in the structure
        If ((oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey = 0 OrElse oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey = 0) AndAlso (Len(oGetHeaderAndSummariesPFPlanByKeyRequest.DocumentRef) = 0)) Then
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CollectionDateError,
                                                  SAMBusinessErrors.CollectionDateError.ToString, "Invalid PFPremiumFinanceKey / PFPremiumFinanceVersionKey Or DocumentRef ")
        End If
        oSAMErrorCollection.CheckForErrors()

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                         _SiriusUser.Username, _SiriusUser.SourceID,
                                         _SiriusUser.LanguageID,
                                         SiriusUserDefaults.AppName)
            Try

                If (oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey = 0 AndAlso oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey = 0) AndAlso
                    Len(oGetHeaderAndSummariesPFPlanByKeyRequest.DocumentRef) > 0 Then

                    'call spu_ACT_get_fp_dets_from_doc_ref to fetch PFPremiumFinanceKey and PFPremiumFinanceVersionKey and override request
                    bReturn = GetPFKeyAndVersionFromDocRef(con, nPFPremiumFinanceKey,
                                                           nPFPremiumFinanceVersionKey,
                                                           oGetHeaderAndSummariesPFPlanByKeyRequest.DocumentRef)
                    If bReturn Then
                        oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey = nPFPremiumFinanceKey
                        oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey = nPFPremiumFinanceVersionKey
                    End If

                End If

                ' Checking the Type of package we are receiving in form of request
                Dim nTypeOfPackage As CoreSAMBusiness.enumTypeOfPackage
                If oGetHeaderAndSummariesPFPlanByKeyRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesPFPlanByKeyRequestType) Then
                    nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
                    oGetHeaderAndSummariesPFPlanByKeyResponseType = New SAMForInsuranceV2ImplementationTypes.GetHeaderAndSummariesPFPlanByKeyResponseType
                End If

                With oGetHeaderAndSummariesPFPlanByKeyResponseType
                    Try
                        'Collect all the Premimum Finace Details by calling the GetPFDetails Function1(1)
                        .PremiumFinanceDetails = GetPFDetails(con, oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey,
                                                            oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey,
                                                            oGetHeaderAndSummariesPFPlanByKeyRequest.UserID)
                    Catch ex As Exception
                        oSAMErrorCollection.AddInvalidData(SAMBusinessErrors.CollectionDateError, SAMBusinessErrors.CollectionDateError.ToString, "GetPFDetails Method Failed")
                        oSAMErrorCollection.CheckForErrors()
                    End Try

                    'Collect all the Premimum Finace Transactions by calling the GetPFTransactionDetails Function1(2)
                    Try
                        Dim oPFget As New BasePremiumFinancePlanDetailsType
                        .Transactions = GetPFTransactionDetails(con, oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey,
                                                                                                             oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey)

                    Catch ex As Exception
                        oSAMErrorCollection.AddInvalidData(SAMBusinessErrors.CollectionDateError, SAMBusinessErrors.CollectionDateError.ToString, "GetPFTransactionDetails Method Failed to called")
                        oSAMErrorCollection.CheckForErrors()
                    End Try

                    'Collect all the Premimum Finace history by calling the GetPFHistoryDetails Function1(3)
                    Try
                        .PFHistory = GetPFHistoryDetails(con, oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey,
                                                                                                           oSAMErrorCollection)
                    Catch ex As Exception
                        oSAMErrorCollection.AddInvalidData(SAMBusinessErrors.CollectionDateError, SAMBusinessErrors.CollectionDateError.ToString, "GetPFHistoryDetails Method Failed to called")
                        oSAMErrorCollection.CheckForErrors()
                    End Try


                    'Collect all the Premimum Finace Transactions by calling the GetPFBankHistoryDetails Function1(4)
                    Try
                        .PFBankHistory = GetPFBankHistoryDetails(con, oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceKey,
                                                                                      oGetHeaderAndSummariesPFPlanByKeyRequest.PFPremiumFinanceVersionKey,
                                                                                      oSAMErrorCollection)
                    Catch ex As Exception
                        oSAMErrorCollection.AddInvalidData(SAMBusinessErrors.CollectionDateError, SAMBusinessErrors.CollectionDateError.ToString, "GetPFBankHistoryDetails Method Failed to called")
                        oSAMErrorCollection.CheckForErrors()
                    End Try

                    'Collect all the Premimum Finace Installements by calling the GetPFInstalments Function1(5)
                    Try
                        .Installements = GetPFInstalments(con, nPFPremiumFinanceKey, nPFPremiumFinanceVersionKey,
                                                                                                    oSAMErrorCollection)
                    Catch ex As Exception
                        oSAMErrorCollection.AddInvalidData(SAMBusinessErrors.CollectionDateError, SAMBusinessErrors.CollectionDateError.ToString, "GetPFInstalments Method Failed to called")
                        oSAMErrorCollection.CheckForErrors()
                    End Try



                End With

            Finally
                con.Dispose()
            End Try

        End Using

        Return oGetHeaderAndSummariesPFPlanByKeyResponseType

    End Function
#End Region

#Region "UpdateInstalmentStatus"
    '''<summary>
    '''This function is used to Update InstalmentStatus
    '''</summary>
    '''<param name="oUpdateInstalmentStatusRequest">BaseUpdateInstalmentStatusRequestType</param>
    '''<remarks></remarks>
    Public Overloads Function UpdateInstalmentStatus(ByVal oUpdateInstalmentStatusRequest As BaseUpdateInstalmentStatusRequestType) As BaseUpdateInstalmentStatusResponseType
        Dim oUpdateInstalmentStatusResponseType As New BaseImplementationTypes.BaseUpdateInstalmentStatusResponseType
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim nStatusId As Integer = 0

        ' validate the data provided in the structure
        oUpdateInstalmentStatusRequest.Validate(CType(oSAMErrorCollection, Object))
        oSAMErrorCollection.CheckForErrors()

        ' Checking the Type of package we are receiving in form of request
        Dim nTypeOfPackage As CoreSAMBusiness.enumTypeOfPackage
        If oUpdateInstalmentStatusRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateInstalmentStatusRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateInstalmentStatusResponseType = New SAMForInsuranceV2ImplementationTypes.UpdateInstalmentStatusResponseType
        End If

        Using conPFI As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Try

                nStatusId = GetAndValidateSpecifiedTableCode(conPFI, "PFInstalments_Status", "PFInstalments_Status_id", "Code",
                                                         oUpdateInstalmentStatusRequest.PFIStatusCode.ToString, oSAMErrorCollection, "Code")
                oSAMErrorCollection.CheckForErrors()
                Try
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_PFInstalment_Status_Update")
                        cmd.AddInParameter("@pfinstalments_id", SqlDbType.Int).Value = oUpdateInstalmentStatusRequest.PFInstalmentKey
                        cmd.AddInParameter("@pfinstalments_status_id", SqlDbType.Int).Value = nStatusId
                        conPFI.ExecuteNonQuery(cmd)
                    End Using

                Catch ex As Exception
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CollectionDateError,
                              SAMBusinessErrors.CollectionDateError.ToString,
                             "Failed to call Procedure spu_ACT_PFInstalment_Status_Update")
                    oSAMErrorCollection.CheckForErrors()

                End Try
            Finally
                conPFI.Dispose()
            End Try
        End Using

        Return oUpdateInstalmentStatusResponseType

    End Function
#End Region

#Region "CancelPremiumFinancePlan"

    ''' <summary>
    ''' Calls the overloaded implementation function CancelPremiumFinancePlan
    ''' </summary>
    ''' <param name="oCancelPremiumFinancePlanRequest"></param>
    ''' <returns>BaseImplementationTypes.BaseCancelPremiumFinancePlanResponseType</returns>
    ''' <remarks></remarks>
    Public Overloads Function CancelPremiumFinancePlan(ByVal oCancelPremiumFinancePlanRequest As BaseCancelPremiumFinancePlanRequestType) As BaseCancelPremiumFinancePlanResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseCancelPremiumFinancePlanResponseType
            oResponse = CancelPremiumFinancePlan(con, oCancelPremiumFinancePlanRequest)
            Return oResponse
        End Using
    End Function



    ''' <summary>
    ''' implementation function for Cancel / Delete / Settle Plan functionality
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oCancelPremiumFinancePlanRequest"></param>
    ''' <returns>BaseCancelPremiumFinancePlanResponseType</returns>
    ''' <remarks></remarks>
    Public Overloads Function CancelPremiumFinancePlan(ByVal con As SiriusConnection,
                                                        ByVal oCancelPremiumFinancePlanRequest As BaseCancelPremiumFinancePlanRequestType) As BaseCancelPremiumFinancePlanResponseType



        Dim oResponse As New BaseCancelPremiumFinancePlanResponseType
        Dim oSamErrorCollection As SAMErrorCollection = New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim nComReturnValue As Integer
        Dim aoPremiumFinance(,) As Object


        If oCancelPremiumFinancePlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        oCancelPremiumFinancePlanRequest.Validate(CType(oSamErrorCollection, Object))
        oSamErrorCollection.CheckForErrors()

        Dim oPremiumFinanceBusiness As bSIRPremiumFinance.Business

        oPremiumFinanceBusiness = CreateAndInitialisePremiumFinanceBusiness(con,
                                                                        oCancelPremiumFinancePlanRequest.BranchCode)

        nComReturnValue = oPremiumFinanceBusiness.GetSingleFinancePlan(oCancelPremiumFinancePlanRequest.PFPremiumFinanceKey,
                                                                            oCancelPremiumFinancePlanRequest.PFPremiumFinanceVersionKey,
                                                                            aoPremiumFinance)

        If nComReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
        End If

        If oCancelPremiumFinancePlanRequest.RequestType <> CancelPFPlanType.DeletePlan Then

            If aoPremiumFinance(k_PFPlanStatusInd, 0).ToString() <> InstalmentPlanStatus.Live Then
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanNoLivePlan,
                                                    "There is no Live Plan." &
                                                    "There is no Live Plan.",
                                                    "Cancel Premium Finance Plan")

            End If

            Dim dSettlementAmount As Decimal
            GetPremiumFinanceOutstanding(con,
                                         aoPremiumFinance,
                                         dSettlementAmount,
                                         oCancelPremiumFinancePlanRequest.BranchCode,
                                         oPremiumFinanceBusiness)


            If dSettlementAmount <= 0 AndAlso Convert.ToInt32(aoPremiumFinance(k_PFPlanSchemeType_ID, 0)) <> 1 Then
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanNoOutStandingBalance,
                                                    "No Outstanding balance." &
                                                    "This instalment plan has no outstanding balance.",
                                                    "Cancel Premium Finance Plan")

            End If
        Else

            If (aoPremiumFinance(k_PFPlanStatusInd, 0).ToString() <> InstalmentPlanStatus.Saved) _
                AndAlso (aoPremiumFinance(k_PFPlanStatusInd, 0).ToString() <> InstalmentPlanStatus.Updated) Then
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanVersionNotCorrect,
                                                    "Plan is not Pre-Live." &
                                                    "Please Specify a valid plan.",
                                                    "Cancel Premium Finance Policy")
            End If

            If oCancelPremiumFinancePlanRequest.PFPremiumFinanceVersionKey > 1 Then
                If aoPremiumFinance(k_PFPlanStatusInd, 0).ToString() = InstalmentPlanStatus.Live Then
                    oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanVersionNotCorrect,
                                                        "Plan is not version 1." &
                                                        "Please Specify a valid plan with version 1.",
                                                        "Cancel Premium Finance Policy")

                End If
            End If

        End If

        oSamErrorCollection.CheckForErrors()

        If oCancelPremiumFinancePlanRequest.RequestType = CancelPFPlanType.DeletePlan Then
            oResponse = DeletePlan(con, oCancelPremiumFinancePlanRequest, aoPremiumFinance, oPremiumFinanceBusiness)
        ElseIf oCancelPremiumFinancePlanRequest.RequestType = CancelPFPlanType.CancelPlan Then
            oResponse = CancelPlan(con, oCancelPremiumFinancePlanRequest, aoPremiumFinance, oPremiumFinanceBusiness)
        ElseIf oCancelPremiumFinancePlanRequest.RequestType = CancelPFPlanType.SettlePlan Then
            oResponse = SettlePlan(con, oCancelPremiumFinancePlanRequest, aoPremiumFinance, oPremiumFinanceBusiness)
        End If

        Return oResponse

    End Function
#End Region

#Region "GetPremiumFinanceOutstanding"
    ''' <summary>
    ''' Premium Finance array from spu_PFPremiumFinance_Sel_Single of object type
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oPremiumFinance"></param>
    ''' <param name="r_dSettlementAmount"></param>
    ''' <param name="sBranchCode"></param>
    ''' <param name="r_oPremiumFinanceBusiness"></param>
    ''' <remarks></remarks>
    Private Sub GetPremiumFinanceOutstanding(ByVal con As SiriusConnection,
                                                ByVal oPremiumFinance As Object,
                                                ByRef r_dSettlementAmount As Decimal,
                                                ByVal sBranchCode As String,
                                                ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business)


        Dim nComReturnValue As Integer

        Try

            nComReturnValue = r_oPremiumFinanceBusiness.SettlePlanCalculate(v_vPremiumFinance:=oPremiumFinance,
                                                                                r_crSettlement:=r_dSettlementAmount,
                                                                                r_crRefund:=0,
                                                                                r_dtNextInstalmentDate:=#12/30/1899#,
                                                                                r_dtNextInstalmentDatePlus1:=#12/30/1899#,
                                                                                r_dtLastInstalmentDate:=#12/30/1899#,
                                                                                r_dtLastPaidInstalmentDate:=#12/30/1899#,
                                                                                r_vSettlementFormatted:=Nothing)

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.SettlePlanCalculate", nComReturnValue)
            End If
        Catch ex As Exception
            Throw New ApplicationException("An exception occured in GetPremiumFinanceOutstanding.", ex)
        End Try

    End Sub

#End Region

#Region "CancelPFPolicies"
    ''' <summary>
    ''' Calls the overloaded implementation function CancelPFPolicies
    ''' </summary>
    ''' <param name="oCancelPFPoliciesRequest"></param>
    ''' <returns>BaseImplementationTypes.BaseCancelPFPoliciesResponseType</returns>
    ''' <remarks></remarks>
    Public Overloads Function CancelPFPolicies(ByVal oCancelPFPoliciesRequest As BaseCancelPFPoliciesRequestType) As BaseCancelPFPoliciesResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseCancelPFPoliciesResponseType
            oResponse = CancelPFPolicies(con, oCancelPFPoliciesRequest)

            Return oResponse

        End Using
    End Function


    ''' <summary>
    ''' implementation function for Cancel / Delete / Settle Plan functionality
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oCancelPFPoliciesRequest"></param>
    ''' <returns>BaseCancelPFPoliciesResponseType</returns>
    ''' <remarks></remarks>
    Public Overloads Function CancelPFPolicies(ByVal con As SiriusConnection,
                                                ByVal oCancelPFPoliciesRequest As BaseCancelPFPoliciesRequestType) As BaseCancelPFPoliciesResponseType

        Dim oResponse As New BaseCancelPFPoliciesResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim oSamErrorCollection As New SAMErrorCollection
        Dim nComReturnValue As Integer
        Dim aoPremiumFinance(,) As Object
        Dim oPremiumFinanceBusiness As bSIRPremiumFinance.Business = Nothing


        If oCancelPFPoliciesRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelPFPoliciesRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelPFPoliciesResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        oCancelPFPoliciesRequest.Validate(CObj(oSamErrorCollection))
        oSamErrorCollection.CheckForErrors()

        Try
            con.BeginTransaction()
            oPremiumFinanceBusiness = CreateAndInitialisePremiumFinanceBusiness(con,
                                                                                oCancelPFPoliciesRequest.BranchCode)

            nComReturnValue = oPremiumFinanceBusiness.GetSingleFinancePlan(oCancelPFPoliciesRequest.PFPremiumFinanceKey,
                                                                            oCancelPFPoliciesRequest.PFPremiumFinanceVersionKey,
                                                                            aoPremiumFinance)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
            End If

            Dim dtCoverStartDate As Date
            If IsDate(aoPremiumFinance(k_PFPlanCoverStartDate, 0)) Then
                dtCoverStartDate = CDate(aoPremiumFinance(k_PFPlanCoverStartDate, 0))
            End If

            If oCancelPFPoliciesRequest.PolicyLapsedDate < dtCoverStartDate Then
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.OutOfSequenceCancellationNotAllowed,
                                                    "Backdated Cancellation is not allowed via this Roadmap." &
                                                    "Please Specify a valid Date or Cancel The Policy via Underwriting Cancel Policy Task.",
                                                    "Cancel Premium Finance Policy")
                oSamErrorCollection.CheckForErrors()
            End If


            Dim nLapseReasonId As Integer

            If CStr(oCancelPFPoliciesRequest.LapsedReasonCode) <> "" Then
                Dim oCoreBusiness As New CoreBusiness
                nLapseReasonId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                PMLookupTable.LapsedReason,
                                                                                oCancelPFPoliciesRequest.LapsedReasonCode,
                                                                                "ReasonCode", oSamErrorCollection)
                oSamErrorCollection.CheckForErrors()
            End If


            Dim sError As String = String.Empty
            nComReturnValue = oPremiumFinanceBusiness.CancelPolicies(CType(aoPremiumFinance, Array), sError, 0, 0,
                                                                        oCancelPFPoliciesRequest.WriteOff,
                                                                        oCancelPFPoliciesRequest.SpoolDoc,
                                                                        nLapseReasonId,
                                                                        oCancelPFPoliciesRequest.PolicyLapsedDate)

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CancelPolicies", nComReturnValue)
                oSamErrorCollection.CheckForErrors()
            End If
            con.CommitTransaction()

            If gPMFunctions.ToSafeInteger(aoPremiumFinance(k_PFPlanInsuranceFileCnt, 0)) = 0 Then
                RaiseComMethodException("InsuranceFileKey missing.No policy is linked with this plan.", 0)
            End If

            'Its being done after con.Commit(), as we have to cancel policy irrespective of the Business rule errors below.
            'However we need to show the business rule errors
            GenerateWriteOffAndAutoAllocate(con,
                                            oCancelPFPoliciesRequest.BranchCode,
                                            CInt(aoPremiumFinance(k_PFPlanInsuranceFileCnt, 0)),
                                            Not oCancelPFPoliciesRequest.WriteOff, True)

        Catch ex As Exception
            If con.TransactionCount > 0 Then
                con.RollbackTransaction()
            End If
            Throw ex

        End Try
        Return oResponse

    End Function

#End Region

#Region "ProcessPFPlan"
    ''' <summary>
    ''' Calls the overloaded implementation function ProcessPFPlan
    ''' </summary>
    ''' <param name="oProcessPFPlanRequest"></param>
    ''' <returns>BaseImplementationTypes.BaseProcessPFPlanResponseType</returns>
    ''' <remarks></remarks>
    Public Overloads Function ProcessPFPlan(ByVal oProcessPFPlanRequest As BaseProcessPFPlanRequestType) As BaseProcessPFPlanResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseProcessPFPlanResponseType
            oResponse = ProcessPFPlan(con, oProcessPFPlanRequest)

            Return oResponse

        End Using
    End Function


    ''' <summary>
    ''' Process Instalment Plan and New Plan
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oProcessPFPlanRequest"></param>
    ''' <returns>BaseProcessPFPlanResponseType</returns>
    ''' <remarks></remarks>
    Public Overloads Function ProcessPFPlan(ByVal con As SiriusConnection,
                                                ByVal oProcessPFPlanRequest As BaseProcessPFPlanRequestType) As BaseProcessPFPlanResponseType

        Dim oProcessPFPlanResponse As New BaseProcessPFPlanResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        Dim nComReturnValue As Integer
        Dim oPremiumFinanceBusiness As bSIRPremiumFinance.Business = Nothing

        If oProcessPFPlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ProcessPFPlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oProcessPFPlanResponse = New SAMForInsuranceV2ImplementationTypes.ProcessPFPlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        oPremiumFinanceBusiness = CreateAndInitialisePremiumFinanceBusiness(con, oProcessPFPlanRequest.BranchCode)
        If oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing Then
            oPremiumFinanceBusiness.VIAPaymentHub = oProcessPFPlanRequest.PFCreditCardDetails.VIAPaymentHub
        End If

        'bSIRPremiumFinance_PostMTA( does not handle AddToNewPlan through PlanMTA correctly
        'This does not work  in 3.1 / 3.2 as well in BO.
        If oProcessPFPlanRequest.TransType = ProcessPFPlanType.PlanMTA AndAlso oProcessPFPlanRequest.Type = InstalmentType.AddToNewPlan Then
            oProcessPFPlanRequest.PFPremFinanceKey = 0
            oProcessPFPlanRequest.PFPremFinanceVersion = 0
            oProcessPFPlanRequest.TransType = ProcessPFPlanType.NewPlan
            oProcessPFPlanRequest.PFQuote.ProcessPFMode = "MTA"
        End If

        If oProcessPFPlanRequest.TransType = ProcessPFPlanType.PlanMTA AndAlso oProcessPFPlanRequest.SaveOnly = False Then
            oProcessPFPlanResponse = PlanMTA(con, oProcessPFPlanRequest, oPremiumFinanceBusiness)

        ElseIf oProcessPFPlanRequest.TransType = ProcessPFPlanType.NewPlan AndAlso oProcessPFPlanRequest.SaveOnly = False Then
            oProcessPFPlanResponse = NewPlan(con, oProcessPFPlanRequest, oPremiumFinanceBusiness)

        ElseIf oProcessPFPlanRequest.SaveOnly Then
            oProcessPFPlanResponse = SavePlan(con, oProcessPFPlanRequest, oPremiumFinanceBusiness)

        End If

        Return oProcessPFPlanResponse

    End Function

#End Region

#End Region

#Region "Private Methods"

#Region "GetHeaderAndSummariesPFPlanByKey"
    '''<summary>
    '''This function is used to GetHeaderAndSummariesPF Details 
    '''</summary>
    '''<param name="conTransactions">SiriusConnection</param>
    '''<param name="onPFPremFinanceKey">Integer</param>
    '''<param name="onPFPremFinanceVersion">Integer</param>
    '''<remarks></remarks>
    Private Function GetPFDetails(ByVal conTransactions As SiriusConnection, ByVal onPFPremFinanceKey As Integer,
                                  ByVal onPFPremFinanceVersion As Integer,
                                  ByVal nUserID As Integer) As BasePremiumFinancePlanDetailsType

        Dim dsFinancePlanDetails As DataSet = Nothing
        Dim oCoreBusiness As New CoreBusiness
        Dim oPFPlandetails As New BasePremiumFinancePlanDetailsType

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFPremiumFinance_Sel_Single")
            cmd.AddInParameter("@financeplancnt", SqlDbType.Int).Value = onPFPremFinanceKey
            cmd.AddInParameter("@financeplanversion", SqlDbType.Int).Value = onPFPremFinanceVersion
            cmd.AddInParameter("@nUserID", SqlDbType.Int).Value = nUserID

            dsFinancePlanDetails = conTransactions.ExecuteDataSet(cmd, "PremiumFinancePlanDet")
        End Using

        If dsFinancePlanDetails.Tables(0).Rows.Count > 0 Then
            Dim dr As DataRow = dsFinancePlanDetails.Tables(0).Rows(0)
            With oPFPlandetails
                .PFPremiumFinanceKey = Cast.ToInt32(dr.Item("pfprem_finance_cnt"), 0)
                .PFPremiumFinanceVersionKey = Cast.ToInt32(dr.Item("pfprem_finance_version"), 0)
                .PFSchemeName = Cast.ToString(dr.Item("SchemeName").ToString)
                .StartDate = Cast.ToDateTime(dr.Item("StartDate"), Nothing)
                .EndDate = Cast.ToDateTime(dr.Item("EndDate"), Nothing)
                .ProductClass = Cast.ToString(dr.Item("ProductClass").ToString)
                .TransType = Cast.ToString(dr.Item("TransType").ToString)
                .FinanceAmount = CDec(Cast.ToDouble(dr.Item("AmountToFinance"), 0))
                .APR = CDec(Cast.ToDouble(dr.Item("APR"), 0))
                .InterestRate = CDec(Cast.ToDouble(dr.Item("InterestRate"), 0))
                .DaysDelay = Cast.ToInt32(dr.Item("DaysDelay"), 0)
                .NoOfInstallments = Cast.ToInt32(dr.Item("NoofInstallments"), 0)
                .FirstInstallment = CDec(Cast.ToDecimal(dr.Item("FirstInstallment"), 0))
                .OtherInstallments = CDec(Cast.ToDecimal(dr.Item("OthInstallments"), 0))
                .CostOfProtection = CDec(Cast.ToDecimal(dr.Item("CostOfProtection"), 0))
                .Deposit = CDec(Cast.ToDouble(dr.Item("Deposit"), 0))
                .NetAmount = CDec(Cast.ToDouble(dr.Item("NetAmount"), 0))
                .TotalCost = CDec(Cast.ToDouble(dr.Item("TotalCost"), 0))
                .InterestCost = CDec(Cast.ToDouble(dr.Item("InterestCost"), 0))
                .MinFinanceChrge = CDec(Cast.ToDouble(dr.Item("MinFinanceChrge"), 0))
                .PayProtection = Cast.ToString(dr.Item("PayProtection").ToString)
                .ClientName = Cast.ToString(dr.Item("ClientName").ToString)
                .ClientAddress1 = Cast.ToString(dr.Item("ClientAddr1").ToString)
                .ClientAddress2 = Cast.ToString(dr.Item("ClientAddr2").ToString)
                .ClientAddress3 = Cast.ToString(dr.Item("ClientAddr3").ToString)
                .ClientAddress4 = Cast.ToString(dr.Item("ClientAddr4").ToString)
                .ClientTown = Cast.ToString(dr.Item("ClientTown").ToString)
                .ClientPcode = Cast.ToString(dr.Item("ClientPcode").ToString)
                .ClientCountry = Cast.ToString(dr.Item("ClientCountry").ToString)
                .ClientAreaCode = Cast.ToString(dr.Item("ClientAreaCode").ToString)
                .ClientPhoneNo = Cast.ToString(dr.Item("ClientPhoneNo").ToString)
                .ClientExtension = Cast.ToString(dr.Item("ClientExtension").ToString)
                .ClientFaxAreaCode = Cast.ToString(dr.Item("ClientFaxAreaCode").ToString)
                .ClientFaxNo = Cast.ToString(dr.Item("ClientFaxNo").ToString)
                .BankName = Cast.ToString(dr.Item("BankName").ToString)
                .BankSortCode = Cast.ToString(dr.Item("BankSortCode").ToString)
                .BankAccountNo = Cast.ToString(dr.Item("BankAccountNo").ToString)
                .BankAccountName = Cast.ToString(dr.Item("BankAccountName").ToString)
                .BankBranch = Cast.ToString(dr.Item("BankBranch").ToString)
                .BankAddress1 = Cast.ToString(dr.Item("BankAddr1").ToString)
                .BankAddress2 = Cast.ToString(dr.Item("BankAddr2").ToString)
                .BankAddress3 = Cast.ToString(dr.Item("BankAddr3").ToString)
                .BankTown = Cast.ToString(dr.Item("BankTown").ToString)
                .BankRegion = Cast.ToString(dr.Item("BankRegion").ToString)
                .BankPostCode = Cast.ToString(dr.Item("BankPCode").ToString)
                .BankCountry = Cast.ToString(dr.Item("BankCountry").ToString)
                .BankAreaCode = Cast.ToString(dr.Item("BankAreaCode").ToString)
                .BankPhoneNo = Cast.ToString(dr.Item("BankPhoneNo").ToString)
                .BankExtension = Cast.ToString(dr.Item("BankExtension").ToString)
                .BankFaxAreaCode = Cast.ToString(dr.Item("BankFaxAreaCode").ToString)
                .BankFaxNo = Cast.ToString(dr.Item("BankFaxNo").ToString)
                .StatusInd = CType([Enum].ToObject(GetType(FinancePlanStatus), oCoreBusiness.FinancePlanNumber(Convert.ToInt32(dr.Item("StatusInd")))), BaseImplementationTypes.FinancePlanStatus)
                .ClientCode = Cast.ToString(dr.Item("ClientCode").ToString)
                .AutoGeneratedPlanRef = Cast.ToString(dr.Item("AutoGeneratedPlanRef").ToString)
                .FinanceCollatedPlanRef = Cast.ToString(dr.Item("FinanceCollatedPlanRef").ToString)
                .InterestFree = Cast.ToString(dr.Item("InterestFree").ToString)
                .IsQuote = Cast.ToBoolean(dr.Item("IsQuote"), False)
                .PlanTransactionKey = Cast.ToInt32(dr.Item("PlanTransaction_id"), 0)
                .InsuranceFileKey = Cast.ToInt32(dr.Item("Insurance_File_Cnt"), 0)
                .PFRFKEY = Cast.ToInt32(dr.Item("pfrf_id"), 0)
                .PFFrequencyCode = Cast.ToString(dr.Item("Code").ToString)
                .BankCountryKey = Cast.ToInt32(dr.Item("bank_country"), 0)
                .ClientCountryKey = Cast.ToInt32(dr.Item("Client_Country"), 0)
                .FirstInstalmentDate = Cast.ToDateTime(dr.Item("First_Instalment_Date"), Nothing)
                .NextInstalmentDate = Cast.ToDateTime(dr.Item("Next_Instalment_Date"), Nothing)
                .LastInstalmentDate = Cast.ToDateTime(dr.Item("last_instalment_date"), Nothing)
                .TaxCost = Cast.ToDouble(dr.Item("Tax_Cost"), 0)
                .MediaTypeCode = Cast.ToString(dr.Item("mediatype_code"))
                .CCNumber = Cast.ToString(dr.Item("CC_Number"))
                .CCExpiryDate = Cast.ToString(dr.Item("CC_Expiry_Date"))
                .CCStartDate = Cast.ToString(dr.Item("CC_Start_Date"))
                .CCIssue = Cast.ToString(dr.Item("CC_Issue"))
                .CCPin = Cast.ToString(dr.Item("CC_Pin"))
                .MediaTypeValidationCode = Cast.ToString(dr.Item("MediaType_validatiON_code"))
                .bank_name_mandatory = Cast.ToBoolean(dr.Item("bank_name_mandatory"), False)
                .IsBankAddressMandatory = Cast.ToBoolean(dr.Item("bank_address_mandatory"), False)
                .IsBranchNameMandatory = Cast.ToBoolean(dr.Item("branch_name_mandatory"), False)
                .IsBranchCodeMandatory = Cast.ToBoolean(dr.Item("branch_code_mandatory"), False)
                .CommissionTransdetailKey = Cast.ToInt32(dr.Item("commission_transdetail_id"), 0)
                .PFFrequencyPeriod = Cast.ToString(dr.Item("Period"))
                .CommissionTransdetailKey = Cast.ToInt32(dr.Item("commissiON_transdetail_id"), 0)
                .PFFrequencyAmount = Cast.ToInt32(dr.Item("Amount"), 0)
                .PFFrequencyKey = Cast.ToInt32(dr.Item("pffrequency_id"), 0)
                .SourceKey = Cast.ToInt32(dr.Item("source_id"), 0)
                .ProductKey = Cast.ToInt32(dr.Item("product_id"), 0)
                .PFSchemeTypeCode = Cast.ToString(dr.Item("PFScheme_Type_Code"))
                .SchemeType = Cast.ToString(dr.Item("scheme_type"))
                .SchemeCode = Cast.ToString(dr.Item("scheme_code"))
                .FinanceFee = CDec(Cast.ToDouble(dr.Item("FinanceFee"), 0))
                .DayOfWeekOrMonth = Cast.ToInt32(dr.Item("DayOfWeekOrMonth"), 0)
                .PaymentMethod = Cast.ToString(dr.Item("PaymentMethod"))
                .PFFrequencyDesc = Cast.ToString(dr.Item("Frequency"))
                .SchemePrintType = Cast.ToString(dr.Item("SchemePrintType"))
                .OriginalAmount = Cast.ToDouble(dr.Item("Original_Amount"), 0)
                .LastInstalment = Cast.ToDouble(dr.Item("Last_Instalment"), 0)
                .ClaimDebtId = Cast.ToInt32(dr.Item("Claim_Debt_Id"), 0)
                .DateCreated = Cast.ToDateTime(dr.Item("Date_Created"), Nothing)
                .DateModified = Cast.ToDateTime(dr.Item("date_modified"), Nothing)
                .DateConfirmed = Cast.ToDateTime(dr.Item("date_confirmed"), Nothing)
                .DateReview = Cast.ToDateTime(dr.Item("Date_Review"), Nothing)
                .IsViaThirdParty = Cast.ToBoolean(dr.Item("is_via_third_party"), False)
                .IsDepositAsInstalment = Cast.ToBoolean(dr.Item("Deposit_As_Instalment"), False)
                .PFRFMnemonic = Cast.ToString(dr.Item("pfrf_mnemONic"))
                .CoverStartDate = Cast.ToDateTime(dr.Item("Cover_Start_Date"), Nothing)
                .ExpiryDate = Cast.ToDateTime(dr.Item("Expiry_Date"), Nothing)
                .Terms = Cast.ToInt32(dr.Item("Terms"), 0)
                .OriginalRate = Cast.ToDouble(dr.Item("OriginalRate"), 0)
                .IsCardHolder = Cast.ToBoolean(dr.Item("is_cardholder"), False)
                .CardHolderName = Cast.ToString(dr.Item("cardholder_name"))
                .CardHolderAddress1 = Cast.ToString(dr.Item("cardholder_address1"))
                .CardHolderAddress2 = Cast.ToString(dr.Item("cardholder_address2"))
                .CardHolderAddress3 = Cast.ToString(dr.Item("cardholder_address3"))
                .CardHolderAddress4 = Cast.ToString(dr.Item("cardholder_address4"))
                .CardHolderPostCode = Cast.ToString(dr.Item("cardholder_postcode"))
                .CardType = Cast.ToString(dr.Item("card_type"))
                .IsProviderCollectDeposit = Cast.ToBoolean(dr.Item("provider_collect_deposit"), False)
                .CardHolderPostCode = Cast.ToString(dr.Item("cardholder_postcode"))
                .DateBankDetailsChanged = Cast.ToDateTime(dr.Item("DateBankDetailsChanged"), Nothing)
                .IsDDCancelled = Cast.ToBoolean(dr.Item("dd_cancelled"), False)
                .IsCCCancelled = Cast.ToBoolean(dr.Item("cc_cancelled"), False)
                .IsPaperlessDD = Cast.ToBoolean(dr.Item("paperdd"), False)
                .PartyBankKey = Cast.ToInt32(dr.Item("party_bank_id"), 0)
                .PFPremiumFinanceCancelReason = Cast.ToString(dr.Item("PFCancelReasondesc"))
                .IsCancelPolicyRun = Cast.ToBoolean(dr.Item("is_cancel_policy_run"), False)
                .IsFinanceNetCommission = Cast.ToBoolean(dr.Item("finance_net_commissiON"), False)
                .IsDepositOnOtherMediaType = Cast.ToBoolean(dr.Item("deposit_ON_other_media_type"), False)
                .SettlementAmount = CDec(Cast.ToDouble(dr.Item("SettleAmount"), 0))
                .DepostiCCTrackingNumber = Cast.ToString(dr.Item("auth_code"))
                If Not IsDBNull(dr.Item("can_update_instalment_status")) AndAlso Val(dr.Item("can_update_instalment_status").ToString()) > 0 Then
                    .CanUserEditInstallmentStatusYN = True
                Else
                    .CanUserEditInstallmentStatusYN = False
                End If
                .Receipt_Difference_Option = Cast.ToInt32(dr.Item("receipt_difference_option"), 0)

            End With
        End If
        Return oPFPlandetails
    End Function
    '''<summary>
    '''This function is used to PremiumFinancePlanTransactions Details 
    '''</summary>
    '''<param name="conTransactions">SiriusConnection</param>
    '''<param name="onPFPremFinanceKey">Integer</param>
    '''<param name="onPFPremFinanceVersion">Integer</param>
    '''<param name="r_oSAMErrorCollection">SAMErrorCollection</param>
    '''<remarks></remarks>
    Private Function GetPFTransactionDetails(ByVal conTransactions As SiriusConnection,
                                             ByVal iPFPremFinanceKey As Integer,
                                             ByVal iPFPremFinanceVersion As Integer) As BasePremiumFinancePlanTransactionsType()
        Dim dtTransactions As DataTable = Nothing
        Dim oGetPFTransactionDetails() As BasePremiumFinancePlanTransactionsType = Nothing
        Dim IntArrIndex As Integer = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFGetTransactions")
            cmd.AddInParameter("@PremiumFinanceCnt", SqlDbType.Int).Value = iPFPremFinanceKey
            cmd.AddInParameter("@PremiumFinanceversion", SqlDbType.Int).Value = iPFPremFinanceVersion

            dtTransactions = conTransactions.ExecuteDataTable(cmd)
        End Using

        If dtTransactions.Rows.Count > 0 Then
            ReDim Preserve oGetPFTransactionDetails(dtTransactions.Rows.Count - 1)
            Dim oGetPFTransactionDetailsItem As BasePremiumFinancePlanTransactionsType = Nothing

            For Each drTrancount As DataRow In dtTransactions.Rows()
                oGetPFTransactionDetailsItem = New BasePremiumFinancePlanTransactionsType
                With oGetPFTransactionDetailsItem
                    .PFTransactionKey = Cast.ToInt32(drTrancount.Item("pftransaction_id"), 0)
                    .InsuranceRefIndex = Cast.ToInt32(drTrancount.Item("insurance_ref_index"), 0)
                    .Amount = Cast.ToDouble(drTrancount.Item("Amount"), 0)
                    .InsuranceFileKey = Cast.ToInt32(drTrancount.Item("insurance_file_cnt"), 0)
                    .TransDetailKey = Cast.ToInt32(drTrancount.Item("transdetail_id"), 0)
                    .DocRef = Cast.ToString(drTrancount.Item("document_ref"), "")
                    .AltRef = Cast.ToString(drTrancount.Item("alternate_reference"), "")
                    .EffectiveDate = Cast.ToDateTime(drTrancount.Item("cover_start_date"), Nothing)
                    .TransDate = Cast.ToDateTime(drTrancount.Item("document_date"), Nothing)
                    .MediaType = Cast.ToString(drTrancount.Item("media_type"), "")
                    .OutstandingAmount = Cast.ToDecimal(drTrancount.Item("outstanding_amount"), 0)
                    .MediaRef = Cast.ToString(drTrancount.Item("spare"), "")
                    .Accountkey = Cast.ToInt32(drTrancount.Item("account_id"), 0)
                    .AccountCode = Cast.ToString(drTrancount.Item("short_code"), "")
                    .Currency = Cast.ToString(drTrancount.Item("currency_description"), "")
                    .TaxBand = Cast.ToString(drTrancount.Item("code"), "")
                    .TransactionCurrenciesAmount = Cast.ToDecimal(drTrancount.Item("currency_amount"), 0)
                    .TransactionCurrency = Cast.ToString(drTrancount.Item("currency_description"), "")
                    .TransactionCurrencyCode = Cast.ToString(drTrancount.Item("currency_Code"), "")
                    .CurrencyDiff = 0
                    .SourceID = Cast.ToInt32(drTrancount.Item("company_id"), 0)
                    .PeriodName = Cast.ToString(drTrancount.Item("period_name"), "")
                    .DocType = Cast.ToString(drTrancount.Item("DocumentType_Descritpion"), "")
                    .DoctypeGroup = Cast.ToString(drTrancount.Item("DocumentTypeGroup_Description"), "")
                    .InsuranceRef = Cast.ToString(drTrancount.Item("insurance_ref"), "")
                    .Spare = Cast.ToString(drTrancount.Item("spare"), "")
                    .DocTypeID = Cast.ToInt32(drTrancount.Item("documenttype_id"), 0)
                    .PrimarySettled = Cast.ToString(IIf(Cast.ToBoolean(drTrancount.Item("fully_matched"), False) = True, "Y", "N"), "")
                End With

                oGetPFTransactionDetails(IntArrIndex) = oGetPFTransactionDetailsItem
                oGetPFTransactionDetailsItem = Nothing
                IntArrIndex = IntArrIndex + 1
            Next
        End If
        Return oGetPFTransactionDetails
    End Function
    '''<summary>
    '''This function is used to GetPFHistoryDetails  
    '''</summary>
    '''<param name="conTransactions">SiriusConnection</param>
    '''<param name="onPFPremFinanceKey">Integer</param>
    '''<param name="onPFPremFinanceVersion">Integer</param>
    '''<param name="r_oSAMErrorCollection">SAMErrorCollection</param>
    '''<remarks></remarks>
    Private Function GetPFHistoryDetails(ByVal conTransactions As SiriusConnection,
                                         ByVal iPFPremFinanceKey As Integer,
                                         ByRef r_oSAMErrorCollection As SAMErrorCollection) As BasePremiumFinancePlanHistoryType()

        Dim dtHistory As DataTable = Nothing
        Dim oCoreBusiness As New CoreBusiness
        Dim oGetPFHistoryDetails() As BasePremiumFinancePlanHistoryType = Nothing
        Dim IntArrIndex As Integer = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PFPremiumFinanceVersions_select")
            cmd.AddInParameter("@nPremiumFinanceCnt", SqlDbType.Int).Value = iPFPremFinanceKey
            dtHistory = conTransactions.ExecuteDataTable(cmd)
        End Using

        If dtHistory.Rows.Count > 0 Then
            Dim oGetPFHistoryDetailsItem As BasePremiumFinancePlanHistoryType = Nothing
            ReDim Preserve oGetPFHistoryDetails(dtHistory.Rows.Count - 1)

            For Each drPFHistory As DataRow In dtHistory.Rows()
                oGetPFHistoryDetailsItem = New BasePremiumFinancePlanHistoryType
                With oGetPFHistoryDetailsItem

                    .PFPremiumFinanceKey = Cast.ToInt32(drPFHistory.Item("pfPrem_finance_cnt"), 0)
                    .PFPremiumFinanceVersionKey = Cast.ToInt32(drPFHistory.Item("pfPrem_finance_version"), 0)
                    .StartDate = Cast.ToDateTime(drPFHistory.Item("StartDate"), Nothing)
                    .FinanceAmount = CDec(Cast.ToDouble(drPFHistory.Item("amounttofinance"), 0))
                    .TotalCost = CDec(Cast.ToDouble(drPFHistory.Item("TotalCost"), 0))
                    .StatusInd = CType([Enum].ToObject(GetType(FinancePlanStatus), oCoreBusiness.FinancePlanNumber(Convert.ToInt32(drPFHistory.Item("StatusInd")))), BaseImplementationTypes.FinancePlanStatus)
                    .AutoGeneratedPlanRef = Cast.ToString(drPFHistory.Item("AutoGeneratedPlanRef"))

                End With
                oGetPFHistoryDetails(IntArrIndex) = oGetPFHistoryDetailsItem
                oGetPFHistoryDetailsItem = Nothing
                IntArrIndex = IntArrIndex + 1
            Next
        End If

        Return oGetPFHistoryDetails

    End Function
    '''<summary>
    '''This function is used to Get PF BankHistory Details  
    '''</summary>
    '''<param name="conTransactions">SiriusConnection</param>
    '''<param name="onPFPremFinanceKey">Integer</param>
    '''<param name="onPFPremFinanceVersion">Integer</param>
    '''<param name="r_oSAMErrorCollection">SAMErrorCollection</param>
    '''<remarks></remarks>
    Private Function GetPFBankHistoryDetails(ByVal conTransactions As SiriusConnection,
                                             ByVal iPFPremFinanceKey As Integer,
                                             ByVal iPFPremFinanceVersion As Integer,
                                             ByRef r_oSAMErrorCollection As SAMErrorCollection) As BasePremiumFinancePlanBankHistoryType()
        Dim dtBankHistory As DataTable = Nothing
        Dim oGetPFBankHistoryDetails() As BasePremiumFinancePlanBankHistoryType = Nothing
        Dim IntArrIndex As Integer = 0

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PFMediaTypeHistory_sel")

            cmd.AddInParameter("@pfprem_finance_cnt", SqlDbType.Int).Value = iPFPremFinanceKey
            cmd.AddInParameter("@pfprem_finance_version", SqlDbType.Int).Value = iPFPremFinanceVersion
            dtBankHistory = conTransactions.ExecuteDataTable(cmd)
        End Using


        If dtBankHistory.Rows.Count > 0 Then
            Dim oGetPFBankHistoryDetailsItem As BasePremiumFinancePlanBankHistoryType = Nothing
            ReDim Preserve oGetPFBankHistoryDetails(dtBankHistory.Rows.Count - 1)

            For Each drPFBankHistory As DataRow In dtBankHistory.Rows()
                oGetPFBankHistoryDetailsItem = New BasePremiumFinancePlanBankHistoryType

                With oGetPFBankHistoryDetailsItem
                    .MediatypeValidationCode = Cast.ToString(drPFBankHistory.Item("mediatype_validation_code"))
                    .ActionCode = Cast.ToString(drPFBankHistory.Item("action_code"))
                    .BankAccountName = Cast.ToString(drPFBankHistory.Item("BankAccountName"))
                    .BankSortCode = Cast.ToString(drPFBankHistory.Item("BankSortCode"))
                    .BankAccountNo = Cast.ToString(drPFBankHistory.Item("BankAccountNo"))
                    .BankName = Cast.ToString(drPFBankHistory.Item("BankName"))
                    .BankBranch = Cast.ToString(drPFBankHistory.Item("BankBranch"))
                    .BankAddress1 = Cast.ToString(drPFBankHistory.Item("BankAddr1"))
                    .BankAddress2 = Cast.ToString(drPFBankHistory.Item("BankAddr2"))
                    .BankAddress3 = Cast.ToString(drPFBankHistory.Item("BankAddr3"))
                    .BankTown = Cast.ToString(drPFBankHistory.Item("BankTown"))
                    .BankPostCode = Cast.ToString(drPFBankHistory.Item("BankPCode"))
                    .BankRegion = Cast.ToString(drPFBankHistory.Item("BankRegion"))
                    .BankCountry = Cast.ToString(drPFBankHistory.Item("BankCountry"))
                    .BankAreaCode = Cast.ToString(drPFBankHistory.Item("BankAreaCode"))
                    .BankPhoneNo = Cast.ToString(drPFBankHistory.Item("BankPhoneNo"))
                    .BankExtension = Cast.ToString(drPFBankHistory.Item("BankExtension"))
                    .BankFaxAreaCode = Cast.ToString(drPFBankHistory.Item("BankFaxAreaCode"))
                    .BankFaxNo = Cast.ToString(drPFBankHistory.Item("BankFaxNo"))
                    .CCNumber = Cast.ToString(drPFBankHistory.Item("CC_Number"))
                    .CCExpiry_date = Cast.ToString(drPFBankHistory.Item("cc_expiry_date"))
                    .CCStartDate = Cast.ToString(drPFBankHistory.Item("CC_Start_Date"))
                    .CCIssue = Cast.ToString(drPFBankHistory.Item("CC_Issue"))
                    .CCPin = Cast.ToString(drPFBankHistory.Item("CC_Pin"))
                    .CardHolderName = Cast.ToString(drPFBankHistory.Item("cardholder_name"))
                    .CardHolderAddress1 = Cast.ToString(drPFBankHistory.Item("cardholder_address1"))
                    .CardHolderAddress2 = Cast.ToString(drPFBankHistory.Item("cardholder_address2"))
                    .CardHolderAddress3 = Cast.ToString(drPFBankHistory.Item("cardholder_address3"))
                    .CardHolderAddress4 = Cast.ToString(drPFBankHistory.Item("cardholder_address4"))
                    .CardHolderPostCode = Cast.ToString(drPFBankHistory.Item("cardholder_postcode"))
                    .DateModified = Cast.ToDateTime(drPFBankHistory.Item("Date_Modified"), Nothing)
                    .UserName = Cast.ToString(drPFBankHistory.Item("UserName"))
                    .PaperDD = Cast.ToBoolean(drPFBankHistory.Item("PaperDD"), False)
                    .PaymentType = Cast.ToString(drPFBankHistory.Item("PaymentType"))
                    .AccountType = Cast.ToString(drPFBankHistory.Item("AccountType"))
                End With
                oGetPFBankHistoryDetails(IntArrIndex) = oGetPFBankHistoryDetailsItem
                oGetPFBankHistoryDetailsItem = Nothing
                IntArrIndex = IntArrIndex + 1
            Next
        End If

        Return oGetPFBankHistoryDetails
    End Function
    '''<summary>
    '''This function is used to Get PF BankHistory Details  
    '''</summary>
    '''<param name="conTransactions">SiriusConnection</param>
    '''<param name="onPFPremFinanceKey">Integer</param>
    '''<param name="onPFPremFinanceVersion">Integer</param>
    '''<param name="r_oSAMErrorCollection">SAMErrorCollection</param>
    '''<remarks></remarks>
    Private Function GetPFInstalments(ByVal conTransactions As SiriusConnection,
                                      ByVal nPFPremFinanceKey As Integer,
                                      ByVal nPFPremFinanceVersion As Integer,
                                      ByVal r_oSAMErrorCollection As SAMErrorCollection) As BasePremiumFinancePlanInstalmentsType()
        Dim dsInstalments As DataSet = Nothing
        Dim oGetPFInstalments() As BasePremiumFinancePlanInstalmentsType = Nothing
        Dim oGetPFInstalmentsHistory() As BasePremiumFinancePlanInstalmentsHistoryType = Nothing
        Dim iHistory As Integer = 0
        Dim IntArrIndex As Integer = 0

        Dim nCurrentPfInstalmentsid As Integer = 0
        Dim nOldPfInstalmentsid As Integer = 0
        Dim dsHistory As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_PFInstalments_saa")
            cmd.AddInParameter("@nPfprem_finance_cnt", SqlDbType.Int).Value = nPFPremFinanceKey
            cmd.AddInParameter("@nPfprem_finance_version", SqlDbType.Int).Value = nPFPremFinanceVersion
            dsInstalments = conTransactions.ExecuteDataSet(cmd, "Instalments")
        End Using

        If dsInstalments IsNot Nothing AndAlso dsInstalments.Tables IsNot Nothing AndAlso dsInstalments.Tables.Count > 0 _
                                       AndAlso dsInstalments.Tables(0).Rows IsNot Nothing AndAlso dsInstalments.Tables(0).Rows.Count > 0 Then

            Dim dvInstalments As DataView = New DataView
            With dvInstalments
                .Table = dsInstalments.Tables(0)
                .Sort = "InstalmentNumber, PfInstalments_id"
            End With
            Dim dtInstalments As DataTable = Nothing
            dtInstalments = dvInstalments.ToTable

            '=======================Filling value in  Instalments[Start]===============================
            Dim oGetPFInstalmentsItem As BasePremiumFinancePlanInstalmentsType = Nothing
            For Each drPFInstalments As DataRow In dtInstalments.Rows()
                If Val(nOldPfInstalmentsid.ToString) <> Convert.ToInt32(Val(drPFInstalments.Item("PfInstalments_id").ToString)) Then

                    ReDim Preserve oGetPFInstalments(IntArrIndex)
                    oGetPFInstalmentsItem = New BasePremiumFinancePlanInstalmentsType

                    nCurrentPfInstalmentsid = Convert.ToInt32(drPFInstalments.Item("PfInstalments_id"))
                    With oGetPFInstalmentsItem

                        .PFInstalmentsKey = Cast.ToInt32(drPFInstalments.Item("PfInstalments_id"), 0)
                        .InstalmentNumber = Cast.ToInt32(drPFInstalments.Item("InstalmentNumber"), 0)
                        .DueDate = Cast.ToDateTime(drPFInstalments.Item("DueDate"), Nothing)
                        .Fee = Cast.ToDouble(drPFInstalments.Item("Fee"), 0)
                        .Amount = Cast.ToDouble(drPFInstalments.Item("Amount"), 0)
                        .TransactionDescription = Cast.ToString(drPFInstalments.Item("transaction_description"))
                        .StatusDescription = Cast.ToString(drPFInstalments.Item("status_description"))
                        .BatchRef = Cast.ToString(drPFInstalments.Item("Batch_Ref"))
                        .ExportDate = Cast.ToDateTime(drPFInstalments.Item("Export_Date"), Nothing)
                        .PostedDate = Cast.ToDateTime(drPFInstalments.Item("PostedDate"), Nothing)
                        .PFTransactionKey = Cast.ToInt32(drPFInstalments.Item("PFTransaction_id"), 0)
                        .Tax = Cast.ToDouble(drPFInstalments.Item("Tax"), 0)
                        .Commission = Cast.ToDouble(drPFInstalments.Item("Commission"), 0)
                        .InstalmentReason = Cast.ToString(drPFInstalments.Item("reason_description"))
                        .StatusCode = Cast.ToString(drPFInstalments.Item("status_Code"))
                        .InstalmentReasonCode = Cast.ToString(drPFInstalments.Item("pfinstalments_resultCode"))
                        .CurrencyDesc = Cast.ToString(drPFInstalments.Item("currency_desc"))

                        Dim dvTransaction As DataView = New DataView
                        With dvTransaction
                            .Table = dsInstalments.Tables(0)
                            .RowFilter = "PfInstalments_id =" & Convert.ToString(nCurrentPfInstalmentsid) & " And pfinstalments_history_id <> 0"
                        End With

                        .History = Nothing

                        If dvTransaction.ToTable.Rows.Count > 0 Then
                            Dim oGetPFInstalmentsHistoryitem As BasePremiumFinancePlanInstalmentsHistoryType = Nothing
                            ReDim oGetPFInstalmentsHistory(dvTransaction.ToTable.Rows.Count - 1)
                            iHistory = 0

                            For Each drPFIHistory As DataRow In dvTransaction.ToTable.Rows()
                                oGetPFInstalmentsHistoryitem = New BasePremiumFinancePlanInstalmentsHistoryType
                                If Not drPFIHistory.Item("pfinstalments_history_id") Is Nothing AndAlso CType(drPFIHistory.Item("pfinstalments_history_id"), Integer) <> 0 Then

                                    With oGetPFInstalmentsHistoryitem
                                        .PostedDate = Cast.ToDateTime(drPFIHistory.Item("PostedDate"), Nothing)
                                        .PFIStatusDescription = Cast.ToString(drPFIHistory.Item("status_description"))
                                        .PFIResultDescription = Cast.ToString(drPFIHistory.Item("pfinstalments_resultDesc"))
                                        .PFIResultCode = Cast.ToString(drPFIHistory.Item("pfinstalments_resultCode"))
                                    End With

                                    oGetPFInstalmentsHistory(iHistory) = oGetPFInstalmentsHistoryitem
                                    oGetPFInstalmentsHistoryitem = Nothing
                                    iHistory = iHistory + 1
                                End If

                            Next drPFIHistory
                            '=======================Filling value in  InstalmentsHistory[End]=================
                            oGetPFInstalmentsItem.History = oGetPFInstalmentsHistory

                        End If

                    End With
                    nOldPfInstalmentsid = nCurrentPfInstalmentsid
                    oGetPFInstalments(IntArrIndex) = oGetPFInstalmentsItem
                    oGetPFInstalmentsItem = Nothing
                    IntArrIndex = IntArrIndex + 1
                End If
            Next
            '=======================Filling value in  Instalments[End]====================================
        End If

        Return oGetPFInstalments
    End Function
    '''<summary>
    '''This function is used to Get PFInsurance key And PFPremFinanceVersion if both are equal zero and DocumentRef is carrieng some Reference
    '''</summary>
    '''<param name="con">SiriusConnection</param>
    '''<param name="o_nPFPremFinanceKey">Integer</param>
    '''<param name="o_nPFPremFinanceVersion">Integer</param>
    '''<param name="r_sDocumentRef">String</param>
    '''<remarks></remarks>
    Private Function GetPFKeyAndVersionFromDocRef(ByVal con As SiriusConnection,
                                             ByRef o_nPFPremFinanceKey As Integer,
                                             ByRef o_nPFPremFinanceVersion As Integer,
                                             ByVal r_sDocumentRef As String) As Boolean

        GetPFKeyAndVersionFromDocRef = False

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_get_fp_dets_from_doc_ref")
            cmd.AddInParameter("@document_ref", SqlDbType.VarChar).Value = r_sDocumentRef
            cmd.AddInParameter("@company_id", SqlDbType.Int).Value = 0

            Dim dsPFKeyAndVersiondet As New DataSet
            dsPFKeyAndVersiondet = con.ExecuteDataSet(cmd, "PFDet")

            If dsPFKeyAndVersiondet.Tables(0).Rows.Count > 0 Then
                Dim dr As DataRow = dsPFKeyAndVersiondet.Tables(0).Rows(0)
                With dsPFKeyAndVersiondet
                    o_nPFPremFinanceKey = CInt(Cast.ToInt32(dr.Item("pfprem_finance_cnt")))
                    o_nPFPremFinanceVersion = CInt(Cast.ToInt32(dr.Item("pfprem_finance_version")))
                End With
            End If

            GetPFKeyAndVersionFromDocRef = True
            cmd.Dispose()

        End Using

        Return GetPFKeyAndVersionFromDocRef
    End Function

#End Region

#Region "DeletePlan"
    ''' <summary>
    ''' Deletes Premium Finance Plan
    ''' </summary>
    ''' <param name="con">SiriusConnection Type</param>
    ''' <param name="oCancelPremiumFinancePlanRequest">BaseCancelPremiumFinancePlanRequestType Type</param>
    ''' <param name="PremiumFinance">Premium Finance array from spu_PFPremiumFinance_Sel_Single of object type</param>
    ''' <returns>BaseCancelPremiumFinancePlanResponseType</returns>
    ''' <remarks></remarks>
    Private Function DeletePlan(ByVal con As SiriusConnection,
                                    ByVal oCancelPremiumFinancePlanRequest As BaseCancelPremiumFinancePlanRequestType,
                                    ByVal aoPremiumFinance(,) As Object,
                                    ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business) As BaseCancelPremiumFinancePlanResponseType



        Dim oResponse As New BaseCancelPremiumFinancePlanResponseType
        Dim oSamErrorCollection As New SAMErrorCollection
        Dim nTypeOfPackage As enumTypeOfPackage
        If oCancelPremiumFinancePlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If


        Dim nReturnState As Integer
        Dim nComReturnValue As Integer

        Try
            con.BeginTransaction()
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_PFPremiumFinance_Delete")
                cmd.AddInParameter("@pfprem_finance_cnt", SqlDbType.Int).Value = CType(aoPremiumFinance(k_PFPlanPFCnt, 0), Integer)
                cmd.AddInParameter("@pfprem_finance_version", SqlDbType.Int).Value = CInt(aoPremiumFinance(k_PFPlanPFVersion, 0))
                cmd.AddOutParameter("@returnstate", SqlDbType.Int)
                con.ExecuteNonQuery(cmd)
                nReturnState = Cast.ToInt32(cmd.Parameters.Item("@returnstate").Value, 0)
            End Using

            If nReturnState <> 1 Then
                oSamErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.GeneralFailure,
                                                   "Unable to delete Plan",
                                                   "InstalmentPlanStatus")
                oSamErrorCollection.CheckForErrors()
            End If

            nComReturnValue = r_oPremiumFinanceBusiness.UpdatePaymentMethod(CType((aoPremiumFinance(k_PFPlanInsuranceFileCnt, 0)), Integer))

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.UpdatePaymentMethod", nComReturnValue)
            End If

            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
            Throw ex
        End Try

        Return oResponse

    End Function
#End Region

#Region "CancelPlan"
    ''' <summary>
    ''' Cancels the Premium Finance Plan
    ''' </summary>
    ''' <param name="con">SiriusConnection Type</param>
    ''' <param name="oCancelPremiumFinancePlanRequest">BaseCancelPremiumFinancePlanRequestType Type</param>
    ''' <param name="PremiumFinance">Premium Finance array from spu_PFPremiumFinance_Sel_Single of object type</param>
    ''' <returns>BaseCancelPremiumFinancePlanResponseType</returns>
    ''' <remarks></remarks>
    Private Function CancelPlan(ByVal con As SiriusConnection,
                                    ByVal oCancelPremiumFinancePlanRequest As BaseCancelPremiumFinancePlanRequestType,
                                    ByVal aoPremiumFinance(,) As Object,
                                    ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business) As BaseCancelPremiumFinancePlanResponseType

        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oSamErrorCollection As New SAMErrorCollection
        Dim nCancelReasonId As Integer
        Dim nComReturnValue As Integer
        Dim dtLapseDate As Date
        Dim oResponse As New BaseCancelPremiumFinancePlanResponseType

        Dim nTypeOfPackage As enumTypeOfPackage

        If oCancelPremiumFinancePlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If

        Try

            con.BeginTransaction()

            If oCancelPremiumFinancePlanRequest.ReasonCode <> "" Then

                nCancelReasonId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                               PMLookupTable.PFCancelReason,
                                                                               oCancelPremiumFinancePlanRequest.ReasonCode,
                                                                               "ReasonCode", oSamErrorCollection)
                oSamErrorCollection.CheckForErrors()
                aoPremiumFinance(k_PFCancelReasonId, 0) = nCancelReasonId
            End If

            Dim oCredits As Object
            Dim oDebits As Object

            'CancelPlan and return the credit and debit amounts
            nComReturnValue = r_oPremiumFinanceBusiness.CancelPlanInHouse(aoPremiumFinance, 0, oCredits, oDebits)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CancelPlanInHouse", nComReturnValue)
            End If

            Dim aoPlanCancellationTransaction(1, 0) As Object


            aoPlanCancellationTransaction(0, 0) = oCredits
            aoPlanCancellationTransaction(1, 0) = oDebits

            If Convert.ToInt32(aoPremiumFinance(k_PFPlanSchemeType_ID, 0)) <> 1 Then
                nComReturnValue = r_oPremiumFinanceBusiness.SavePlanCancellationTransactions(CType(aoPremiumFinance(k_PFPlanPFCnt, 0), Integer),
                                                                                            CType(aoPremiumFinance(k_PFPlanPFVersion, 0), Integer),
                                                                                            aoPlanCancellationTransaction)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.SavePlanCancellationTransactions", nComReturnValue)
                End If
            End If

            'Update Status
            nComReturnValue = r_oPremiumFinanceBusiness.StatusUpdate(ToSafeInteger(aoPremiumFinance(k_PFPlanPFCnt, 0)),
                                                                        ToSafeInteger(aoPremiumFinance(k_PFPlanPFVersion, 0)),
                                                                        InstalmentPlanStatus.Cancelled,
                                                                        nCancelReasonId)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate", nComReturnValue)
            End If

            oResponse.DebitTransdetailKey = CType(oDebits, Integer)

            'Create event
            nComReturnValue = r_oPremiumFinanceBusiness.CreateEvent((ToSafeInteger(PMEComponentAction.PMDelete)).ToString(),
                                                                       ToSafeInteger(aoPremiumFinance(k_PFPlanInsuranceFileCnt, 0), 0),
                                                                       CType(aoPremiumFinance(k_PFPlanClientId, 0), Integer),
                                                                       CType(aoPremiumFinance(k_PFPlanAutoGenPlanRef, 0), String))

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CreateEvent", nComReturnValue)
            End If

            Dim dtCoverStartDate As Date
            Dim dtCoverExpiryDate As Date
            Dim oPolicyPaidToDate(,) As Object
            Dim aoPolicyPaidToDateArray(,) As System.Array
            Dim crTotalPremium As Decimal
            Dim crTotalPaidToDate As Decimal
            Dim bTryParse As Boolean

            dtCoverStartDate = ToSafeDate(aoPremiumFinance(k_PFPlanCoverStartDate, 0))
            dtCoverExpiryDate = ToSafeDate(aoPremiumFinance(k_PFPlanCoverEndDate, 0))

            nComReturnValue = r_oPremiumFinanceBusiness.GetPolicyPaidToDate(CType(aoPremiumFinance(k_PFPlanPFCnt, 0), Integer),
                                                                                oPolicyPaidToDate,
                                                                                CType(aoPremiumFinance(k_PFPlanPFVersion, 0), Integer))
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetPolicyPaidToDate", nComReturnValue)
            End If

            If IsArray(oPolicyPaidToDate) Then

                Dim aoPremiumFinanceArray As Object(,) = oPolicyPaidToDate
                If CStr(aoPremiumFinanceArray(0, 0)) = "" Then
                    crTotalPremium = CDec(0)
                Else
                    crTotalPremium = CDec(aoPremiumFinanceArray(0, 0))
                End If

                If CStr(aoPremiumFinanceArray(1, 0)) = "" Then
                    crTotalPaidToDate = CDec(0)
                Else
                    crTotalPaidToDate = CDec(aoPremiumFinanceArray(1, 0))
                End If
                Dim nDays As Integer = CType(((crTotalPaidToDate / crTotalPremium) * Informations.WCFDateDiff(DateInterval.Day, dtCoverStartDate,
                                                     dtCoverExpiryDate,
                                                     FirstDayOfWeek.Sunday,
                                                     FirstWeekOfYear.FirstJan1)), Integer)

                dtLapseDate = dtCoverStartDate.AddDays(nDays)
            Else
                dtLapseDate = dtCoverStartDate
            End If

            ReDim Preserve oResponse.PFPolicies(0)
            oResponse.PFPolicies(0) = New BaseCancelPremiumFinancePlanResponseTypePolicies
            oResponse.PFPolicies(0).InsuranceFileKey = CInt(aoPremiumFinance(k_PFPlanInsuranceFileCnt, 0))
            oResponse.PFPolicies(0).CalculatedLapsedDate = dtLapseDate


            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
            Throw ex
        End Try


        Return oResponse

    End Function
#End Region

#Region "SettlePlan"
    ''' <summary>
    ''' Settles the the plan
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oCancelPremiumFinancePlanRequest"></param>
    ''' <param name="PremiumFinance">Premium Finance array from spu_PFPremiumFinance_Sel_Single of object type</param>
    ''' <returns>BaseCancelPremiumFinancePlanResponseType</returns>
    ''' <remarks></remarks>
    Private Function SettlePlan(ByVal con As SiriusConnection,
                                    ByVal oCancelPremiumFinancePlanRequest As BaseCancelPremiumFinancePlanRequestType,
                                    ByVal aoPremiumFinance(,) As Object,
                                    ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business) As BaseCancelPremiumFinancePlanResponseType

        Dim oCredits As Object
        Dim oDebits As Object
        Dim nComReturnValue As Integer
        Dim oResponse As New BaseCancelPremiumFinancePlanResponseType


        Dim nTypeOfPackage As enumTypeOfPackage
        If oCancelPremiumFinancePlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.CancelPremiumFinancePlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If


        Try

            con.BeginTransaction()
            'Settles the complete plan
            nComReturnValue = r_oPremiumFinanceBusiness.CancelPlanInHouse(aoPremiumFinance, 0, oCredits, oDebits)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CancelPlanInHouse", nComReturnValue)
            End If

            nComReturnValue = r_oPremiumFinanceBusiness.StatusUpdate(ToSafeInteger(aoPremiumFinance(k_PFPlanPFCnt, 0)),
                                                                        ToSafeInteger(aoPremiumFinance(k_PFPlanPFVersion, 0)),
                                                                        InstalmentPlanStatus.Completed)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate", nComReturnValue)
            End If

            oResponse.DebitTransdetailKey = CType(oDebits, Integer)

            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
            Throw ex
        End Try

        Return oResponse
    End Function
#End Region

#Region "GetMatchingQuote"
    ''' <summary>
    ''' Tries to find the FinancePlan based requested values
    ''' </summary>
    ''' <param name="r_oPremiumFinanceBusiness">bSIRPremiumFinance.Business Type</param>
    ''' <param name="r_oProcessPFPlanRequest">BaseProcessPFPlanRequestType Type</param>
    ''' <param name="r_aoInstalmentQuotes">Object(,) Type</param>
    ''' <param name="r_nMatchingRatePositionInQuoteArray">Integer Type</param>
    ''' <param name="r_oFinancePlanTransArray">Object(,)</param>
    ''' <param name="nPartyKey">Integer</param>
    ''' <remarks></remarks>
    Private Sub GetMatchingQuote(ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business,
                                   ByRef r_oProcessPFPlanRequest As BaseProcessPFPlanRequestType,
                                   ByRef r_aoInstalmentQuotes As Object(,),
                                   ByRef r_nMatchingRatePositionInQuoteArray As Integer,
                                   ByRef r_oFinancePlanTransArray As Object(,),
                                   ByVal nPartyKey As Integer,
                                   ByVal r_oCon As SiriusConnection)

        Const kQuoteSchemeNo As Integer = 2
        Const kQuoteSchemeVersion As Integer = 3
        Const kQuoteRateId As Integer = 32

        Dim oSamErrorCollection As New SAMErrorCollection
        Dim nComReturnValue As Integer

        Dim sTransactionType As String = String.Empty
        Dim dFinanceAmount As Decimal
        Dim nRenewalInsuranceFileCnt As Integer = 0

        'Calculate_Quotes( parameters values should be same as BO
        r_oProcessPFPlanRequest.PFQuote.QuoteDate = Date.MinValue
        r_oProcessPFPlanRequest.PFQuote.StartDate = Date.MinValue
        r_oProcessPFPlanRequest.PFQuote.EndDate = Date.MinValue
        'Nexus sends the PreferredDate as ddlPaymentDate selected - DaysDelay, corrected it in nexus
        r_oProcessPFPlanRequest.PFQuote.PreferredDate = (r_oProcessPFPlanRequest.PFQuote.PreferredDate) '.AddDays(r_oProcessPFPlanRequest.PFQuote.DaysDelay)

        If r_oProcessPFPlanRequest.TransType = ProcessPFPlanType.NewPlan Then
            sTransactionType = "NB"
            dFinanceAmount = 0
            ' amount will be calculated from transacion array we are sending, 
            ' in bSIRPRemiumFinance.GetMissingQuoteData and TaxNotIncluded will be subtracted from it.
            ' if we send FinanceAmount, the FinanceAmount - TaxNotIncluded gives a wrong amount


            Dim nInsuranceFileTypeId As Integer = 0
            Dim nInsuranceFileTypeCode As String = String.Empty
            Dim nPolicyVersion As Integer = 0

            GetInsuranceFileType(r_oCon, r_oProcessPFPlanRequest.PFQuote.InsuranceFileKey, nInsuranceFileTypeId, nInsuranceFileTypeCode, nPolicyVersion)

            Select Case nInsuranceFileTypeCode
                Case InsuranceFileType.LivePolicy, InsuranceFileType.Quote, InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated
                    If (InsuranceFileType.LivePolicy = Trim(nInsuranceFileTypeCode)) AndAlso nPolicyVersion > 1 Then
                        sTransactionType = "REN"
                    Else
                        sTransactionType = "NB"
                    End If
                Case InsuranceFileType.MTAPermanentQuotation, InsuranceFileType.MTATemporaryQuotation,
                    InsuranceFileType.MTAPermanent, InsuranceFileType.MTATemporary, InsuranceFileType.MTACancellation,
                    InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated, InsuranceFileType.MTAQuotationCancellation

                    sTransactionType = "MTA"

                Case InsuranceFileType.Renewal
                    sTransactionType = "REN"

            End Select

            Dim originalInsuranceFileCnt As Integer = 0
            Dim premiumFinanceCnt As Integer = 0
            Dim premiumFinanceVersion As Integer = 0


            If r_oProcessPFPlanRequest.PFQuote.ProcessPFMode IsNot Nothing AndAlso
                (r_oProcessPFPlanRequest.PFQuote.ProcessPFMode = "MTA" OrElse
                r_oProcessPFPlanRequest.PFQuote.ProcessPFMode = "REN") Then
                ' If it's an MTA or Renewal then get the original premium finance details
                GetOriginalInsuranceFileDetails(r_oCon, r_oProcessPFPlanRequest.PFQuote.InsuranceFileKey, originalInsuranceFileCnt, premiumFinanceCnt, premiumFinanceVersion)
                If originalInsuranceFileCnt <> 0 Then
                    nRenewalInsuranceFileCnt = r_oProcessPFPlanRequest.PFQuote.InsuranceFileKey
                    r_oProcessPFPlanRequest.PFQuote.InsuranceFileKey = originalInsuranceFileCnt
                    r_oProcessPFPlanRequest.PFQuote.PFPremiumFinanceKey = premiumFinanceCnt
                    r_oProcessPFPlanRequest.PFPremFinanceKey = premiumFinanceCnt
                    r_oProcessPFPlanRequest.PFQuote.PFPremiumFinanceVersionKey = premiumFinanceVersion
                    r_oProcessPFPlanRequest.PFPremFinanceVersion = premiumFinanceVersion
                End If
            End If


        ElseIf r_oProcessPFPlanRequest.TransType = ProcessPFPlanType.PlanMTA Then
            sTransactionType = "MTA"
            dFinanceAmount = CDec(r_oProcessPFPlanRequest.PFQuote.FinanceAmount)
        End If

        If r_oProcessPFPlanRequest.PFQuote.ProcessPFMode IsNot Nothing AndAlso
            r_oProcessPFPlanRequest.PFQuote.ProcessPFMode <> "" Then

            Select Case r_oProcessPFPlanRequest.PFQuote.ProcessPFMode.ToUpper()
                Case "MTA"
                    sTransactionType = "MTA"
                Case "NB"
                    sTransactionType = "NB"
                Case "REN"
                    sTransactionType = "REN"
            End Select

        End If



        Dim nDayOfWeek As Integer = 1
        Dim nDayOfMonth As Integer = 1


        If Trim(r_oProcessPFPlanRequest.PFQuote.PFFrequencyCode).ToUpper() = "W" Then
            nDayOfWeek = r_oProcessPFPlanRequest.PFQuote.DayOfWeekOrMonth
        ElseIf Trim(r_oProcessPFPlanRequest.PFQuote.PFFrequencyCode).ToUpper() = "M" Then
            nDayOfMonth = r_oProcessPFPlanRequest.PFQuote.DayOfWeekOrMonth
        End If

        'Explicitly Assign the value to setting the correct dayofmonth from Portal.
        r_oPremiumFinanceBusiness.m_iDayOfWeekOrMonth = 1
        Dim oInstalmentQuoteArray As Object
        'OverrideInterestRate should be -1 checkbox not checked
        nComReturnValue = r_oPremiumFinanceBusiness.Calculate_Quotes(v_lSourceID:=r_oProcessPFPlanRequest.SourceId,
                                                                         v_sProductCode:=sTransactionType,
                                                                         v_dtQuoteDate:=r_oProcessPFPlanRequest.PFQuote.QuoteDate,
                                                                         v_dtStartDate:=r_oProcessPFPlanRequest.PFQuote.StartDate,
                                                                         v_dtEndDate:=r_oProcessPFPlanRequest.PFQuote.EndDate,
                                                                         v_dtPreferredDate:=r_oProcessPFPlanRequest.PFQuote.PreferredDate,
                                                                         v_iDayInMonth:=CShort(nDayOfMonth),
                                                                         v_iDayInWeek:=CShort(nDayOfWeek),
                                                                         v_crAmountToFinance:=dFinanceAmount,
                                                                         v_bPaymentProtection:=r_oProcessPFPlanRequest.PFQuote.PaymentProtection,
                                                                         v_dInterestOverrideRate:=r_oProcessPFPlanRequest.PFQuote.OverrideInterestRate,
                                                                         v_bOverrideCommission:=r_oProcessPFPlanRequest.PFQuote.OverrideCommission,
                                                                         v_dOverrideDeposit:=r_oProcessPFPlanRequest.PFQuote.Deposit,
                                                                         v_lPartyCnt:=nPartyKey,
                                                                         r_vQuoteArray:=oInstalmentQuoteArray,
                                                                         v_lInsuranceFileCnt:=r_oProcessPFPlanRequest.PFQuote.InsuranceFileKey,
                                                                         v_lRenewalInsFileCnt:=nRenewalInsuranceFileCnt,
                                                                         r_lPremFinanceCnt:=r_oProcessPFPlanRequest.PFQuote.PFPremiumFinanceKey,
                                                                         r_lPremFinanceVer:=r_oProcessPFPlanRequest.PFQuote.PFPremiumFinanceVersionKey,
                                                                         v_vPFTransArray:=r_oFinancePlanTransArray,
                                                                         v_iMTAType:=r_oProcessPFPlanRequest.Type)

        If nComReturnValue <> PMEReturnCode.PMTrue Then
            RaiseComMethodException("bSIRPremiumFinance.Business.Calculate_Quotes", nComReturnValue)
        End If

        r_aoInstalmentQuotes = CType(oInstalmentQuoteArray, Object(,))

        Dim bFoundSelectedRate As Boolean = False

        'Loop through the Plan array and find the plan based on
        'SelectedSchemeNo , SelectedSchemeVersion , PFRFKEY coming from the request
        If IsArray(r_aoInstalmentQuotes) Then
            Dim nLowerBound As Integer = r_aoInstalmentQuotes.GetLowerBound(1)
            Dim nUpperBound As Integer = r_aoInstalmentQuotes.GetUpperBound(1)

            For lPosition As Integer = nLowerBound To nUpperBound

                Dim nSchemeNo As Integer = Cast.ToInt32(r_aoInstalmentQuotes(kQuoteSchemeNo, lPosition), 0)
                Dim nSchemeVersion As Integer = Cast.ToInt32(r_aoInstalmentQuotes(kQuoteSchemeVersion, lPosition), 0)
                Dim nRateId As Integer = Cast.ToInt32(r_aoInstalmentQuotes(kQuoteRateId, lPosition), 0)


                If nSchemeNo = r_oProcessPFPlanRequest.PFQuote.SelectedSchemeNo AndAlso
                nSchemeVersion = r_oProcessPFPlanRequest.PFQuote.SelectedSchemeVersion AndAlso
                nRateId = r_oProcessPFPlanRequest.PFQuote.PFRFKEY Then

                    r_nMatchingRatePositionInQuoteArray = lPosition
                    bFoundSelectedRate = True
                    Exit For
                End If
            Next
        End If



        If Not bFoundSelectedRate Then
            oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.NoMatchingInstalmentQuoteFoundForSpecifiedDetails,
                                                SAMBusinessErrors.NoMatchingInstalmentQuoteFoundForSpecifiedDetails.ToString(),
                                                "Instalment Quote not found for scheme no:" & r_oProcessPFPlanRequest.PFQuote.SelectedSchemeNo.ToString() &
                                                "scheme version: " & r_oProcessPFPlanRequest.PFQuote.SelectedSchemeVersion.ToString() &
                                                "pfrf_id: " & r_oProcessPFPlanRequest.PFQuote.PFRFKEY.ToString())


            oSamErrorCollection.CheckForErrors()
        End If

    End Sub
#End Region

#Region "ValidateAndPopulatePFPlan"

    ''' <summary>
    ''' Validate and Populate the relevant PartyBank Details
    ''' </summary>
    ''' <param name="r_oProcessPFPlanRequest">BaseProcessPFPlanRequestType Type</param>
    ''' <param name="r_aoPremiumFinanceArray">Object(, Type</param>
    ''' <param name="r_sBankDetailsAction">String Type</param>
    ''' <remarks></remarks>
    Private Sub ValidateAndPopulatePFPlan(ByRef r_oProcessPFPlanRequest As BaseProcessPFPlanRequestType,
                                          ByRef r_aoPremiumFinanceArray As Object(,),
                                          ByRef r_sBankDetailsAction As String)

        Dim oCoreBusiness As New CoreBusiness
        Dim oSAMErrorCollection As New SAMErrorCollection
        Dim bCCCancelled As Boolean
        Dim bDDCancelled As Boolean
        Dim bPaperDD As Boolean
        Dim bResult As Boolean
        Try


            PopulatePFPartyBank(r_oProcessPFPlanRequest, r_aoPremiumFinanceArray, r_sBankDetailsAction)

            bResult = Boolean.TryParse(CType(r_aoPremiumFinanceArray(k_PFPlanCCCancelled, 0), String), bCCCancelled)
            bResult = Boolean.TryParse(CType(r_aoPremiumFinanceArray(k_PFPlanDDCancelled, 0), String), bDDCancelled)

            If CType(r_aoPremiumFinanceArray(k_PFPlanPaperDD, 0), String) = "1" Then
                bPaperDD = True
            End If

            ' Check if CC or DD cancelled checkbox have been set at interface
            If r_oProcessPFPlanRequest.PFQuote.IsCCCancelled Or r_oProcessPFPlanRequest.PFQuote.IsDDCancelled Then
                If (r_oProcessPFPlanRequest.PFQuote.IsCCCancelled <> bCCCancelled) _
                Or (r_oProcessPFPlanRequest.PFQuote.IsDDCancelled <> bDDCancelled) Then

                    r_sBankDetailsAction = "Cancellation"

                End If
            End If

            If r_oProcessPFPlanRequest.PFQuote.IsPaperlessDD <> bPaperDD Then
                r_sBankDetailsAction = "Amendment"
            End If



            r_aoPremiumFinanceArray(k_PFPlanStatusInd, 0) = oCoreBusiness.FinancePlanStatusString(CType([Enum].ToObject(GetType(FinancePlanStatus), r_oProcessPFPlanRequest.PFQuote.StatusInd), CoreBusiness.FinancePlanStatus))
            r_aoPremiumFinanceArray(k_PFPlanClientName, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientName)
            r_aoPremiumFinanceArray(k_PFPlanClientAddress1, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientAddress1)
            r_aoPremiumFinanceArray(k_PFPlanClientAddress2, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientAddress2)
            r_aoPremiumFinanceArray(k_PFPlanClientAddress3, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientAddress3)
            r_aoPremiumFinanceArray(k_PFPlanClientAddress4, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientAddress4)
            r_aoPremiumFinanceArray(k_PFPlanClientTown, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientTown)
            r_aoPremiumFinanceArray(k_PFPlanClientPostcode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientPcode)
            r_aoPremiumFinanceArray(k_PFPlanClientCountry_ID, 0) = Cast.ToInt32(r_oProcessPFPlanRequest.PFQuote.ClientCountryKey, 0)
            r_aoPremiumFinanceArray(k_PFPlanClientCountry, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientCountry)
            r_aoPremiumFinanceArray(k_PFPlanClientAreaCode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientAreaCode)
            r_aoPremiumFinanceArray(k_PFPlanClientPhone, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientPhoneNo)
            r_aoPremiumFinanceArray(k_PFPlanClientExtn, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientExtension)
            r_aoPremiumFinanceArray(k_PFPlanClientFaxCode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientFaxAreaCode)
            r_aoPremiumFinanceArray(k_PFPlanClientFax, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientFaxNo)
            r_aoPremiumFinanceArray(k_PFPlanAutoGenPlanRef, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef)
            r_aoPremiumFinanceArray(k_PfPlanDateBankDetailsChanged, 0) = Cast.ToDateTime(r_oProcessPFPlanRequest.PFQuote.DateBankDetailsChanged, Date.Today)
            r_aoPremiumFinanceArray(k_PFPlanDDCancelled, 0) = IIf(r_oProcessPFPlanRequest.PFQuote.IsDDCancelled = True, 1, 0)
            r_aoPremiumFinanceArray(k_PFPlanCCCancelled, 0) = IIf(r_oProcessPFPlanRequest.PFQuote.IsCCCancelled = True, 1, 0)
            r_aoPremiumFinanceArray(k_PFPlanPaperDD, 0) = IIf(r_oProcessPFPlanRequest.PFQuote.IsPaperlessDD = True, 1, 0)
            'r_aoPremiumFinanceArray(k_PFPlanStartDate, 0) = Cast.ToDateTime(r_oProcessPFPlanRequest.PFQuote.StartDate, Date.MinValue)
            r_aoPremiumFinanceArray(k_PFPlanDateConfirmed, 0) = Cast.ToDateTime(r_oProcessPFPlanRequest.PFQuote.DateConfirmed, Date.MinValue)
            r_aoPremiumFinanceArray(k_PFPlanDateReview, 0) = Cast.ToDateTime(r_oProcessPFPlanRequest.PFQuote.DateReview, Date.MinValue)
            r_aoPremiumFinanceArray(k_PFPlanClientCode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.ClientCode)
            r_aoPremiumFinanceArray(k_PFPlanPFRF_ID, 0) = Cast.ToInt32(r_oProcessPFPlanRequest.PFQuote.PFRFKEY, 0)
            r_aoPremiumFinanceArray(k_PFPlanDateCreated, 0) = Cast.ToDateTime(r_oProcessPFPlanRequest.PFQuote.DateCreated, Date.Today)


        Catch ex As Exception
            Throw New ApplicationException("An exception occured in ValidateAndPopulatePFPlan", ex)
        Finally
            oCoreBusiness = Nothing
        End Try



    End Sub
#End Region

#Region "PopulatePFPartyBank"

    ''' <summary>
    ''' Populate the relevant PartyBank Details
    ''' </summary>
    ''' <param name="r_oProcessPFPlanRequest">BaseProcessPFPlanRequestType Type</param>
    ''' <param name="r_aoPremiumFinanceArray">Object(, Type</param>
    ''' <param name="r_sBankDetailsAction">String Type</param>
    ''' <remarks></remarks>
    Private Sub PopulatePFPartyBank(ByRef r_oProcessPFPlanRequest As BaseProcessPFPlanRequestType,
                                    ByRef r_aoPremiumFinanceArray As Object(,),
                                    ByRef r_sBankDetailsAction As String)

        'Uncomment if required
        If r_oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing Then
            'r_aoPremiumFinanceArray(k_PFPlanBankAccountName, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.NameOnCreditCard)
            'r_aoPremiumFinanceArray(k_PFPlanCCNumber, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.Number)
            'r_aoPremiumFinanceArray(k_PFPlanCCExpiryDate, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.ExpiryDate)
            'r_aoPremiumFinanceArray(k_PFPlanCCStartDate, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.StartDate)
            'r_aoPremiumFinanceArray(k_PFPlanCCIssue, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.Issue)
            'r_aoPremiumFinanceArray(k_PFPlanCCPin, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.Pin)
            'r_aoPremiumFinanceArray(k_PFPlanCardType, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.TypeCode)
            r_aoPremiumFinanceArray(k_PFPlanAuthCode, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.AuthCode)

            'If Cast.ToInt32(r_oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey, 0) > 0 Then
            '    r_aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = r_oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey
            'End If

            'If r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder IsNot Nothing Then
            '    r_aoPremiumFinanceArray(k_PFPlanIsCardholder, 0) = IIf(r_oProcessPFPlanRequest.PFQuote.IsCardHolder = True, 1, 0)
            '    r_aoPremiumFinanceArray(k_PfPlanCardholderName, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder.Name)
            '    r_aoPremiumFinanceArray(k_PfPlanCardholderAddress1, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder.AddressLine1)
            '    r_aoPremiumFinanceArray(k_PfPlanCardholderAddress2, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder.AddressLine2)
            '    r_aoPremiumFinanceArray(k_PfPlanCardholderAddress3, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder.AddressLine3)
            '    r_aoPremiumFinanceArray(k_PfPlanCardholderAddress4, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder.AddressLine4)
            '    r_aoPremiumFinanceArray(k_PfPlanCardholderPostcode, 0) = Trim(r_oProcessPFPlanRequest.PFCreditCardDetails.CardHolder.PostCode)
            'End If
        End If



        If CType(r_aoPremiumFinanceArray(k_PFPlanBankSortCode, 0), String) <> Trim(r_oProcessPFPlanRequest.PFQuote.BankSortCode) Or
        CType(r_aoPremiumFinanceArray(k_PFPlanBankAccountNo, 0), String) <> Trim(r_oProcessPFPlanRequest.PFQuote.BankAccountNo) Or
        CType(r_aoPremiumFinanceArray(k_PFPlanBankBranch, 0), String) <> Trim(r_oProcessPFPlanRequest.PFQuote.BankBranch) Or
        CType(r_aoPremiumFinanceArray(k_PFPlanBankAccountName, 0), String) <> Trim(r_oProcessPFPlanRequest.PFQuote.BankAccountName) Then

            r_sBankDetailsAction = "Amendment"

        End If



        r_aoPremiumFinanceArray(k_PFPlanBankName, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankName)
        r_aoPremiumFinanceArray(k_PFPlanBankSortCode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankSortCode)
        r_aoPremiumFinanceArray(k_PFPlanBankAccountNo, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAccountNo)
        r_aoPremiumFinanceArray(k_PFPlanBankAccountName, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAccountName)
        r_aoPremiumFinanceArray(k_PFPlanBankBranch, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankBranch)
        r_aoPremiumFinanceArray(k_PFPlanBankAreaCode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAreaCode)
        r_aoPremiumFinanceArray(k_PFPlanBankPhone, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankPhoneNo)
        r_aoPremiumFinanceArray(k_PFPlanBankExtn, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankExtension)
        r_aoPremiumFinanceArray(k_PFPlanBankFaxCode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankFaxAreaCode)
        r_aoPremiumFinanceArray(k_PFPlanBankFax, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankFaxNo)

        If Cast.ToInt32(r_oProcessPFPlanRequest.PFQuote.PartyBankKey, 0) > 0 Then
            r_aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = r_oProcessPFPlanRequest.PFQuote.PartyBankKey
        End If

        r_aoPremiumFinanceArray(k_PFPlanBankAddress1, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAddress1)
        r_aoPremiumFinanceArray(k_PFPlanBankAddress2, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAddress2)
        r_aoPremiumFinanceArray(k_PFPlanBankAddress3, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAddress3)

        If r_oProcessPFPlanRequest.PFQuote.BankAddress4 Is Nothing AndAlso Trim(r_oProcessPFPlanRequest.PFQuote.BankAddress4) = "" Then
            r_aoPremiumFinanceArray(k_PFPlanBankAddress4, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankRegion)
        Else
            r_aoPremiumFinanceArray(k_PFPlanBankAddress4, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankAddress4)
        End If

        r_aoPremiumFinanceArray(k_PFPlanBankPostcode, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankPostCode)
        r_aoPremiumFinanceArray(k_PFPlanBankCountry, 0) = Trim(r_oProcessPFPlanRequest.PFQuote.BankCountry)
        r_aoPremiumFinanceArray(k_PFPlanBankCountry_ID, 0) = Cast.ToInt32(r_oProcessPFPlanRequest.PFQuote.BankCountryKey, 0)





    End Sub
#End Region

#Region "PopulatePFCreditCardDetails"
    Private Sub PopulatePremiumFinanceCreditCardDetailsForInstalment(ByRef oRequest As BaseProcessPFPlanRequestType, ByRef premiumFinanceDetailsArray As Object(,))
        If oRequest.PFCreditCardDetails IsNot Nothing Then
            premiumFinanceDetailsArray(k_PFPlanBankAccountName, 0) = oRequest.PFCreditCardDetails.NameOnCreditCard
            premiumFinanceDetailsArray(k_PFPlanCCNumber, 0) = oRequest.PFCreditCardDetails.Number
            premiumFinanceDetailsArray(k_PFPlanCCExpiryDate, 0) = oRequest.PFCreditCardDetails.ExpiryDate
            premiumFinanceDetailsArray(k_PFPlanCCStartDate, 0) = oRequest.PFCreditCardDetails.StartDate
            premiumFinanceDetailsArray(k_PFPlanCCIssue, 0) = oRequest.PFCreditCardDetails.Issue
            premiumFinanceDetailsArray(k_PFPlanCCPin, 0) = oRequest.PFCreditCardDetails.Pin
            premiumFinanceDetailsArray(k_PFPlanCardType, 0) = oRequest.PFCreditCardDetails.TypeCode
            premiumFinanceDetailsArray(k_PFPlanAuthCode, 0) = oRequest.PFCreditCardDetails.AuthCode
            If oRequest.PFCreditCardDetails.PartyBankKey > 0 Then
                premiumFinanceDetailsArray(k_PFPlanPartyBankIdSel, 0) = oRequest.PFCreditCardDetails.PartyBankKey
            End If
            premiumFinanceDetailsArray(k_PFPlanIsCardholder, 0) = oRequest.PFCreditCardDetails.CardHolder Is Nothing
            If oRequest.PFCreditCardDetails.CardHolder IsNot Nothing Then
                premiumFinanceDetailsArray(k_PfPlanCardholderName, 0) = oRequest.PFCreditCardDetails.CardHolder.Name
                premiumFinanceDetailsArray(k_PfPlanCardholderAddress1, 0) = oRequest.PFCreditCardDetails.CardHolder.AddressLine1
                premiumFinanceDetailsArray(k_PfPlanCardholderAddress2, 0) = oRequest.PFCreditCardDetails.CardHolder.AddressLine2
                premiumFinanceDetailsArray(k_PfPlanCardholderAddress3, 0) = oRequest.PFCreditCardDetails.CardHolder.AddressLine3
                premiumFinanceDetailsArray(k_PfPlanCardholderAddress4, 0) = oRequest.PFCreditCardDetails.CardHolder.AddressLine4
                premiumFinanceDetailsArray(k_PfPlanCardholderPostcode, 0) = oRequest.PFCreditCardDetails.CardHolder.PostCode
            End If
        End If
    End Sub
#End Region

#Region "SavePlan"
    ''' <summary>
    ''' Used for updating the details of an existing plan
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oProcessPFPlanRequest"></param>
    ''' <returns>BaseProcessPFPlanResponseType</returns>
    ''' <remarks></remarks>
    Private Function SavePlan(ByVal con As SiriusConnection,
                                    ByVal oProcessPFPlanRequest As BaseProcessPFPlanRequestType,
                                    ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business) As BaseProcessPFPlanResponseType

        Dim nComReturnValue As Integer
        Dim oResponse As New BaseProcessPFPlanResponseType
        Dim oSamErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)


        Dim nTypeOfPackage As enumTypeOfPackage
        If oProcessPFPlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ProcessPFPlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.ProcessPFPlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If


        Try
            con.BeginTransaction()

            Dim nPartyKey As Integer
            nPartyKey = GetAndValidateSpecifiedTableCode(con, PMLookupTable.Party,
                                                         "party_cnt", "shortname",
                                                         oProcessPFPlanRequest.PFQuote.ClientCode, oSamErrorCollection, "PartyCode")

            If (UCase(Trim(oProcessPFPlanRequest.PFQuote.BankCountry)) = "GBR" _
             OrElse UCase(Trim(oProcessPFPlanRequest.PFQuote.BankCountry)) = "RSA") AndAlso
             Trim(oProcessPFPlanRequest.PFQuote.BankSortCode).Length() <> 6 Then

                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidSortCode,
                                                    "Sort code invalid.")
            End If
            Dim oMediaTypeValidationbusiness As New bSIRMediaTypeValidation.Business
            SAMFunc.InitialiseSBOObject(con, oMediaTypeValidationbusiness, _SiriusUser,
                    sObjectName:="bSirMediaTypeValidation.Business")


            Dim nMediaTypeId As Integer
            If oProcessPFPlanRequest.PFQuote IsNot Nothing AndAlso
            String.IsNullOrEmpty(oProcessPFPlanRequest.PFQuote.MediaTypeCode) = False Then
                nMediaTypeId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                              PMLookupTable.MediaType,
                                                                                              oProcessPFPlanRequest.PFQuote.MediaTypeCode,
                                                                                              "MediaTypeCode", oSamErrorCollection)
            End If



            Dim bValid As Boolean
            Dim sValidationMessage As Object
            Dim bValidationOverridable As Boolean

            If oProcessPFPlanRequest.PFQuote IsNot Nothing Then
                If UCase(Trim(oProcessPFPlanRequest.PFQuote.MediaTypeValidationCode)) = "BANK" Then
                    With oProcessPFPlanRequest
                        oMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=nMediaTypeId,
                                                                     v_lCountryID:= .PFQuote.BankCountryKey,
                                                                     v_sNumber:= .PFQuote.BankAccountNo,
                                                                     r_bValid:=bValid,
                                                                     r_sBankName:= .PFQuote.BankAccountName,
                                                                     r_sAddress1:= .PFQuote.BankAddress1,
                                                                     r_sAddress2:= .PFQuote.BankAddress2,
                                                                     r_sAddress3:= .PFQuote.BankAddress3,
                                                                     r_sAddress4:= .PFQuote.BankAddress4,
                                                                     r_sPostalCode:= .PFQuote.BankPostCode,
                                                                     r_vValidationMessage:=sValidationMessage,
                                                                     r_bValidationOverridable:=bValidationOverridable,
                                                                     sMediaCode:= .PFQuote.MediaTypeCode)
                    End With
                    If bValid = False Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid,
                                                                "Failed to validate Account No.")
                    End If
                End If
                'We will only receive Auth_Code, no need for this validation
                If UCase(Trim(oProcessPFPlanRequest.PFQuote.MediaTypeValidationCode)) = "CC" Then
                    'With oProcessPFPlanRequest
                    '    oMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=nMediaTypeId, _
                    '                          v_lCountryID:=.PFQuote.BankCountryKey, _
                    '                          v_sNumber:=.PFCreditCardDetails.Number, _
                    '                          r_bValid:=bValid)
                    'End With
                    'If bValid = False Then
                    '    oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid, _
                    '                                            "Failed to validate Credit Card No.")
                    'End If
                End If
            End If

            Dim sTransType As String = String.Empty
            If oProcessPFPlanRequest.TransType = ProcessPFPlanType.NewPlan Then
                sTransType = "G_NB"
            ElseIf oProcessPFPlanRequest.TransType = ProcessPFPlanType.PlanMTA Then
                sTransType = "MTA"
            Else
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanInvalidTransactionType,
                                                    "Invalid Transaction Type.")
            End If

            Dim nInsuranceFileKey As Integer = 0
            Dim oFinancePlanTransArray As Object(,)

            If oProcessPFPlanRequest.PFTransaction IsNot Nothing AndAlso IsArray(oProcessPFPlanRequest.PFTransaction) Then

                ReDim Preserve oFinancePlanTransArray(5, oProcessPFPlanRequest.PFTransaction.Length - 1)

                For lCount As Integer = 0 To oProcessPFPlanRequest.PFTransaction.Length - 1
                    If nInsuranceFileKey = 0 Then
                        nInsuranceFileKey = CInt(oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey)
                    ElseIf nInsuranceFileKey <> CInt(oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey) Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.TransactionSelectedMustBeOfSameTypeAndPolicy,
                                                            "Transactions selected must be of same Type and from the same Policy File.")
                    End If

                    oFinancePlanTransArray(0, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).TransdetailKey
                    oFinancePlanTransArray(1, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).PolicyRef
                    oFinancePlanTransArray(2, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).OutstandingAmount
                    oFinancePlanTransArray(3, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).Spare
                    oFinancePlanTransArray(4, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).DocumentTypeId
                    oFinancePlanTransArray(5, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey

                Next
            End If

            Const NoDeposit As Boolean = False
            Dim nCreatePartyBankRecord As Integer = 1
            Dim nInstalmentType As Integer = oProcessPFPlanRequest.Type


            r_oPremiumFinanceBusiness.InsuranceFileCnt = nInsuranceFileKey
            r_oPremiumFinanceBusiness.TransType = sTransType
            r_oPremiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)


            oProcessPFPlanRequest.SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                          PMLookupTable.Source,
                                                                                          oProcessPFPlanRequest.BranchCode,
                                                                                          "BranchCode", oSamErrorCollection)


            Dim aoPremiumFinanceArray As Object(,) = Nothing
            nComReturnValue = r_oPremiumFinanceBusiness.GetSingleFinancePlan(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                            oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                            aoPremiumFinanceArray)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
            End If



            Dim aoOldPFArray As Object(,) = aoPremiumFinanceArray
            Dim bPlanExists As Boolean

            'Verify if AutoGeneratedPlanRef has been set in request, if yes verify it with the one in the db, if different, 
            'then verify the AutoGeneratedPlanRef
            If oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef IsNot Nothing AndAlso
            aoOldPFArray IsNot Nothing AndAlso
            oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef.Trim().ToUpper() <> CType(aoOldPFArray(k_PFPlanAutoGenPlanRef, 0), String).Trim().ToUpper() Then

                nComReturnValue = r_oPremiumFinanceBusiness.CheckForDuplicatePlanReference(oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef,
                                                                                                            bPlanExists)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.CheckForDuplicatePlanReference",
                                            nComReturnValue)
                End If

                If bPlanExists Then
                    oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.PlanReferenceAlreadyExist,
                                                        "Plan Reference already exists.")
                End If
            End If

            If aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0) IsNot Nothing AndAlso
                CStr(aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0)) <> "" AndAlso
                CInt(aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0)) > 0 Then

                Dim oArray As Object = Nothing
                Dim oTaxArray(,) As Object = Nothing
                Dim sMissingTaxCode As String = String.Empty
                Dim nTaxAccountID As Integer = 0
                Dim sTaxCode As String = String.Empty

                nComReturnValue = r_oPremiumFinanceBusiness.GetTaxBandsByTaxGroup(CStr(aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0)), oArray)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.GetTaxBandsByTaxGroup",
                                            nComReturnValue)
                End If

                oTaxArray = CType(oArray, Object(,))
                If IsArray(oTaxArray) Then

                    Dim nLowerBound As Integer = oTaxArray.GetLowerBound(1)
                    Dim nUpperBound As Integer = oTaxArray.GetUpperBound(1)

                    For nCount As Integer = nLowerBound To nUpperBound
                        nTaxAccountID = 0
                        sTaxCode = String.Empty
                        If oTaxArray(0, nCount) IsNot Nothing AndAlso CStr(oTaxArray(0, nCount)) <> "" Then
                            nTaxAccountID = CInt(oTaxArray(0, nCount))
                        End If

                        sTaxCode = Cast.ToString(oTaxArray(1, nCount), "")
                        If nTaxAccountID = 0 Then
                            sMissingTaxCode = sMissingTaxCode + sTaxCode + Environment.NewLine
                        End If
                    Next

                    If sMissingTaxCode <> String.Empty Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid,
                                                                    "Following Tax Accounts are Missing for This Tax Posting." +
                                                                    Environment.NewLine + sMissingTaxCode +
                                                                    " Please create using Account Explorer.")
                    End If
                End If
            End If


            oSamErrorCollection.CheckForErrors()

            Dim sBankDetailsAction As String = String.Empty
            Dim aoPremiumFinanceArrayObject As Object(,) = Nothing

            'Update the existing plan array from the request
            ValidateAndPopulatePFPlan(oProcessPFPlanRequest,
                                      aoPremiumFinanceArray,
                                      sBankDetailsAction)

            aoPremiumFinanceArray(k_PFPlanClientId, 0) = nPartyKey
            'oPremiumFinanceArrayObject has the latest details from the request
            aoPremiumFinanceArrayObject = CType(aoPremiumFinanceArray, Object(,))


            aoPremiumFinanceArrayObject(k_PFPlanDateModified, 0) = Date.Today
            aoPremiumFinanceArrayObject(k_PfPlanDateBankDetailsChanged, 0) = Date.Today

            If oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing Then
                r_oPremiumFinanceBusiness.DepositCCTrackingNumber = Trim(oProcessPFPlanRequest.PFCreditCardDetails.TrackingNumber)
                r_oPremiumFinanceBusiness.AccountType = Trim(oProcessPFPlanRequest.PFCreditCardDetails.AccountType)
                PopulatePremiumFinanceCreditCardDetailsForInstalment(oProcessPFPlanRequest, aoPremiumFinanceArray)
            End If



            If (oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing AndAlso
                oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey > 0) _
                Or (oProcessPFPlanRequest.PFQuote.PartyBankKey > 0) Then

                nCreatePartyBankRecord = 0
                If oProcessPFPlanRequest.PFQuote.PartyBankKey > 0 Then
                    aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = oProcessPFPlanRequest.PFQuote.PartyBankKey
                Else
                    aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey
                End If
            End If


            Dim bNewTrans As Boolean = False
            If oProcessPFPlanRequest.PFQuote.ProductClass = "MTA" AndAlso IsArray(oFinancePlanTransArray) Then
                'bNewTrans = True
            End If

            If bNewTrans Then
                'New transactions to be financed
                If oProcessPFPlanRequest.PFQuote.StatusInd = FinancePlanStatus.Item010 Or
                oProcessPFPlanRequest.PFQuote.StatusInd = FinancePlanStatus.Item011 Then

                    r_oPremiumFinanceBusiness.UpdateExistingRecord(aoPremiumFinanceArrayObject,
                                                                 oProcessPFPlanRequest.PFPremFinanceKey,
                                                                 oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                   0,
                                                                   Nothing,
                                                                  iCreatePartyBankRecord:=nCreatePartyBankRecord)
                    If nComReturnValue <> PMEReturnCode.PMTrue Then
                        RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", nComReturnValue)
                    End If

                ElseIf oProcessPFPlanRequest.PFQuote.StatusInd = FinancePlanStatus.Item040 Then

                    nComReturnValue = r_oPremiumFinanceBusiness.InsertNewVersion(aoPremiumFinanceArrayObject,
                                                                                oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                oFinancePlanTransArray)
                    If nComReturnValue <> PMEReturnCode.PMTrue Then
                        RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", nComReturnValue)
                    End If
                End If
            Else
                nComReturnValue = r_oPremiumFinanceBusiness.UpdateExistingRecord(aoPremiumFinanceArrayObject,
                                                             oProcessPFPlanRequest.PFPremFinanceKey,
                                                             oProcessPFPlanRequest.PFPremFinanceVersion,
                                                             0,
                                                                   Nothing,
                                                                  iCreatePartyBankRecord:=nCreatePartyBankRecord)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", nComReturnValue)
                End If
            End If

            If sBankDetailsAction <> String.Empty Then

                nComReturnValue = r_oPremiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                            oProcessPFPlanRequest.PFPremFinanceVersion, sBankDetailsAction)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", nComReturnValue)
                End If

            End If

            nComReturnValue = r_oPremiumFinanceBusiness.DeletePFTransID(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                        oProcessPFPlanRequest.PFPremFinanceVersion)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.DeletePFTransID", nComReturnValue)
            End If

            nComReturnValue = r_oPremiumFinanceBusiness.InsertPFTransID(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                        oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                        oFinancePlanTransArray, 5)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertPFTransID", nComReturnValue)
            End If

            oResponse.PFPremFinanceKey = oProcessPFPlanRequest.PFPremFinanceKey
            oResponse.PFPremFinanceVersion = oProcessPFPlanRequest.PFPremFinanceVersion

            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
            Throw ex
        End Try

        Return oResponse
    End Function
#End Region

#Region "PlanMTA"

    ''' <summary>
    ''' This methods gets called when existing plan or its details need to be amended
    ''' Same as BO Maintain Instalment Plan task
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oProcessPFPlanRequest"></param>
    ''' <returns>BaseProcessPFPlanResponseType</returns>
    ''' <remarks></remarks>
    Private Function PlanMTA(ByVal con As SiriusConnection,
                                       ByVal oProcessPFPlanRequest As BaseProcessPFPlanRequestType,
                                       ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business) As BaseProcessPFPlanResponseType

        Dim oCredits As Object
        Dim oDebits As Object
        Dim nComReturnValue As Integer
        Dim oResponse As New BaseProcessPFPlanResponseType
        Dim oSamErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)



        Dim nTypeOfPackage As enumTypeOfPackage
        If oProcessPFPlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ProcessPFPlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.ProcessPFPlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If


        Try
            con.BeginTransaction()

            Dim nPartyKey As Integer
            nPartyKey = GetAndValidateSpecifiedTableCode(con, PMLookupTable.Party,
                                                         "party_cnt", "shortname",
                                                         oProcessPFPlanRequest.PFQuote.ClientCode, oSamErrorCollection, "PartyCode")

            If (UCase(Trim(oProcessPFPlanRequest.PFQuote.BankCountry)) = "GBR" _
             OrElse UCase(Trim(oProcessPFPlanRequest.PFQuote.BankCountry)) = "RSA") AndAlso
             Trim(oProcessPFPlanRequest.PFQuote.BankSortCode).Length() <> 6 Then

                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidSortCode,
                                                    "Sort code invalid.")
            End If
            Dim oMediaTypeValidationbusiness As New bSIRMediaTypeValidation.Business
            SAMFunc.InitialiseSBOObject(con, oMediaTypeValidationbusiness, _SiriusUser,
                    sObjectName:="bSirMediaTypeValidation.Business")


            Dim nMediaTypeId As Integer
            If oProcessPFPlanRequest.PFQuote IsNot Nothing AndAlso
            String.IsNullOrEmpty(oProcessPFPlanRequest.PFQuote.MediaTypeCode) = False Then
                nMediaTypeId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                              PMLookupTable.MediaType,
                                                                                              oProcessPFPlanRequest.PFQuote.MediaTypeCode,
                                                                                              "MediaTypeCode", oSamErrorCollection)

            End If



            Dim bValid As Boolean
            Dim sValidationMessage As Object
            Dim bValidationOverridable As Boolean

            If oProcessPFPlanRequest.PFQuote IsNot Nothing Then
                If UCase(Trim(oProcessPFPlanRequest.PFQuote.MediaTypeValidationCode)) = "BANK" Then


                    With oProcessPFPlanRequest
                        oMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=nMediaTypeId,
                                                                     v_lCountryID:= .PFQuote.BankCountryKey,
                                                                     v_sNumber:= .PFQuote.BankAccountNo,
                                                                     r_bValid:=bValid,
                                                                     r_sBankName:= .PFQuote.BankAccountName,
                                                                     r_sAddress1:= .PFQuote.BankAddress1,
                                                                     r_sAddress2:= .PFQuote.BankAddress2,
                                                                     r_sAddress3:= .PFQuote.BankAddress3,
                                                                     r_sAddress4:= .PFQuote.BankAddress4,
                                                                     r_sPostalCode:= .PFQuote.BankPostCode,
                                                                     r_vValidationMessage:=sValidationMessage,
                                                                     r_bValidationOverridable:=bValidationOverridable,
                                                                     sMediaCode:= .PFQuote.MediaTypeCode)
                    End With
                    If bValid = False Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid,
                                                                "Failed to validate Account No.")
                    End If
                End If
                'We will only receive Auth_Code, no need for this validation
                If UCase(Trim(oProcessPFPlanRequest.PFQuote.MediaTypeValidationCode)) = "CC" Then
                    'With oProcessPFPlanRequest
                    '    oMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=nMediaTypeId, _
                    '                          v_lCountryID:=.PFQuote.BankCountryKey, _
                    '                          v_sNumber:=.PFCreditCardDetails.Number, _
                    '                          r_bValid:=bValid)
                    'End With
                    'If bValid = False Then
                    '    oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid, _
                    '                                            "Failed to validate Credit Card No.")
                    'End If
                End If

            End If

            Dim sTransType As String = String.Empty
            If oProcessPFPlanRequest.TransType = ProcessPFPlanType.PlanMTA Then
                sTransType = "MTA"
            Else
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanInvalidTransactionType,
                                                    "Invalid Transaction Type.")
            End If

            Dim nInsuranceFileKey As Integer = 0
            Dim oFinancePlanTransArray As Object(,)

            If oProcessPFPlanRequest.PFTransaction IsNot Nothing AndAlso IsArray(oProcessPFPlanRequest.PFTransaction) Then

                ReDim Preserve oFinancePlanTransArray(5, oProcessPFPlanRequest.PFTransaction.Length - 1)

                For lCount As Integer = 0 To oProcessPFPlanRequest.PFTransaction.Length - 1
                    If nInsuranceFileKey = 0 Then
                        nInsuranceFileKey = CInt(oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey)
                    ElseIf nInsuranceFileKey <> CInt(oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey) Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.TransactionSelectedMustBeOfSameTypeAndPolicy,
                                                            "Transactions selected must be of same Type and from the same Policy File.")
                    End If

                    oFinancePlanTransArray(0, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).TransdetailKey
                    oFinancePlanTransArray(1, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).PolicyRef
                    oFinancePlanTransArray(2, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).OutstandingAmount
                    oFinancePlanTransArray(3, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).Spare
                    oFinancePlanTransArray(4, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).DocumentTypeId
                    oFinancePlanTransArray(5, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey

                Next
            End If

            Const NoDeposit As Boolean = False
            Dim nCreatePartyBankRecord As Integer = 1
            Dim nInstalmentType As Integer = oProcessPFPlanRequest.Type


            r_oPremiumFinanceBusiness.InsuranceFileCnt = nInsuranceFileKey
            r_oPremiumFinanceBusiness.TransType = sTransType
            r_oPremiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)


            oProcessPFPlanRequest.SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                          PMLookupTable.Source,
                                                                                          oProcessPFPlanRequest.BranchCode,
                                                                                          "BranchCode", oSamErrorCollection)

            oSamErrorCollection.CheckForErrors()


            Dim aoInstalmentQuotes As Object(,) = Nothing
            Dim nMatchingRatePositionInQuoteArray As Integer
            GetMatchingQuote(r_oPremiumFinanceBusiness, oProcessPFPlanRequest,
                             aoInstalmentQuotes, nMatchingRatePositionInQuoteArray,
                             oFinancePlanTransArray, nPartyKey, con)


            Dim aoPremiumFinanceArray As Object(,) = Nothing
            nComReturnValue = r_oPremiumFinanceBusiness.GetSingleFinancePlan(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                            oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                            aoPremiumFinanceArray)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
            End If


            Dim aoOldPFArray As Object(,) = aoPremiumFinanceArray ' Save the existing finance plan array
            Dim bPlanExists As Boolean


            'Verify if AutoGeneratedPlanRef has been set in request, if yes verify it with the one in the db, if different, 
            'then verify the AutoGeneratedPlanRef
            If oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef IsNot Nothing AndAlso
            aoOldPFArray IsNot Nothing AndAlso
            oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef.Trim().ToUpper() <> CType(aoOldPFArray(k_PFPlanAutoGenPlanRef, 0), String).Trim().ToUpper() Then

                nComReturnValue = r_oPremiumFinanceBusiness.CheckForDuplicatePlanReference(oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef,
                                                                                                            bPlanExists)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.CheckForDuplicatePlanReference",
                                            nComReturnValue)
                End If

                If bPlanExists Then
                    oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.PlanReferenceAlreadyExist,
                                                        "Plan Reference already exists.")
                End If
            End If
            oSamErrorCollection.CheckForErrors()

            Dim dSettlementAmount As Decimal
            GetPremiumFinanceOutstanding(con,
                                         aoPremiumFinanceArray,
                                         dSettlementAmount,
                                         oProcessPFPlanRequest.BranchCode,
                                         r_oPremiumFinanceBusiness)
            If dSettlementAmount <= 0 Then
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanNoOutStandingBalance,
                                                    "This instalment plan has no outstanding balance.")
                oSamErrorCollection.CheckForErrors()
            End If


            'It updates the oProcessPFPlanRequest.PFPremFinanceVersion
            nComReturnValue = r_oPremiumFinanceBusiness.InsertOrUpdatePremiumFinance(aoInstalmentQuotes,
                                                                                    nMatchingRatePositionInQuoteArray,
                                                                                    oFinancePlanTransArray,
                                                                                    oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                    oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                    CShort(oProcessPFPlanRequest.Type))
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertOrUpdatePremiumFinance", nComReturnValue)
            End If



            Dim aoPremiumFinanceArrayObject As Object(,) = Nothing
            'oPremiumFinanceArrayObject has the new version details as oProcessPFPlanRequest.PFPremFinanceVersion has been updated
            nComReturnValue = r_oPremiumFinanceBusiness.GetSingleFinancePlan(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                            oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                            aoPremiumFinanceArrayObject)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
            End If

            If aoPremiumFinanceArrayObject(k_PfPlanTaxGroupID, 0) IsNot Nothing AndAlso
                CStr(aoPremiumFinanceArrayObject(k_PfPlanTaxGroupID, 0)) <> "" AndAlso
                CInt(aoPremiumFinanceArrayObject(k_PfPlanTaxGroupID, 0)) > 0 Then

                Dim oArray As Object = Nothing
                Dim oTaxArray(,) As Object = Nothing
                Dim sMissingTaxCode As String = String.Empty
                Dim nTaxAccountID As Integer = 0
                Dim sTaxCode As String = String.Empty

                nComReturnValue = r_oPremiumFinanceBusiness.GetTaxBandsByTaxGroup(CStr(aoPremiumFinanceArrayObject(k_PfPlanTaxGroupID, 0)), oArray)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.GetTaxBandsByTaxGroup",
                                            nComReturnValue)
                End If

                oTaxArray = CType(oArray, Object(,))
                If IsArray(oTaxArray) Then

                    Dim nLowerBound As Integer = oTaxArray.GetLowerBound(1)
                    Dim nUpperBound As Integer = oTaxArray.GetUpperBound(1)

                    For nCount As Integer = nLowerBound To nUpperBound
                        nTaxAccountID = 0
                        sTaxCode = String.Empty
                        If oTaxArray(0, nCount) IsNot Nothing AndAlso CStr(oTaxArray(0, nCount)) <> "" Then
                            nTaxAccountID = CInt(oTaxArray(0, nCount))
                        End If

                        sTaxCode = Cast.ToString(oTaxArray(1, nCount), "")
                        If nTaxAccountID = 0 Then
                            sMissingTaxCode = sMissingTaxCode + sTaxCode + Environment.NewLine
                        End If
                    Next

                    If sMissingTaxCode <> String.Empty Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid,
                                                                    "Following Tax Accounts are Missing for This Tax Posting." +
                                                                    Environment.NewLine + sMissingTaxCode +
                                                                    " Please create using Account Explorer.")
                    End If
                End If
            End If

            oSamErrorCollection.CheckForErrors()

            'oPremiumFinanceArrayObject has the latest version details
            'oPremiumFinanceArray has the latest version details
            aoPremiumFinanceArray = CType(aoPremiumFinanceArrayObject, Object(,))

            oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef = oProcessPFPlanRequest.PFPremFinanceKey.ToString()

            aoPremiumFinanceArray(k_PFPlanDateModified, 0) = Date.Today
            aoPremiumFinanceArray(k_PfPlanDateBankDetailsChanged, 0) = Date.Today


            If oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing Then
                r_oPremiumFinanceBusiness.DepositCCTrackingNumber = Trim(oProcessPFPlanRequest.PFCreditCardDetails.TrackingNumber)
                r_oPremiumFinanceBusiness.AccountType = Trim(oProcessPFPlanRequest.PFCreditCardDetails.AccountType)
                PopulatePremiumFinanceCreditCardDetailsForInstalment(oProcessPFPlanRequest, aoPremiumFinanceArray)
            End If



            If (oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing AndAlso
                oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey > 0) _
                Or (oProcessPFPlanRequest.PFQuote.PartyBankKey > 0) Then

                nCreatePartyBankRecord = 0
                If oProcessPFPlanRequest.PFQuote.PartyBankKey > 0 Then
                    aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = oProcessPFPlanRequest.PFQuote.PartyBankKey
                Else
                    aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey
                End If
            End If

            Dim sBankDetailsAction As String = String.Empty
            'Updating the oPremiumFinanceArray from Request
            ValidateAndPopulatePFPlan(oProcessPFPlanRequest,
                                      aoPremiumFinanceArray,
                                      sBankDetailsAction)

            aoPremiumFinanceArray(k_PFPlanDateCreated, 0) = Date.Today
            aoPremiumFinanceArray(k_PFPlanClientId, 0) = nPartyKey
            'oPremiumFinanceArray from has the new version details updated from Request
            'oPremiumFinanceArrayObject has the new version detail updated from request
            aoPremiumFinanceArrayObject = CType(aoPremiumFinanceArray, Object(,))

            Dim nInsuranceFileTypeId As Integer = 0
            Dim nInsuranceFileTypeCode As String = String.Empty
            Dim nPolicyVersion As Integer = 0
            Dim sTransactionType As String = String.Empty

            GetInsuranceFileType(con, oProcessPFPlanRequest.PFQuote.InsuranceFileKey, nInsuranceFileTypeId, nInsuranceFileTypeCode, nPolicyVersion)

            Select Case nInsuranceFileTypeCode
                Case InsuranceFileType.LivePolicy, InsuranceFileType.Quote, InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated
                    If (InsuranceFileType.LivePolicy = Trim(nInsuranceFileTypeCode)) AndAlso nPolicyVersion > 1 Then
                        sTransactionType = "REN"
                    Else
                        sTransactionType = "NB"
                    End If
                Case InsuranceFileType.MTAPermanentQuotation, InsuranceFileType.MTATemporaryQuotation,
                    InsuranceFileType.MTAPermanent, InsuranceFileType.MTATemporary, InsuranceFileType.MTACancellation,
                    InsuranceFileType.MTAQuotationReinstatement, InsuranceFileType.MTAReinstated, InsuranceFileType.MTAQuotationCancellation

                    sTransactionType = "MTA"

                Case InsuranceFileType.Renewal
                    sTransactionType = "REN"

            End Select


            If (sTransactionType = "NB" OrElse sTransactionType = "REN" OrElse sTransactionType = "MTA") AndAlso oProcessPFPlanRequest.Type = InstalmentType.NoAmountChange Then
                nComReturnValue = r_oPremiumFinanceBusiness.PostMTA(CType(CObj(aoPremiumFinanceArrayObject), System.Array),
                                                                    Nothing,
                                                                    aoOldPFArray)
            Else
                nComReturnValue = r_oPremiumFinanceBusiness.PostMTA(CType(CObj(aoPremiumFinanceArrayObject), System.Array),
                                                    CType(CObj(oFinancePlanTransArray), Object(,)),
                                                    aoOldPFArray)
            End If

            Dim sPostMTAError As String = String.Empty
            If nComReturnValue = 9 Then
                sPostMTAError = "The transactions you have choosen do not meet the Minimum MTA amount - please reselect"
            ElseIf nComReturnValue = 99 Then
                sPostMTAError = "There are not enough instalments left over to spread"
            ElseIf nComReturnValue = k_PFPlanNoFinanceRate Then
                sPostMTAError = "Finance Scheme does not offer MTA Instalments."
            ElseIf nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.PostMTA", nComReturnValue)
            End If

            If sPostMTAError <> String.Empty Then
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanPostMTAError,
                                                    sPostMTAError)
                oSamErrorCollection.CheckForErrors()
            End If

            'While doing PlanMTA, the bank detials in request are different from the previous versions bank details
            ' we assume details have been modified.
            If CType(aoOldPFArray(k_PFPlanBankSortCode, 0), String) <> Trim(oProcessPFPlanRequest.PFQuote.BankSortCode) Or
            CType(aoOldPFArray(k_PFPlanBankAccountNo, 0), String) <> Trim(oProcessPFPlanRequest.PFQuote.BankAccountNo) Or
            CType(aoOldPFArray(k_PFPlanBankBranch, 0), String) <> Trim(oProcessPFPlanRequest.PFQuote.BankBranch) Or
            CType(aoOldPFArray(k_PFPlanBankAccountName, 0), String) <> Trim(oProcessPFPlanRequest.PFQuote.BankAccountName) Then

                sBankDetailsAction = "Amendment"
                aoPremiumFinanceArrayObject(k_PfPlanDateBankDetailsChanged, 0) = Cast.ToDateTime(Date.Today)
            End If

            'Create the Setup bank hstory by default
            nComReturnValue = r_oPremiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                            oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                            "Setup")

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", nComReturnValue)
            End If


            nComReturnValue = r_oPremiumFinanceBusiness.UpdateExistingRecord(CType(CObj(aoPremiumFinanceArrayObject), System.Array),
                                                                           oProcessPFPlanRequest.PFPremFinanceKey,
                                                                           oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                           nArrayIndex:=0,
                                                                           vPremiumFinanceMTA:=Nothing,
                                                                           iCreatePartyBankRecord:=CInt(nCreatePartyBankRecord))
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", nComReturnValue)
            End If

            If sBankDetailsAction = "Amendment" Then
                nComReturnValue = r_oPremiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                sBankDetailsAction)

                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", nComReturnValue)
                End If
            End If

            nComReturnValue = r_oPremiumFinanceBusiness.DeletePFTransID(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                        oProcessPFPlanRequest.PFPremFinanceVersion)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.DeletePFTransID", nComReturnValue)
            End If


            nComReturnValue = r_oPremiumFinanceBusiness.InsertPFTransID(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                        oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                        CType(CObj(oFinancePlanTransArray), Object(,)))
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertPFTransID", nComReturnValue)
            End If


            oResponse.PFPremFinanceKey = oProcessPFPlanRequest.PFPremFinanceKey
            oResponse.PFPremFinanceVersion = oProcessPFPlanRequest.PFPremFinanceVersion

            If oProcessPFPlanRequest.TransType = ProcessPFPlanType.PlanMTA _
                    AndAlso oProcessPFPlanRequest.PFTransaction IsNot Nothing AndAlso IsArray(oProcessPFPlanRequest.PFTransaction) Then

                aoPremiumFinanceArrayObject = Nothing
                'oPremiumFinanceArrayObject has the new version details as oProcessPFPlanRequest.PFPremFinanceVersion has been updated
                nComReturnValue = r_oPremiumFinanceBusiness.GetSingleFinancePlan(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                aoPremiumFinanceArrayObject)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
                End If
                If IsArray(aoPremiumFinanceArrayObject) AndAlso aoPremiumFinanceArrayObject.Length > 0 AndAlso gPMFunctions.ToSafeCurrency(aoPremiumFinanceArrayObject(k_PFPlanTotalCost, 0), 0) = 0 _
                    AndAlso Convert.ToString(aoPremiumFinanceArrayObject(k_PFPlanStatusInd, 0)) = InstalmentPlanStatus.Live Then

                    nComReturnValue = r_oPremiumFinanceBusiness.StatusUpdate(CType(oProcessPFPlanRequest.PFPremFinanceKey, Integer),
                                                                       CType(oProcessPFPlanRequest.PFPremFinanceVersion, Integer),
                                                                       InstalmentPlanStatus.Completed)
                    If nComReturnValue <> PMEReturnCode.PMTrue Then
                        RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate", nComReturnValue)
                    End If
                End If

            End If

            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
            Throw ex
        Finally
            oCoreBusiness = Nothing
        End Try

        Return oResponse

    End Function
#End Region

#Region "NewPlan"
    ''' <summary>
    ''' This methods gets called when existing transaction like [SND]need to be place on an instalment plan
    ''' Same as BO New Instalment Plan task
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="oProcessPFPlanRequest"></param>
    ''' <returns>BaseProcessPFPlanResponseType</returns>
    ''' <remarks></remarks>
    Private Function NewPlan(ByVal con As SiriusConnection,
                                    ByVal oProcessPFPlanRequest As BaseProcessPFPlanRequestType,
                                    ByRef r_oPremiumFinanceBusiness As bSIRPremiumFinance.Business) As BaseProcessPFPlanResponseType

        Dim oCredits As Object
        Dim oDebits As Object
        Dim nComReturnValue As Integer
        Dim oResponse As New BaseProcessPFPlanResponseType
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)


        Dim nTypeOfPackage As enumTypeOfPackage
        If oProcessPFPlanRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ProcessPFPlanRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.ProcessPFPlanResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If


        Try
            con.BeginTransaction()

            Dim aoInstalmentQuotes As Object(,) = Nothing
            Dim nMatchingRatePositionInQuoteArray As Integer
            Dim nPartyKey As Integer
            Dim oSamErrorCollection As New SAMErrorCollection

            nPartyKey = GetAndValidateSpecifiedTableCode(con, PMLookupTable.Party,
                                                         "party_cnt", "shortname",
                                                         oProcessPFPlanRequest.PFQuote.ClientCode, oSamErrorCollection, "PartyCode")

            Dim oMediaTypeValidationbusiness As New bSIRMediaTypeValidation.Business
            Dim nMediaTypeId As Integer
            Dim bValid As Boolean
            Dim sValidationMessage As Object
            Dim bValidationOverridable As Boolean
            SAMFunc.InitialiseSBOObject(con, oMediaTypeValidationbusiness, _SiriusUser,
                                sObjectName:="bSirMediaTypeValidation.Business")

            If oProcessPFPlanRequest.PFQuote IsNot Nothing AndAlso
            String.IsNullOrEmpty(oProcessPFPlanRequest.PFQuote.MediaTypeCode) = False Then

                nMediaTypeId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                              PMLookupTable.MediaType,
                                                                                              oProcessPFPlanRequest.PFQuote.MediaTypeCode,
                                                                                              "MediaTypeCode", oSamErrorCollection)

            End If

            If oProcessPFPlanRequest.PFQuote IsNot Nothing Then

                If UCase(Trim(oProcessPFPlanRequest.PFQuote.MediaTypeValidationCode)) = "BANK" Then
                    With oProcessPFPlanRequest
                        oMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=nMediaTypeId,
                                                                     v_lCountryID:= .PFQuote.BankCountryKey,
                                                                     v_sNumber:= .PFQuote.BankAccountNo,
                                                                     r_bValid:=bValid,
                                                                     r_sBankName:= .PFQuote.BankAccountName,
                                                                     r_sAddress1:= .PFQuote.BankAddress1,
                                                                     r_sAddress2:= .PFQuote.BankAddress2,
                                                                     r_sAddress3:= .PFQuote.BankAddress3,
                                                                     r_sAddress4:= .PFQuote.BankAddress4,
                                                                     r_sPostalCode:= .PFQuote.BankPostCode,
                                                                     r_vValidationMessage:=sValidationMessage,
                                                                     r_bValidationOverridable:=bValidationOverridable,
                                                                     sMediaCode:= .PFQuote.MediaTypeCode)
                    End With
                    If bValid = False Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid,
                                                                "Failed to validate Account No.")
                    End If
                End If
                'We will only receive Auth_Code, no need for this validation
                If UCase(Trim(oProcessPFPlanRequest.PFQuote.MediaTypeValidationCode)) = "CC" Then
                    'With oProcessPFPlanRequest
                    '    oMediaTypeValidationbusiness.ValidateNumber(v_lMediaID:=nMediaTypeId, _
                    '                          v_lCountryID:=.PFQuote.BankCountryKey, _
                    '                          v_sNumber:=.PFCreditCardDetails.Number, _
                    '                          r_bValid:=bValid)
                    'End With
                    'If bValid = False Then
                    '    oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid, _
                    '                                            "Failed to validate Credit Card No.")
                    'End If
                End If

            End If


            Dim sTransType As String = String.Empty
            If oProcessPFPlanRequest.TransType = ProcessPFPlanType.NewPlan Then
                sTransType = "G_NB"
            Else
                oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.InstalmentPlanInvalidTransactionType,
                                    "Invalid Transaction Type.")
            End If


            Dim nInsuranceFileKey As Integer = 0
            Dim oFinancePlanTransArray As Object(,)

            If oProcessPFPlanRequest.PFTransaction IsNot Nothing AndAlso IsArray(oProcessPFPlanRequest.PFTransaction) Then

                ReDim Preserve oFinancePlanTransArray(5, oProcessPFPlanRequest.PFTransaction.Length - 1)

                For lCount As Integer = 0 To oProcessPFPlanRequest.PFTransaction.Length - 1
                    If nInsuranceFileKey = 0 Then
                        nInsuranceFileKey = CInt(oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey)
                    ElseIf nInsuranceFileKey <> CInt(oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey) Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.TransactionSelectedMustBeOfSameTypeAndPolicy,
                                                            "Transactions selected must be of same Type and from the same Policy File.")
                    End If

                    oFinancePlanTransArray(0, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).TransdetailKey
                    oFinancePlanTransArray(1, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).PolicyRef
                    oFinancePlanTransArray(2, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).OutstandingAmount
                    oFinancePlanTransArray(3, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).Spare
                    oFinancePlanTransArray(4, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).DocumentTypeId
                    oFinancePlanTransArray(5, lCount) = oProcessPFPlanRequest.PFTransaction(lCount).InsuranceFileKey

                Next
            End If


            Const NoDeposit As Boolean = False
            Dim nCreatePartyBankRecord As Integer = 1
            Dim nInstalmentType As Integer = 0

            nInstalmentType = oProcessPFPlanRequest.Type
            r_oPremiumFinanceBusiness.InsuranceFileCnt = nInsuranceFileKey
            r_oPremiumFinanceBusiness.TransType = sTransType
            r_oPremiumFinanceBusiness.SetProcessModes(vTask:=PMEComponentAction.PMEdit)


            oProcessPFPlanRequest.SourceId = oCoreBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                                          PMLookupTable.Source,
                                                                                          oProcessPFPlanRequest.BranchCode,
                                                                                          "BranchCode", oSamErrorCollection)

            oSamErrorCollection.CheckForErrors()




            GetMatchingQuote(r_oPremiumFinanceBusiness, oProcessPFPlanRequest,
                             aoInstalmentQuotes, nMatchingRatePositionInQuoteArray,
                             oFinancePlanTransArray, nPartyKey, con)


            Dim aoPremiumFinanceArray As Object(,) = Nothing

            'We will get a new PFPRemFinanceKey and PFPremFinanceVersion
            nComReturnValue = r_oPremiumFinanceBusiness.InsertOrUpdatePremiumFinance(aoInstalmentQuotes,
                                                                                    nMatchingRatePositionInQuoteArray,
                                                                                    oFinancePlanTransArray,
                                                                                    oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                    oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                    CShort(oProcessPFPlanRequest.Type))

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertOrUpdatePremiumFinance", nComReturnValue)
            End If

            nComReturnValue = r_oPremiumFinanceBusiness.GetSingleFinancePlan(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                            oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                            aoPremiumFinanceArray)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.GetSingleFinancePlan", nComReturnValue)
            End If

            If aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0) IsNot Nothing AndAlso
                CStr(aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0)) <> "" AndAlso
                CInt(aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0)) > 0 Then

                Dim oArray As Object = Nothing
                Dim oTaxArray(,) As Object = Nothing
                Dim sMissingTaxCode As String = String.Empty
                Dim nTaxAccountID As Integer = 0
                Dim sTaxCode As String = String.Empty

                nComReturnValue = r_oPremiumFinanceBusiness.GetTaxBandsByTaxGroup(CStr(aoPremiumFinanceArray(k_PfPlanTaxGroupID, 0)), oArray)
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.GetTaxBandsByTaxGroup",
                                            nComReturnValue)
                End If

                oTaxArray = CType(oArray, Object(,))
                If IsArray(oTaxArray) Then

                    Dim nLowerBound As Integer = oTaxArray.GetLowerBound(1)
                    Dim nUpperBound As Integer = oTaxArray.GetUpperBound(1)

                    For nCount As Integer = nLowerBound To nUpperBound
                        nTaxAccountID = 0
                        sTaxCode = String.Empty
                        If oTaxArray(0, nCount) IsNot Nothing AndAlso CStr(oTaxArray(0, nCount)) <> "" Then
                            nTaxAccountID = CInt(oTaxArray(0, nCount))
                        End If

                        sTaxCode = Cast.ToString(oTaxArray(1, nCount), "")
                        If nTaxAccountID = 0 Then
                            sMissingTaxCode = sMissingTaxCode + sTaxCode + Environment.NewLine
                        End If
                    Next

                    If sMissingTaxCode <> String.Empty Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountNumberNotValid,
                                                                    "Following Tax Accounts are Missing for This Tax Posting." +
                                                                    Environment.NewLine + sMissingTaxCode +
                                                                    " Please create using Account Explorer.")
                    End If
                End If
            End If
            oSamErrorCollection.CheckForErrors()


            aoPremiumFinanceArray(k_PFPlanDateModified, 0) = Date.Today
            aoPremiumFinanceArray(k_PfPlanDateBankDetailsChanged, 0) = Date.Today

            Dim premiumFinanceArray As Object(,) = CType(aoPremiumFinanceArray, Object(,))

            If oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing Then
                r_oPremiumFinanceBusiness.DepositCCTrackingNumber = Trim(oProcessPFPlanRequest.PFCreditCardDetails.TrackingNumber)
                r_oPremiumFinanceBusiness.AccountType = Trim(oProcessPFPlanRequest.PFCreditCardDetails.AccountType)
                PopulatePremiumFinanceCreditCardDetailsForInstalment(oProcessPFPlanRequest, premiumFinanceArray)
            End If


            If (oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing AndAlso
                oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey > 0) _
                Or (oProcessPFPlanRequest.PFQuote.PartyBankKey > 0) Then

                nCreatePartyBankRecord = 0
                If oProcessPFPlanRequest.PFQuote.PartyBankKey > 0 Then
                    aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = oProcessPFPlanRequest.PFQuote.PartyBankKey
                Else
                    aoPremiumFinanceArray(k_PFPlanPartyBankIdSel, 0) = oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey
                End If
            End If

            oProcessPFPlanRequest.PFQuote.AutoGeneratedPlanRef = oProcessPFPlanRequest.PFPremFinanceKey.ToString()

            If oProcessPFPlanRequest.PFCreditCardDetails IsNot Nothing AndAlso oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey = 0 Then
                aoPremiumFinanceArray = premiumFinanceArray
                Dim premiumFinanceCntObject As Object = oProcessPFPlanRequest.PFPremFinanceKey
                Dim premiumFinanceVersionObject As Object = oProcessPFPlanRequest.PFPremFinanceVersion

                'Modified the value of iCreatePartyBankRecord parameter. 
                'If request objects party bank key value is non zero, the party bank item is already created. 
                'No need to create it again.
                nComReturnValue = r_oPremiumFinanceBusiness.UpdateExistingRecord(
                        vExistingRecord:=CType(CObj(aoPremiumFinanceArray), System.Array),
                        vPremiumFinanceCnt:=premiumFinanceCntObject,
                        vPremiumFinanceVersion:=premiumFinanceVersionObject,
                        nArrayIndex:=0,
                        vPremiumFinanceMTA:=Nothing,
                        iCreatePartyBankRecord:=CInt(nCreatePartyBankRecord))
                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.UpdateExistingRecord", nComReturnValue)
                End If
            End If

            Dim sBankDetailsAction As String = String.Empty
            'Updating the oPremiumFinanceArray from Request
            ValidateAndPopulatePFPlan(oProcessPFPlanRequest,
                                      aoPremiumFinanceArray,
                                      sBankDetailsAction)

            aoPremiumFinanceArray(k_PFPlanDateCreated, 0) = Date.Today
            aoPremiumFinanceArray(k_PFPlanClientId, 0) = nPartyKey
            'oPremiumFinanceArray from has the new version details updated from Request
            'oPremiumFinanceArrayObject has the new version detail updated from request
            Dim aoPremiumFinanceArrayObject As Object(,) = Nothing
            aoPremiumFinanceArrayObject = CType(aoPremiumFinanceArray, Object(,))

            Dim nPFPlanTransDetailId As Integer = 0
            Dim oDepositTransDetailsId As Object = Nothing
            'No need to send trans array, as it will pick it up, in the same format as used in BO.
            nComReturnValue = r_oPremiumFinanceBusiness.ProcessPlan(InstalmentsMode.NewBusinessOrRenewals,
                                                                    aoPremiumFinanceArrayObject,
                                                                    oFinancePlanTransArray,
                                                                    nPFPlanTransDetailId,
                                                                    False,
                                                                    oDepositTransDetailsId,
                                                                    CShort(oProcessPFPlanRequest.Type))

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.ProcessPlan", nComReturnValue)
            End If

            sBankDetailsAction = "Setup"
            nComReturnValue = r_oPremiumFinanceBusiness.SaveInstalmentsPlanMediaTypeDetails(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                                            oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                                            sBankDetailsAction)

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.SaveInstalmentsPlanMediaTypeDetails", nComReturnValue)
            End If
            nComReturnValue = r_oPremiumFinanceBusiness.StatusUpdate(CType(oProcessPFPlanRequest.PFPremFinanceKey, Integer),
                                                                    CType(oProcessPFPlanRequest.PFPremFinanceVersion, Integer),
                                                                    InstalmentPlanStatus.Live)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.StatusUpdate", nComReturnValue)
            End If

            nComReturnValue = r_oPremiumFinanceBusiness.DeletePFTransID(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                        oProcessPFPlanRequest.PFPremFinanceVersion)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.DeletePFTransID", nComReturnValue)
            End If
            'Add 5 index because  insurance file Cnt is on 5th index in current Collection.
            nComReturnValue = r_oPremiumFinanceBusiness.InsertPFTransID(oProcessPFPlanRequest.PFPremFinanceKey,
                                                                        oProcessPFPlanRequest.PFPremFinanceVersion,
                                                                        oFinancePlanTransArray, 5)
            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.InsertPFTransID", nComReturnValue)
            End If

            oResponse.PFPremFinanceKey = oProcessPFPlanRequest.PFPremFinanceKey
            oResponse.PFPremFinanceVersion = oProcessPFPlanRequest.PFPremFinanceVersion

            'Create event
            nComReturnValue = r_oPremiumFinanceBusiness.CreateEvent((ToSafeInteger(PMEComponentAction.PMAdd)).ToString,
                                                                       ToSafeInteger(aoPremiumFinanceArray(k_PFPlanInsuranceFileCnt, 0), 0),
                                                                       CType(aoPremiumFinanceArray(k_PFPlanClientId, 0), Integer),
                                                                       CType(aoPremiumFinanceArray(k_PFPlanAutoGenPlanRef, 0), String))

            If nComReturnValue <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPremiumFinance.Business.CreateEvent", nComReturnValue)
            End If


            con.CommitTransaction()
        Catch ex As Exception
            con.RollbackTransaction()
            Throw ex
        End Try

        Return oResponse
    End Function
#End Region

#Region "GetFinancePlanInformation"
    Public Overloads Function GetFinancePlanInformation(ByVal oGetFinancePlanInformationRequest As BaseGetFinancePlanInformationRequestType) As BaseGetFinancePlanInformationResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseGetFinancePlanInformationResponseType
            oResponse = GetFinancePlanInformation(con, oGetFinancePlanInformationRequest)
            Return oResponse
        End Using
    End Function


    Private Overloads Function GetFinancePlanInformation(ByVal con As SiriusConnection, ByVal oRequest As BaseGetFinancePlanInformationRequestType) As BaseGetFinancePlanInformationResponseType


        Dim oResponse As BaseGetFinancePlanInformationResponseType
        Dim nTypeOfPackage As enumTypeOfPackage
        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.GetFinancePlanInformationRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.GetFinancePlanInformationResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
            Return Nothing
        End If


        Dim nReturn As Integer = 0
        Dim lFolderCnt As Integer
        Dim lPolicyVersion As Integer
        Dim oRenewalCount As Object = Nothing
        Dim oInsuranceFile() As Object = Nothing
        Dim aoInsuranceFile() As Object = Nothing

        Dim obSIRInsuranceFile As New bSIRInsuranceFile.Business
        Dim obSIRInsuranceFolder As New bSIRInsuranceFolder.Business
        Dim obSIRPremiumFinance As New bSIRPremiumFinance.Business

        Dim lInsuranceFileKey As Integer
        Dim lOrigInsuranceFileKey As Integer
        Dim sProductCode As String = String.Empty
        Dim lRenewalInsuranceFileCnt As Integer
        Dim lPremiumFinanceCnt As Integer
        Dim lPremiumFinanceVersion As Integer


        lInsuranceFileKey = oRequest.InsuranceFileKey

        Try

            If lInsuranceFileKey > 0 Then

                SAMFunc.InitialiseSBOObject(con, obSIRInsuranceFile, _SiriusUser,
                                sObjectName:="bSIRInsuranceFile.Business")

                nReturn = obSIRInsuranceFile.GetDetails(vInsuranceFileCnt:=CType(lInsuranceFileKey, Object))
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("Failed to get details for " & lInsuranceFileKey, nReturn)
                End If

                nReturn = obSIRInsuranceFile.GetNext(oInsuranceFile)
                If nReturn <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("Failed to GetNext for " & lInsuranceFileKey, nReturn)
                End If

                If IsArray(oInsuranceFile) Then
                    aoInsuranceFile = DirectCast(oInsuranceFile, Object())
                End If

                lFolderCnt = gPMFunctions.NullToLong(aoInsuranceFile(InsuranceFileConst.ACInsuranceFolderCnt))


                obSIRInsuranceFile = Nothing

                'TR - Make sure there is a valid InsuranceFolder
                If lFolderCnt > 0 Then

                    SAMFunc.InitialiseSBOObject(con, obSIRInsuranceFolder, _SiriusUser,
                               sObjectName:="bSIRInsuranceFolder.Business")

                    With obSIRInsuranceFolder

                        nReturn = .GetDetails(vInsuranceFolderCnt:=lFolderCnt)
                        nReturn = .GetNext(vRenewalCount:=oRenewalCount)
                    End With
                Else
                    RaiseComMethodException("Failed to get InsuranceFolder ID", lFolderCnt)
                End If

                lPolicyVersion = gPMFunctions.NullToLong(aoInsuranceFile(InsuranceFileConst.ACPolicyVersion))

                If CType(oRenewalCount, Integer) > 0 AndAlso lPolicyVersion > 1 Then
                    sProductCode = "REN"
                Else
                    'TR - If this is the first Policy Version, then this is New Business
                    If lPolicyVersion <= 1 Then
                        sProductCode = "NB"
                    Else
                        Dim lPFPremFinanceCnt As Integer
                        Dim lPFPremFinanceVer As Integer
                        SAMFunc.InitialiseSBOObject(con, obSIRPremiumFinance, _SiriusUser,
                               sObjectName:="bSIRPremiumFinance.Business")

                        nReturn = obSIRPremiumFinance.GetFinancePlanFromInsFolderAndVersion(lPolicyVersion, lFolderCnt, lPFPremFinanceCnt, lPFPremFinanceVer, lOrigInsuranceFileKey)

                        If nReturn <> PMEReturnCode.PMTrue Then
                            RaiseComMethodException("Failed to Get Finance Plan.", nReturn)
                        End If

                        'TR - If there is a PLan then this is an MTA, otherwise it is NB
                        If lPFPremFinanceCnt > 0 And lPFPremFinanceVer > 0 Then
                            sProductCode = "MTA"
                            'TR - Swap over the Insurance file counts for Quoting Instalments
                            'TR - The New Insurance File
                            lRenewalInsuranceFileCnt = lInsuranceFileKey
                            'TR - The Insurance file with an existing Plan
                            lInsuranceFileKey = lOrigInsuranceFileKey
                            If lPremiumFinanceCnt = 0 Then
                                lPremiumFinanceCnt = lPFPremFinanceCnt
                            End If
                            If lPremiumFinanceVersion = 0 Then
                                lPremiumFinanceVersion = lPFPremFinanceVer
                            End If
                        Else
                            lRenewalInsuranceFileCnt = 0
                            sProductCode = "NB"
                        End If
                    End If
                End If
            End If

            oResponse.PremiumFinanceKey = lPremiumFinanceCnt
            oResponse.PremiumFinanceVersion = lPremiumFinanceVersion
            oResponse.ProductCode = sProductCode
            oResponse.OriginalInsuranceFileKey = lOrigInsuranceFileKey
            'RETURN RENEWALINSURANCEKEY IS NOT REQUIRED AS OF NOW

            Return oResponse

        Catch ex As System.Exception
            Throw ex
        Finally
            oRenewalCount = Nothing
            obSIRInsuranceFile = Nothing
            obSIRInsuranceFolder = Nothing
            obSIRPremiumFinance = Nothing
        End Try
    End Function

#End Region

#Region "GenerateWriteOffAndAutoAllocate"
    ''' <summary>
    ''' Generate WriteOff Entry and Auto Allocate it with Outstanding Trasactions
    ''' Called while doing MTC via Underwritting or Instalment Plan Maintenance    
    ''' </summary>
    ''' <param name="con">SiriusConnection Type</param>
    ''' <param name="sBranchCode"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="bReturnIfWriteOffRequired"></param>    
    ''' <param name="bThrowBusinessRuleErrors"></param>
    ''' <remarks></remarks>
    Private Sub GenerateWriteOffAndAutoAllocate(ByVal con As SiriusConnection,
                                                ByVal sBranchCode As String,
                                                ByVal nInsuranceFileCnt As Integer,
                                                ByVal bReturnIfWriteOffRequired As Boolean,
                                                ByVal bThrowBusinessRuleErrors As Boolean)

        Dim oSamErrorCollection As New SAMErrorCollection
        Dim oCoreBusiness As New CoreBusiness(_SiriusUser)
        Dim oPremiumFinanceBusiness As bSIRPremiumFinance.Business = Nothing

        Try

            Const kAutoAllocateIfAbleOptionNumber As Integer = 5059
            Dim sOptionValue As String = String.Empty
            Dim nComReturnValue As Integer

            oPremiumFinanceBusiness = CreateAndInitialisePremiumFinanceBusiness(con, sBranchCode)

            oCoreBusiness.GetSystemOption(sBranchCode, kAutoAllocateIfAbleOptionNumber, sOptionValue)

            Dim bAllocationWriteOffAuthority As Boolean = False
            Dim crAllocationWriteOffAuthorityAmount As Decimal
            Dim dsUserAuthority As DataSet
            Dim oRequest As New BaseGetUserAuthorityValueRequestType

            'Only attempt if system option is on
            If sOptionValue IsNot Nothing _
            AndAlso sOptionValue <> String.Empty _
            AndAlso sOptionValue = "1" Then

                oRequest.UserAuthorityOption = UserAuthorityOptions.HasWriteOffAuthority
                dsUserAuthority = GetValueForUserAuthority(con, _SiriusUser.UserID, oRequest)

                Dim oTransactions As Object(,)
                Dim crOutStandingBalance As Decimal
                GetOutStandingTransactionsForInsuranceFolder(con,
                                                             nInsuranceFileCnt,
                                                             oTransactions,
                                                             crOutStandingBalance)
                'bReturnIfWriteOffRequired will be true when Write Off check box on Cancel Policy screen
                'via instalment plan maintenance is unchecked
                If bReturnIfWriteOffRequired = True AndAlso crOutStandingBalance <> CDec(0) Then
                    'we are coming through Plan MTA
                    'Then dont attempt write off or auto allocation as check box unticked, even if system option is on
                    'However, if Write Off checkbox unticked, and writeoff not rquired. 
                    'Then we will attempt auto allocate.
                    Return
                End If

                If Not dsUserAuthority Is Nothing AndAlso
                dsUserAuthority.Tables.Count > 0 AndAlso
                dsUserAuthority.Tables(0).Rows.Count > 0 Then
                    If dsUserAuthority.Tables(0).Rows(0).Item(0).ToString() = "1" Then
                        bAllocationWriteOffAuthority = True
                        crAllocationWriteOffAuthorityAmount = gPMFunctions.ToSafeDecimal(dsUserAuthority.Tables(0).Rows(0).Item(2).ToString())
                    Else
                        If bThrowBusinessRuleErrors Then
                            oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.UserAuthorityWriteOffAuthorityDisabled,
                                            "Write off attempt was unsuccessful. You do not have write off authority.")
                            oSamErrorCollection.CheckForErrors()
                        Else
                            Return
                        End If

                    End If
                Else
                    If bThrowBusinessRuleErrors Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.UserAuthorityWriteOffAuthorityDisabled,
                                        "Write off attempt was unsuccessful. You do not have write off authority.")
                        oSamErrorCollection.CheckForErrors()
                    Else
                        Return
                    End If
                End If

                If Math.Abs(crOutStandingBalance) > crAllocationWriteOffAuthorityAmount Then
                    If bThrowBusinessRuleErrors Then
                        oSamErrorCollection.AddBusinessRule(SAMBusinessErrors.UserAuthorityWriteOffAuthorityAmountExceeded,
                                        "Write off attempt was unsuccessful. The outstanding balance amount was not within your write off limit.")
                        oSamErrorCollection.CheckForErrors()
                    Else
                        Return
                    End If
                End If

                Dim nAccountId As Integer = 0
                If oTransactions IsNot Nothing Then
                    nAccountId = gPMFunctions.ToSafeInteger(oTransactions(0, 0))
                End If

                nComReturnValue = oPremiumFinanceBusiness.GenerateWriteOffAndAutoAllocate(nAccountId,
                                                                                          crOutStandingBalance,
                                                                                          oTransactions)

                If nComReturnValue <> PMEReturnCode.PMTrue Then
                    RaiseComMethodException("bSIRPremiumFinance.Business.CancelPolicies", nComReturnValue)
                    oSamErrorCollection.CheckForErrors()
                End If
            End If
        Catch ex As Exception
            Throw ex
        Finally
            oCoreBusiness = Nothing
        End Try

    End Sub

    ''' <summary>
    '''This method is used to getClaim payment
    '''<param name="con" type ="SiriusConnection">An object of SiriusConnection class</param>
    '''<param name="nInsuranceFileCnt" type ="integer"></param>
    '''<param name="o_dsTransactions" type ="DataSet"></param>
    '''<param name="o_crOutstandingBalance" type ="Decimal"></param>
    '''<returns></returns>
    ''' </summary>
    Public Sub GetOutStandingTransactionsForInsuranceFolder(ByVal con As SiriusConnection,
                                         ByVal nInsuranceFileCnt As Integer,
                                         ByRef o_oTransactions As Object(,),
                                         ByRef o_crOutstandingBalance As Decimal)
        Dim dsTransactions As DataSet
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Get_Outstanding_Transactions_For_Insurance_Folder")
            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = nInsuranceFileCnt
            cmd.AddOutParameter("@outstandingbalance", SqlDbType.Money)
            dsTransactions = con.ExecuteDataSet(cmd, "Claims")
            o_crOutstandingBalance = gPMFunctions.ToSafeDecimal(cmd.Parameters("@outstandingbalance").Value, CDec(0))
        End Using

        If dsTransactions IsNot Nothing _
        AndAlso dsTransactions.Tables.Count > 0 _
        AndAlso dsTransactions.Tables(0).Rows.Count > 0 Then

            ReDim o_oTransactions(3, dsTransactions.Tables(0).Rows.Count - 1)

            For nRowCount As Integer = 0 To dsTransactions.Tables(0).Rows.Count - 1
                For nColumnCount As Integer = 0 To dsTransactions.Tables(0).Columns.Count - 1
                    o_oTransactions(nColumnCount, nRowCount) = CObj(dsTransactions.Tables(0).Rows(nRowCount).Item(nColumnCount).ToString())
                Next
                'o_oTransactions(0, nRowCount) = CObj(dsTransactions.Tables(0).Rows(nRowCount).Item(0).ToString())
                'o_oTransactions(1, nRowCount) = CObj(dsTransactions.Tables(0).Rows(nRowCount).Item(1).ToString())
                'o_oTransactions(2, nRowCount) = CObj(dsTransactions.Tables(0).Rows(nRowCount).Item(2).ToString())
                'o_oTransactions(3, nRowCount) = CObj(dsTransactions.Tables(0).Rows(nRowCount).Item(3).ToString())
            Next

        End If
    End Sub

#End Region

#Region "Reverse Manual Collected Instalment"

    Public Overloads Function ReverseCollectedInstalment(ByVal oBaseReverseCollectedInstalmentRequestType As BaseReverseCollectedInstalmentRequestType) As BaseReverseCollectedInstalmentResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As BaseReverseCollectedInstalmentResponseType
            oResponse = ReverseCollectedInstalment(con, oBaseReverseCollectedInstalmentRequestType)
            Return oResponse
        End Using
    End Function
    Public Overloads Function ReverseCollectedInstalment(ByVal con As SiriusConnection, ByVal oBaseReverseCollectedInstalmentRequestType As BaseReverseCollectedInstalmentRequestType) As BaseReverseCollectedInstalmentResponseType

        Dim obSIRPFInstalments As New bSIRPFInstalments.Business

        Try
            Dim nReturn As Integer = 0
            Dim oSamErrorCollection As New SAMErrorCollection
            Dim oResponse As BaseReverseCollectedInstalmentResponseType
            Dim nTypeOfPackage As enumTypeOfPackage

            If oBaseReverseCollectedInstalmentRequestType.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ReverseCollectedInstalmentRequestType) Then
                nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
                oResponse = New SAMForInsuranceV2ImplementationTypes.ReverseCollectedInstalmentResponseType
            Else
                nTypeOfPackage = enumTypeOfPackage.UnknownPackage
                Return Nothing
            End If



            SAMFunc.InitialiseSBOObject(con, obSIRPFInstalments, _SiriusUser,
                            sObjectName:="bSIRPFInstalments.Business")
            If obSIRPFInstalments Is Nothing Then
                RaiseComMethodException("Failed to Get bSIRPFInstalments.Business.", nReturn)
            End If

            nReturn = obSIRPFInstalments.ReverseCollectedInstalment(oBaseReverseCollectedInstalmentRequestType.PFInstalmentId,
                        oBaseReverseCollectedInstalmentRequestType.PFPlanStatusInd)
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseComMethodException("bSIRPFInstalments.Business.ReverseCollectedInstalment", nReturn)
                oSamErrorCollection.CheckForErrors()
            End If

            Return oResponse
        Catch ex As Exception
            Throw
        Finally
            If Not obSIRPFInstalments Is Nothing Then
                obSIRPFInstalments.Dispose()
                obSIRPFInstalments = Nothing
            End If
        End Try
    End Function

#End Region
#End Region
#Region "UpdateInstalmentDetails"
    '''<summary>
    '''This function is used to Update InstalmentStatus
    '''</summary>
    '''<param name="oUpdateInstalmentStatusRequest">BaseUpdateInstalmentStatusRequestType</param>
    '''<remarks></remarks>
    Public Overloads Function UpdateInstalmentDetails(ByVal oUpdateInstalmentDetailsRequest As BaseUpdateInstalmentDetailsRequestType) As BaseUpdateInstalmentDetailsResponseType
        Dim oUpdateInstalmentDetailsResponseType As New BaseImplementationTypes.BaseUpdateInstalmentDetailsResponseType
        Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection


        ' validate the data provided in the structure
        'oUpdateInstalmentStatusRequest.Validate(CType(oSAMErrorCollection, Object))
        'oSAMErrorCollection.CheckForErrors()

        ' Checking the Type of package we are receiving in form of request
        Dim nTypeOfPackage As CoreSAMBusiness.enumTypeOfPackage
        If oUpdateInstalmentDetailsRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.UpdateInstalmentDetailsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oUpdateInstalmentDetailsResponseType = New SAMForInsuranceV2ImplementationTypes.UpdateInstalmentDetailsResponseType
        End If

        Using conPFI As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Try


                Try
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_PFInstalment_Details_Update")
                        cmd.AddInParameter("@pfprem_Financialcnt", SqlDbType.Int).Value = oUpdateInstalmentDetailsRequest.FinancialPlanKey
                        cmd.AddInParameter("@pfFinancialVersion", SqlDbType.Int).Value = oUpdateInstalmentDetailsRequest.FinancialPlanVersion
                        cmd.AddInParameter("@nInstalmentno", SqlDbType.Int).Value = oUpdateInstalmentDetailsRequest.InstalmentNo
                        cmd.AddInParameter("@DueDate", SqlDbType.DateTime).Value = oUpdateInstalmentDetailsRequest.DueDate
                        conPFI.ExecuteNonQuery(cmd)
                    End Using

                Catch ex As Exception
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CollectionDateError,
                              SAMBusinessErrors.CollectionDateError.ToString,
                             "Failed to call Procedure spu_ACT_PFInstalment_Status_Update")
                    oSAMErrorCollection.CheckForErrors()

                End Try
            Finally
                conPFI.Dispose()
            End Try
        End Using

        Return oUpdateInstalmentDetailsResponseType

    End Function
#End Region

End Class
