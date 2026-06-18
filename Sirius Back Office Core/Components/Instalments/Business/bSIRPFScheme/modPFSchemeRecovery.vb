Option Strict Off
Option Explicit On
Imports SharedFiles
Imports SharedFiles.iPMFunc
Imports SharedFiles.gPMConstants

' ADO #39456, #39458: Instalment for Claim Recovery - Scheme Configuration
' Business logic for Scheme Type dropdown (T7) and Transaction Type configuration (T8)
' in Instalment Scheme Maintenance
Public Module modPFSchemeRecovery

    Private Const ACClass As String = "modPFSchemeRecovery"

    ' Scheme Type constants
    Public Const SCHEME_TYPE_PREMIUM_FINANCE As Integer = 1
    Public Const SCHEME_TYPE_CLAIM_RECOVERY As Integer = 2

    ' Transaction Type constants
    Public Const TRANS_TYPE_SALVAGE_RECOVERY As Integer = 1
    Public Const TRANS_TYPE_THIRD_PARTY_RECOVERY As Integer = 2

    ''' <summary>
    ''' T7: Load the scheme_type for a given scheme.
    ''' Returns 1 (Premium Finance) or 2 (Claim Recovery).
    ''' </summary>
    Public Function LoadSchemeType(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer) As Integer
        Dim lSchemeType As Integer = SCHEME_TYPE_PREMIUM_FINANCE

        Try
            Dim rs As Object = iPMFunc.ExecSPReturnRS(
                "spu_PFScheme_Sel",
                "@CompanyNo", lCompanyNo,
                "@SchemeNo", lSchemeNo,
                "@SchemeVersion", lSchemeVersion,
                "@SchemeType", 0) ' 0 = no filter, get whatever exists

            If rs IsNot Nothing AndAlso Not rs.EOF Then
                If Not IsDBNull(rs("scheme_type")) Then
                    lSchemeType = CInt(rs("scheme_type"))
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to load scheme type",
                vApp:="Instalments", vClass:=ACClass, vMethod:="LoadSchemeType",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
        End Try

        Return lSchemeType
    End Function

    ''' <summary>
    ''' T7: Save the scheme_type for a given scheme via the modified spu_PFScheme_upd.
    ''' The @SchemeType parameter is included in the existing update procedure.
    ''' </summary>
    Public Function SaveSchemeType(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal lSchemeType As Integer) As Boolean
        Try
            Dim sSQL As String = "UPDATE PFScheme SET scheme_type = " & lSchemeType &
                " WHERE companyno = " & lCompanyNo &
                " AND schemeno = " & lSchemeNo &
                " AND schemeversion = " & lSchemeVersion

            Dim lReturn As Integer = iPMFunc.ExecSQL(sSQL)
            Return (lReturn = gPMConstants.PMEReturnCode.PMTrue)
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to save scheme type",
                vApp:="Instalments", vClass:=ACClass, vMethod:="SaveSchemeType",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' T8: Load transaction types available for a Claim Recovery scheme's rates.
    ''' Returns a recordset of rates filtered by transaction type.
    ''' </summary>
    Public Function LoadTransactionTypes(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal lTransactionType As Integer) As Object
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
                sMsg:="Failed to load transaction types for scheme " & lSchemeNo,
                vApp:="Instalments", vClass:=ACClass, vMethod:="LoadTransactionTypes",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
        End Try

        Return rs
    End Function

    ''' <summary>
    ''' T8: Save rate with transaction type for a Claim Recovery scheme.
    ''' </summary>
    Public Function SaveRateByTransactionType(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal lTransactionType As Integer, ByVal decRate As Decimal, ByVal decAdminFee As Decimal) As Boolean
        Try
            Dim sSQL As String = "UPDATE PFRate SET transaction_type = " & lTransactionType &
                " WHERE companyno = " & lCompanyNo &
                " AND schemeno = " & lSchemeNo &
                " AND schemeversion = " & lSchemeVersion &
                " AND (transaction_type = " & lTransactionType & " OR transaction_type IS NULL)"

            Dim lReturn As Integer = iPMFunc.ExecSQL(sSQL)
            Return (lReturn = gPMConstants.PMEReturnCode.PMTrue)
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError,
                sMsg:="Failed to save rate by transaction type",
                vApp:="Instalments", vClass:=ACClass, vMethod:="SaveRateByTransactionType",
                vErrNo:=Err.Number, vErrDesc:=ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' T8: Get rate for a specific transaction type.
    ''' </summary>
    Public Function GetRateByTransactionType(ByVal lCompanyNo As Integer, ByVal lSchemeNo As Integer, ByVal lSchemeVersion As Integer, ByVal lTransactionType As Integer) As Object
        Return LoadTransactionTypes(lCompanyNo, lSchemeNo, lSchemeVersion, lTransactionType)
    End Function

    ''' <summary>
    ''' Helper: Get scheme type display name.
    ''' </summary>
    Public Function GetSchemeTypeDisplayName(ByVal lSchemeType As Integer) As String
        Select Case lSchemeType
            Case SCHEME_TYPE_PREMIUM_FINANCE
                Return "Premium Finance"
            Case SCHEME_TYPE_CLAIM_RECOVERY
                Return "Claim Recovery"
            Case Else
                Return "Unknown"
        End Select
    End Function

    ''' <summary>
    ''' Helper: Get transaction type display name.
    ''' </summary>
    Public Function GetTransactionTypeDisplayName(ByVal lTransactionType As Integer) As String
        Select Case lTransactionType
            Case TRANS_TYPE_SALVAGE_RECOVERY
                Return "Salvage Recovery"
            Case TRANS_TYPE_THIRD_PARTY_RECOVERY
                Return "Third-Party Recovery"
            Case Else
                Return "Unknown"
        End Select
    End Function

End Module
