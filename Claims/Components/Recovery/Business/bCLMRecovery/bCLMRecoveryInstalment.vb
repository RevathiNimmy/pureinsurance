Option Strict Off
Option Explicit On
Imports SharedFiles
Imports SharedFiles.iPMFunc
Imports SharedFiles.gPMConstants

' ADO #39459: Instalment for Claim Recovery - Scheme Configuration
' Core business logic for recovery instalment plan creation
Public Class bCLMRecoveryInstalment

    Private Const ACClass As String = "bCLMRecoveryInstalment"
    Private Const SCHEME_TYPE_CLAIM_RECOVERY As Integer = 2
    Private Const TRANS_TYPE_SALVAGE As Integer = 1
    Private Const TRANS_TYPE_THIRD_PARTY As Integer = 2

    ''' <summary>
    ''' Check if recovery instalments are enabled for the product.
    ''' Reads Product.recovery_instalments_enabled directly.
    ''' </summary>
    Public Function IsRecoveryInstalmentEnabled(ByVal lProductId As Integer) As Boolean
        Dim lEnabled As Integer = 0
        Dim lReturn As Integer

        Try
            lReturn = iPMFunc.ExecSPReturnParam(
                "spu_Product_GetRecoveryInstalmentFlag",
                "@ProductId", lProductId,
                "@RecoveryInstalmentsEnabled", lEnabled)

            Return (lReturn = gPMConstants.PMEReturnCode.PMTrue AndAlso lEnabled = 1)
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to check recovery instalment config",
                vApp:="Claims", vClass:=ACClass, vMethod:="IsRecoveryInstalmentEnabled",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Get the recovery type (Salvage=1, Third-Party=2) from a CLR transaction.
    ''' </summary>
    Public Function GetRecoveryType(ByVal lClrTransactionId As Integer) As Integer
        ' Read recovery_type from the CLR transaction record
        ' This field already exists on the CLR transaction table
        Dim lRecoveryType As Integer = 0

        Try
            Dim rs As Object = iPMFunc.ExecSPReturnRS(
                "spu_Check_Recovery_Type",
                "@ClrTransactionId", lClrTransactionId)

            If rs IsNot Nothing AndAlso Not rs.EOF Then
                lRecoveryType = CInt(rs("recovery_type"))
            End If
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to get recovery type for CLR transaction " & lClrTransactionId,
                vApp:="Claims", vClass:=ACClass, vMethod:="GetRecoveryType",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
        End Try

        Return lRecoveryType
    End Function

    ''' <summary>
    ''' Get available Claim Recovery schemes for a company filtered by transaction type.
    ''' </summary>
    Public Function GetAvailableRecoverySchemes(ByVal lCompanyNo As Integer, ByVal lTransactionType As Integer) As Object
        Dim rs As Object = Nothing

        Try
            rs = iPMFunc.ExecSPReturnRS(
                "spu_PF_Scheme_SelByType",
                "@CompanyNo", lCompanyNo,
                "@SchemeType", SCHEME_TYPE_CLAIM_RECOVERY)
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to get recovery schemes for company " & lCompanyNo,
                vApp:="Claims", vClass:=ACClass, vMethod:="GetAvailableRecoverySchemes",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
        End Try

        Return rs
    End Function

    ''' <summary>
    ''' Validate that no active instalment plan exists for the given CLR transaction.
    ''' Returns True if creation is allowed (no active plan), False if blocked.
    ''' </summary>
    Public Function ValidateNoActivePlan(ByVal lClrTransactionId As Integer) As Boolean
        Dim lHasActivePlan As Integer = 0

        Try
            Dim lReturn As Integer = iPMFunc.ExecSPReturnParam(
                "spu_CLR_RecoveryInstalmentPlan_Validate",
                "@ClrTransactionId", lClrTransactionId,
                "@HasActivePlan", lHasActivePlan)

            Return (lHasActivePlan = 0)
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to validate active plan for CLR transaction " & lClrTransactionId,
                vApp:="Claims", vClass:=ACClass, vMethod:="ValidateNoActivePlan",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Get rates for a specific scheme and transaction type.
    ''' </summary>
    Public Function GetRatesForTransactionType(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal lTransactionType As Integer) As Object
        Dim rs As Object = Nothing

        Try
            rs = iPMFunc.ExecSPReturnRS(
                "spu_PF_Rate_SelByTransType",
                "@CompanyNo", lCompanyNo,
                "@SchemeNo", lSchemeNo,
                "@SchemeVersion", lSchemeVersion,
                "@TransactionType", lTransactionType)
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to get rates for scheme " & lSchemeNo & " transaction type " & lTransactionType,
                vApp:="Claims", vClass:=ACClass, vMethod:="GetRatesForTransactionType",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
        End Try

        Return rs
    End Function

End Class
