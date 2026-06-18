Imports System.Web.HttpContext
Imports Microsoft.Practices.EnterpriseLibrary.Logging

Partial Public Class ProviderSAMForInsuranceV2

    Public Overrides Function ApplyPolicyDiscount(ByVal v_iInsuranceFileKey As Integer,
                                                  ByVal v_iProductId As Integer,
                                                  ByVal v_sTransactionType As String,
                                                  ByVal v_iTask As Integer,
                                                  ByRef r_sFailureReason As String,
                                                  Optional ByVal v_crAppliedDiscountPremium As Decimal = 0,
                                                  Optional ByVal v_dAppliedDiscountPercentage As Double = 0,
                                                  Optional ByVal v_iAppliedMatchDiscountPremium As Integer = 0,
                                                  Optional ByVal v_iAppliedDiscountReasonId As Integer = 0,
                                                  Optional ByVal v_iAppliedDiscountRecurringTypeId As Integer = 0) As Boolean

        SyncLock oLock
            Dim oRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.ApplyDiscountCommand
            Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ApplyDiscountCommandResponse

            Try
                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .BranchCode = If(String.IsNullOrEmpty(sBranchCode), sDefaultBranchCode, sBranchCode)
                    .InsuranceFileKey = v_iInsuranceFileKey
                    .ProductId = v_iProductId
                    .TransactionType = v_sTransactionType
                    .Task = v_iTask
                    .AppliedDiscountPremium = v_crAppliedDiscountPremium
                    .AppliedDiscountPercentage = v_dAppliedDiscountPercentage
                    .AppliedMatchDiscountPremium = v_iAppliedMatchDiscountPremium
                    .AppliedDiscountReasonId = v_iAppliedDiscountReasonId
                    .AppliedDiscountRecurringTypeId = v_iAppliedDiscountRecurringTypeId
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.ApplyDiscount, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ApplyDiscountCommandResponse)(result)
                End Using

                If oResponse IsNot Nothing AndAlso oResponse.ApplyDiscountResponse IsNot Nothing Then
                    r_sFailureReason = oResponse.ApplyDiscountResponse.FailureReason
                End If

                Return True

            Catch ex As Exception
                Throw
            End Try
        End SyncLock
    End Function

    Public Overrides Function RollbackPolicyDiscount(ByVal v_iInsuranceFileKey As Integer,
                                                     ByVal v_iProductId As Integer,
                                                     ByVal v_sTransactionType As String,
                                                     ByVal v_iTask As Integer) As Boolean

        SyncLock oLock
            Dim oRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.RollbackDiscountCommand

            Try
                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .BranchCode = If(String.IsNullOrEmpty(sBranchCode), sDefaultBranchCode, sBranchCode)
                    .InsuranceFileKey = v_iInsuranceFileKey
                    .ProductId = v_iProductId
                    .TransactionType = v_sTransactionType
                    .Task = v_iTask
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.RollbackDiscount, oRequest)
                End Using

                Return True

            Catch ex As Exception
                Throw
            End Try
        End SyncLock
    End Function

    Public Overrides Function GetPolicyDiscountInfo(ByVal v_iInsuranceFileKey As Integer) As PolicyDiscount

        SyncLock oLock
            Dim oRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyDiscountInfoQuery
            Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyDiscountInfoQueryResponse
            Dim oDiscount As New PolicyDiscount

            Try
                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .BranchCode = If(String.IsNullOrEmpty(sBranchCode), sDefaultBranchCode, sBranchCode)
                    .InsuranceFileKey = v_iInsuranceFileKey
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetPolicyDiscountInfo, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyDiscountInfoQueryResponse)(result)
                End Using

                If oResponse IsNot Nothing Then
                    oDiscount.IsDiscountApplied = oResponse.IsDiscountApplied
                    oDiscount.DiscountPercentage = oResponse.DiscountPercentage
                    oDiscount.DiscountedPremium = oResponse.DiscountedPremium
                    oDiscount.DiscountReasonId = oResponse.DiscountReasonId
                    oDiscount.MatchDiscountedPremium = oResponse.MatchDiscountedPremium
                    oDiscount.RecurringTypeId = oResponse.RecurringTypeId
                    oDiscount.TotalPremium = oResponse.TotalPremium
                End If

                Return oDiscount
            
            Catch ex As Exception
                Throw
            End Try
        End SyncLock
    End Function

    Public Overrides Function GetPolicyDiscountTotalPremium(ByVal v_iInsuranceFileKey As Integer) As Decimal

        SyncLock oLock
            Dim oRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyDiscountTotalPremiumQuery
            Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyDiscountTotalPremiumQueryResponse

            Try
                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .BranchCode = If(String.IsNullOrEmpty(sBranchCode), sDefaultBranchCode, sBranchCode)
                    .InsuranceFileKey = v_iInsuranceFileKey
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetPolicyDiscountTotalPremium, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetPolicyDiscountTotalPremiumQueryResponse)(result)
                End Using

                Return If(oResponse IsNot Nothing, oResponse.TotalPremium, 0D)

            Catch ex As Exception
                Throw
            End Try
        End SyncLock
    End Function

    Public Overrides Function GetInvalidPolicyDiscountRisks(ByVal v_iInsuranceFileKey As Integer) As Object(,)

        SyncLock oLock
            Dim oRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInvalidDiscountRisksQuery
            Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInvalidDiscountRisksQueryResponse

            Try
                With oRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    .BranchCode = If(String.IsNullOrEmpty(sBranchCode), sDefaultBranchCode, sBranchCode)
                    .InsuranceFileKey = v_iInsuranceFileKey
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetInvalidDiscountRisks, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetInvalidDiscountRisksQueryResponse)(result)
                End Using

                If oResponse Is Nothing OrElse oResponse.InvalidRisks Is Nothing OrElse oResponse.InvalidRisks.Count = 0 Then
                    Return Nothing
                End If

                Dim vResults(4, oResponse.InvalidRisks.Count - 1) As Object
                For i As Integer = 0 To oResponse.InvalidRisks.Count - 1
                    Dim oRisk = oResponse.InvalidRisks(i)
                    vResults(0, i) = oRisk.RiskKey
                    vResults(1, i) = oRisk.TotalBilledPremium
                    vResults(2, i) = oRisk.TotalReturnPremium
                    vResults(3, i) = oRisk.RiskDescription
                    vResults(4, i) = oRisk.RiskNumber
                Next

                Return vResults

            Catch ex As Exception
                Throw
            End Try
        End SyncLock
    End Function

End Class
