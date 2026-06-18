Option Strict Off
Option Explicit On

Imports System.Data
Imports SSP.Shared
Partial Public Class Business


#Region "Public Types"
    Private Enum TaxArray
        TaxGroupId = 0
        TaxBandId = 1
        TaxCurrencyCode = 2
        Percentage = 3
        Value = 4
        IsValue = 5
        ClassOfBusinessId = 6
        Sequence = 7
        IsManuallyChanged = 8
        TaxGroupDescription = 9
        TaxBandDescription = 10
        TaxBandcode = 11
        TaxGroupCode = 12
    End Enum
#End Region

#Region "Private Types"
#End Region

#Region "Public Constants"
    Public Const kATSPayment_IsInsuredDomiciled As Integer = 1
    Public Const kATSPayment_InsuredPercentage As Integer = 2
    Public Const kATSPayment_InsuranceTaxNumber As Integer = 3
    Public Const kATSPayment_IsPayeeDomiciled As Integer = 4
    Public Const kATSPayment_PayeePercentage As Integer = 5
    Public Const kATSPayment_PayeeTaxNumber As Integer = 6
    Public Const kATSPayment_SafeHarbourCode As Integer = 7
    Public Const kATSPayment_SafeHarbourPercentage As Integer = 8
    Public Const kATSPayment_IsTaxExempt As Integer = 9
    Public Const kATSPayment_IsWHTExempt As Integer = 10
    Public Const kATSPayment_IsSettlement As Integer = 11
    Public Const kATSPayment_SafeHarbourID As Integer = 12
    Public Const kATSPayment_FieldCount As Integer = kATSPayment_SafeHarbourID

    Public Const kATSReceipt_PaymentToCode As Integer = 1
    Public Const kATSReceipt_IsInsuredDomiciled As Integer = 2
    Public Const kATSReceipt_InsuredPercentage As Integer = 3
    Public Const kATSReceipt_InsuredTaxNumber As Integer = 4
    Public Const kATSReceipt_IsTaxExempt As Integer = 5
    Public Const kATSReceipt_IsSettlement As Integer = 6
    Public Const kATSReceipt_ReceivablePercentage As Integer = 7
    Public Const kATSReceipt_FieldCount As Integer = kATSReceipt_ReceivablePercentage

#End Region

#Region "Private Constants"
    'PAYMENT PARAMETERS CONSTANTS
    Private Const kPaymentParameters_ProcessType As Integer = 0
    Private Const kPaymentParameters_Payee As Integer = 1
    Private Const kPaymentParameters_PaymentToCode As Integer = 2
    Private Const kPaymentParameters_SafeHarbourCode As Integer = 3
    Private Const kPaymentParameters_SafeHarbourPercentage As Integer = 4
    Private Const kPaymentParameters_InsuredDomiciled As Integer = 5
    Private Const kPaymentParameters_InsuredPercentage As Integer = 6
    Private Const kPaymentParameters_InsuredTaxNumber As Integer = 7
    Private Const kPaymentParameters_PayeeDomiciled As Integer = 8
    Private Const kPaymentParameters_PayeePercentage As Integer = 9
    Private Const kPaymentParameters_PayeeTaxNumber As Integer = 10
    Private Const kPaymentParameters_IsTaxExempt As Integer = 11
    Private Const kPaymentParameters_IsWHTExempt As Integer = 12
    Private Const kPaymentParameters_IsSettlement As Integer = 13
    Private Const kPaymentParameters_CurrencyCode As Integer = 14
    Private Const kPaymentParameters_Amount As Integer = 15
    Private Const kPaymentParameters_ExcessAmount As Integer = 16
    Private Const kPaymentParameters_PaymentAdjustment As Integer = 17
    Private Const kPaymentParameters_TaxArray As Integer = 18
    Private Const kPaymentParameters_ErrorMessage As Integer = 19
    Private Const kPaymentParameters_FieldCount As Integer = kPaymentParameters_ErrorMessage

    'RECIEPT PARAMETERS CONSTANTS
    Private Const kReceiptParameters_ProcessType As Integer = 0
    Private Const kReceiptParameters_Payee As Integer = 1
    Private Const kReceiptParameters_PaymentToCode As Integer = 2
    Private Const kReceiptParameters_InsuredDomiciled As Integer = 3
    Private Const kReceiptParameters_InsuredPercentage As Integer = 4
    Private Const kReceiptParameters_InsuredTaxNumber As Integer = 5
    Private Const kReceiptParameters_IsTaxExempt As Integer = 6
    Private Const kReceiptParameters_IsSettlement As Integer = 7
    Private Const kReceiptParameters_CurrencyCode As Integer = 8
    Private Const kReceiptParameters_Amount As Integer = 9
    Private Const kReceiptParameters_TaxArray As Integer = 10
    Private Const kReceiptParameters_ReceivablePercentage As Integer = 11
    Private Const kReceiptParameters_ErrorMessage As Integer = 12
    Private Const kReceiptParameters_FieldCount As Integer = kReceiptParameters_ErrorMessage

#End Region

#Region "Public Variables"
#End Region

#Region "Private Variables"
#End Region

#Region "Public Properties"
    Public ReadOnly Property GetNewATSObjectForPayment() As Object
        Get
            Dim oAdvancedTaxDetatailObject(kATSPayment_FieldCount) As Object
            oAdvancedTaxDetatailObject(kATSProcessMode) = kProcessMode_Payment
            Return oAdvancedTaxDetatailObject
        End Get
    End Property

    Public ReadOnly Property GetNewATSObjectForReciept() As Object
        Get
            Dim oAdvancedTaxDetatailObject(kATSReceipt_FieldCount) As Object
            oAdvancedTaxDetatailObject(kATSProcessMode) = kProcessMode_ReceiptFromRules
            Return oAdvancedTaxDetatailObject
        End Get
    End Property

    Public ReadOnly Property LossCurrencyName() As String
        Get
            If m_sLossCurrencyName = String.Empty Then
                GetClaimCurrency(nClaimID:=CurrentClaimKey, o_nLossCurrencyID:=m_nLossCurrencyID, o_sLossCurrencyName:=m_sLossCurrencyName)
            End If
            Return m_sLossCurrencyName
        End Get
    End Property

    Public ReadOnly Property LossCurrencyID() As String
        Get
            If m_nLossCurrencyID = 0 Then
                GetClaimCurrency(nClaimID:=CurrentClaimKey, o_nLossCurrencyID:=m_nLossCurrencyID, o_sLossCurrencyName:=m_sLossCurrencyName)
            End If
            Return m_nLossCurrencyID
        End Get
    End Property
#End Region

#Region "Private Properties"
#End Region

#Region "Public Methods"
    'THIS FUNCTION CAN UPDATE PAYMENTS AND UPDATE TAX VALUES BASED ON :
    '1) CALCULATE ATS BASED ON oAdvancedTaxArray AND USE THAT
    '2) OR USE CALCULATED TAX VALUE PASSED AS crScriptCalculatedTax FROM CALLING CODE 
    '3) OR CALCULATE TAX USING BASIC TAX GROUP CODE PASSED AS PART OF oClaimDetailsArray

    Public Function UpdateClaimPaymentDetails(ByVal oClaimDetailsArray As Object,
                                              Optional ByVal oAdvancedTaxArray As Object = Nothing,
                                              Optional ByVal aoUpdatedTaxArray As Object = Nothing,
                                              Optional ByVal dScriptCalculatedTax As Double = 0,
                                              Optional ByVal bIsSpecifiedScriptCalculatedTax As Boolean = False,
                                              Optional ByVal bPostPayment As Boolean = False) As Integer

        Const kMethodName As String = "UpdateClaimPaymentDetails"
        Const kColumnRank As Integer = 1
        Const kRowRank As Integer = 2

        Const kClaimIndex As Integer = 0
        Dim nTotalPerils As Integer = 0
        Dim nReserveCount As Integer
        Dim bPaymentIsGross As Boolean = IsGrossClaimPayment
        Dim oTaxArray As Object
        Try
            If m_sTransactionType = kTRANSACTIONTYPE_PayClaim Then
                If Informations.IsArray(oClaimDetailsArray) AndAlso Informations.IsArray(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)) Then

                    nTotalPerils = UBound(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, nTotalPerils), kRowRank)
                    For iPeril As Integer = 0 To nTotalPerils
                        Dim sPayeeShortCode As String = String.Empty
                        Dim nClaimPaymentID As Integer = 0
                        Dim crTotalPayment As Decimal = 0
                        Dim crTotalTax As Decimal = 0
                        Dim nClaimPerilID As Integer = ToSafeInteger(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_PerilID, iPeril))
                        Dim sPerilTypeCode As String = ToSafeString(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERILType_Code, iPeril))
                        If Informations.IsArray(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)) Then
                            nReserveCount = UBound(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril), kRowRank)

                            'If Fields of Reserve is not total number of field expected at the time of payment throw an exception.
                            If UBound(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril), kColumnRank) <> kRESERVE_CP_FieldCount Then
                                Throw New ApplicationException("Please make sure to pass valid array to " & kMethodName & " method.")
                            End If


                            For iReserve As Integer = 0 To nReserveCount

                                Dim nReserveID As Integer
                                Dim cThisPayment As Decimal
                                Dim nPartyBankID As Integer
                                Dim sMediaTypeCode As String
                                Dim nPayeeType As Integer
                                Dim sTaxGroupCode As String
                                Dim bIsExcess As Boolean
                                Dim sCurrencyCode As String
                                Dim crATSCalculatedTax As Decimal
                                Dim sMediaRef As String
                                Dim nClaimPaymentToID As Integer
                                Dim bOverrideTax As Boolean = False
                                Dim bIs_ExGratia As Boolean = False
                                Dim nUserId As Integer
                                Dim nCreatedBy As Integer

                                oTaxArray = Nothing

                                nReserveID = ToSafeInteger(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_ReserveID, iReserve))
                                cThisPayment = ToSafeDecimal(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_ThisPayment, iReserve))
                                sTaxGroupCode = ToSafeString(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_TaxGroupCode, iReserve))
                                nPayeeType = ToSafeInteger(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_PayeeType, iReserve))
                                sPayeeShortCode = ToSafeString(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_PayeeShortCode, iReserve))
                                sMediaTypeCode = ToSafeString(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_MediaType, iReserve))
                                nPartyBankID = ToSafeInteger(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_BankPaymentType, iReserve))
                                bIsExcess = ToSafeBoolean(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_IsExcess, iReserve))
                                sCurrencyCode = ToSafeString(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_CurrencyCode, iReserve))
                                sMediaRef = ToSafeString(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_MediaRef, iReserve))
                                nClaimPaymentToID = ToSafeInteger(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_ClaimPaymentToID, iReserve))
                                bIs_ExGratia = ToSafeBoolean(oClaimDetailsArray(kCLAIMSDETAILS_ArrayOfPerils, kClaimIndex)(kPERIL_ArrayofReserve, iPeril)(kRESERVE_CP_IsExGratia, iReserve))
                                nUserId = m_iUserID
                                nCreatedBy = ToSafeInteger(m_iUserID)

                                If cThisPayment <> 0 Then
                                    If Informations.IsArray(oAdvancedTaxArray) Then
                                        If GetPaymentTaxFromATS(nClaimPerilID:=nClaimPerilID,
                                                                 bIsExcess:=bIsExcess,
                                                                 sPartyName:=sPayeeShortCode,
                                                                 nPaymentPartyType:=nPayeeType,
                                                                 crPaymentAmount:=cThisPayment,
                                                                 sCurrencyCode:=sCurrencyCode,
                                                                 sTaxGroupCode:=sTaxGroupCode,
                                                                 oAdvancedTaxArray:=oAdvancedTaxArray,
                                                                 o_crScriptedTaxAmount:=crATSCalculatedTax,
                                                                 o_aoUpdatedTaxArray:=oTaxArray) <> PMEReturnCode.PMTrue Then
                                            Throw New ApplicationException("Failed to GetPaymentTaxFromATS")
                                        End If

                                        If SaveTaxBandInfo(oTaxArray, nReserveID) <> PMEReturnCode.PMTrue Then
                                            Throw New ApplicationException("Failed to SaveTaxBandInfo")
                                        End If
                                        bOverrideTax = True

                                    ElseIf Informations.IsArray(aoUpdatedTaxArray) Then
                                        SaveTaxBandInfo(aoUpdatedTaxArray, nReserveID)
                                        bOverrideTax = True
                                    ElseIf bIsSpecifiedScriptCalculatedTax Then
                                        'GET SYSTEM CALCULATED TAX ARRAY
                                        If GetSystemCalculatedTax(nClaimPerilID:=nClaimPerilID,
                                                                  crPaymentAmount:=cThisPayment,
                                                                  sTaxGroupCode:=sTaxGroupCode,
                                                                  o_aoUpdatedTaxArray:=oTaxArray,
                                                                  sCurrencyCode:=sCurrencyCode) <> PMEReturnCode.PMTrue OrElse oTaxArray Is Nothing Then
                                            Throw New ApplicationException("Failed to GetSystemCalculatedTax, Please make sure to configure tax with valid percentages or values.")
                                        End If
                                        'UPDATE SYSTEM TAX SET TAX ON ALL BANDS TO 0 AND UPDATE FIRST BAND TO HAVE TAX=dScriptCalculatedTax
                                        PutAllTaxOnFirstBand(o_aoUpdatedTaxArray:=oTaxArray, crTaxAmount:=dScriptCalculatedTax)
                                        If SaveTaxBandInfo(oTaxArray, nReserveID) <> PMEReturnCode.PMTrue Then
                                            Throw New ApplicationException("Failed to SaveTaxBandInfo")
                                        End If
                                        bOverrideTax = True
                                    End If

                                    m_oDatabase.Parameters.Clear()
                                    AddParameterLite(m_oDatabase, "bOverrideTax", bOverrideTax, PMEParameterDirection.PMParamInput, PMEDataType.PMBoolean, True)
                                    AddParameterLite(m_oDatabase, "reserveid", nReserveID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                                    AddParameterLite(m_oDatabase, "This_Payment", cThisPayment, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)
                                    AddParameterLite(m_oDatabase, "Payee_Type", nPayeeType, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                                    AddParameterLite(m_oDatabase, "Payee_Short_code", sPayeeShortCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                                    AddParameterLite(m_oDatabase, "Media_Type", sMediaTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                                    AddParameterLite(m_oDatabase, "Party_Bank_Id", nPartyBankID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                                    AddParameterLite(m_oDatabase, "Tax_Group_code", sTaxGroupCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                                    AddParameterLite(m_oDatabase, "bIsGrossClaimPaymentAmount", bPaymentIsGross, PMEParameterDirection.PMParamInput, PMEDataType.PMBoolean)
                                    AddParameterLite(m_oDatabase, "nIsExGratia", bIs_ExGratia, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                                    AddParameterLite(m_oDatabase, "nUserId", m_iUserID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

                                    If sMediaRef <> String.Empty Then
                                        AddParameterLite(m_oDatabase, "sMedia_Ref", sMediaRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                                    End If

                                    If nClaimPaymentToID > 0 Then
                                        AddParameterLite(m_oDatabase, "nClaimPaymentToID", nClaimPaymentToID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                                    End If

                                    AddParameterLite(m_oDatabase, "o_nClaimPaymentID", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
                                    AddParameterLite(m_oDatabase, "o_crTotalPayment", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)
                                    AddParameterLite(m_oDatabase, "o_crTotalTax", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)
                                    AddParameterLite(m_oDatabase, "CreatedBy", nCreatedBy, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)


                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateClaimPaymentDetailsSQL, sSQLName:=ACUpdateClaimPaymentDetailsName, bStoredProcedure:=ACUpdateClaimPaymentDetailsStored)

                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        Throw New ApplicationException(ACUpdateClaimPaymentDetailsSQL & "Failed")
                                    End If

                                    nClaimPaymentID = ToSafeInteger(m_oDatabase.Parameters.Item("o_nClaimPaymentID").Value)
                                    crTotalPayment = ToSafeDecimal(m_oDatabase.Parameters.Item("o_crTotalPayment").Value)
                                    crTotalTax = ToSafeDecimal(m_oDatabase.Parameters.Item("o_crTotalTax").Value)
                                End If
                            Next

                            If bPostPayment AndAlso nClaimPaymentID > 0 Then
                                If SavePaymentToAccounts(nClaimPaymentID, nClaimPerilID, sPayeeShortCode, crTotalPayment, crTotalTax, sPerilTypeCode) <> PMEReturnCode.PMTrue Then
                                    Throw New ApplicationException("Failed to SavePaymentToAccounts")
                                End If
                            End If
                        End If

                    Next

                End If
            End If

            Return PMEReturnCode.PMTrue

        Catch ex As ApplicationException
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:=ex.Message, vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        Finally
            oTaxArray = Nothing
        End Try

    End Function
    ''' <summary>
    ''' Used to get the PaymentaxFromATS
    ''' </summary>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="bIsExcess"></param>
    ''' <param name="sPartyName"></param>
    ''' <param name="nPaymentPartyType"></param>
    ''' <param name="crPaymentAmount"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="sTaxGroupCode"></param>
    ''' <param name="oAdvancedTaxArray"></param>
    ''' <param name="o_crScriptedTaxAmount"></param>
    ''' <param name="o_aoUpdatedTaxArray"></param>
    ''' <returns></returns>
    Public Function GetPaymentTaxFromATS(ByVal nClaimPerilID As Integer,
                                    ByVal bIsExcess As Boolean,
                                    ByVal sPartyName As String,
                                    ByVal nPaymentPartyType As Integer,
                                    ByVal crPaymentAmount As Decimal,
                                    ByVal sCurrencyCode As String,
                                    ByVal sTaxGroupCode As String,
                                    ByVal oAdvancedTaxArray As Object,
                                    ByRef o_crScriptedTaxAmount As Decimal,
                                    Optional ByRef o_aoUpdatedTaxArray As Object = Nothing) As Integer

        Const kMethodName As String = "GetPaymentTaxFromATS"

        Dim sAdvancedTaxScript As String = String.Empty
        Dim crTaxLossAmount As Decimal = 0
        Dim crTaxCurrencyAmount As Decimal = 0
        Dim crTaxBaseAmount As Decimal = 0
        Dim nCurrencyID As Integer
        Dim dsTaxBandRate As DataSet = Nothing
        Dim nTaxGroupID As Integer = 0
        Dim bIsATSArrayValid As Boolean = False

        Try

            If Informations.IsArray(oAdvancedTaxArray) AndAlso UBound(oAdvancedTaxArray, 1) = kPaymentParameters_FieldCount Then
                bIsATSArrayValid = True
            End If

            If Not bIsATSArrayValid Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                                   sMsg:="Invalid oAdvancedTaxArray passed.")
                Throw New Exception
            End If


            m_lReturn = GetAdvancedTaxScript(r_nTaxGroupID:=nTaxGroupID, o_sAdvancedTaxScript:=sAdvancedTaxScript, r_sTaxGroupCode:=sTaxGroupCode)

            If sAdvancedTaxScript = "" Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                             sMsg:="ATS Rule File is not specified for tax_group_ID " & nTaxGroupID & ".")
                Throw New Exception
            End If

            o_crScriptedTaxAmount = 0

            If sCurrencyCode.Trim = "" Then
                nCurrencyID = LossCurrencyID
            Else
                If GetCurrencyIDFromCode(sCurrencyCode, nCurrencyID) <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If
            End If

            If IsValidCurrency(nCurrencyID:=nCurrencyID) = False Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                          sMsg:="Invalid Currency ID.")
                Throw New Exception
            End If

            If CalculateTaxAmounts(nCompanyID:=0,
                                     nTaxGroupID:=nTaxGroupID,
                                     sTranstype:=kTaxTransTypeClaimPayment,
                                     nCurrencyID:=nCurrencyID,
                                     nLossCurrencyID:=LossCurrencyID,
                                     crAmount:=crPaymentAmount,
                                     o_crTaxCurrencyAmount:=crTaxCurrencyAmount,
                                     o_crTaxLossAmount:=crTaxLossAmount,
                                     o_crTaxBaseAmount:=crTaxBaseAmount,
                                     nClaimPerilID:=nClaimPerilID,
                                     nClaimPaymentID:=1,
                                     nClaimReceiptID:=0,
                                     nClaimPaymentItemID:=1,
                                     nClaimReceiptItemID:=0,
                                     o_dsTaxBandDetails:=dsTaxBandRate) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If CalculateATSTax(sAdvancedTaxScript:=sAdvancedTaxScript,
                                     bIsExcess:=bIsExcess,
                                     crPaymentAdjustment:=0,
                                     crPaymentAmount:=crPaymentAmount,
                                     dsTaxBandRate:=dsTaxBandRate,
                                     nPaymentPartyType:=nPaymentPartyType,
                                     sPayeeName:=sPartyName,
                                     sCurrencyCode:=sCurrencyCode,
                                     oAdvancedTaxDetatail:=oAdvancedTaxArray,
                                     o_crScriptedTaxAmount:=o_crScriptedTaxAmount,
                                     o_aoUpdatedTaxArray:=o_aoUpdatedTaxArray) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        Finally
            dsTaxBandRate = Nothing
        End Try
    End Function
    ''' <summary>
    ''' Used to get receiptTax from ATS
    ''' </summary>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="sPartyName"></param>
    ''' <param name="crReceiptAmount"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="sTaxGroupCode"></param>
    ''' <param name="oAdvancedTaxArray"></param>
    ''' <param name="o_crScriptedTaxAmount"></param>
    ''' <param name="o_aoUpdatedTaxArray"></param>
    ''' <returns></returns>
    Public Function GetReceiptTaxFromATS(ByVal nClaimPerilID As Integer,
                                  ByVal sPartyName As String,
                                  ByVal crReceiptAmount As Decimal,
                                  ByVal sCurrencyCode As String,
                                  ByVal sTaxGroupCode As String,
                                  ByVal oAdvancedTaxArray As Object,
                                  ByRef o_crScriptedTaxAmount As Decimal,
                                  Optional ByRef o_aoUpdatedTaxArray As Object = Nothing) As Integer

        Const kMethodName As String = "GetReceiptTaxFromATS"

        Dim sAdvancedTaxScript As String = String.Empty
        Dim crTaxLossAmount As Decimal = 0
        Dim crTaxCurrencyAmount As Decimal = 0
        Dim crTaxBaseAmount As Decimal = 0
        Dim nCurrencyID As Integer
        Dim dsTaxBandRate As DataSet = Nothing
        Dim nTaxGroupID As Integer = 0
        Dim bIsATSArrayValid As Boolean = False

        Try

            If Informations.IsArray(oAdvancedTaxArray) AndAlso UBound(oAdvancedTaxArray, 1) = kReceiptParameters_FieldCount Then
                bIsATSArrayValid = True
            End If

            If Not bIsATSArrayValid Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                                   sMsg:="Invalid oAdvancedTaxArray passed.")
                Throw New Exception
            End If


            m_lReturn = GetAdvancedTaxScript(r_nTaxGroupID:=nTaxGroupID, o_sAdvancedTaxScript:=sAdvancedTaxScript, r_sTaxGroupCode:=sTaxGroupCode)

            If sAdvancedTaxScript = "" Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                             sMsg:="ATS Rule File is not specified for tax_group_ID " & nTaxGroupID & ".")
                Throw New Exception
            End If

            o_crScriptedTaxAmount = 0

            If sCurrencyCode.Trim = "" Then
                nCurrencyID = LossCurrencyID
            Else
                If GetCurrencyIDFromCode(sCurrencyCode, nCurrencyID) <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If
            End If

            If IsValidCurrency(nCurrencyID:=nCurrencyID) = False Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                          sMsg:="Invalid Currency ID.")
                Throw New Exception
            End If

            If CalculateTaxAmounts(nCompanyID:=0,
                                 nTaxGroupID:=nTaxGroupID,
                                 sTranstype:=kTaxTransTypeClaimPayment,
                                 nCurrencyID:=nCurrencyID,
                                 nLossCurrencyID:=LossCurrencyID,
                                 crAmount:=crReceiptAmount,
                                 o_crTaxCurrencyAmount:=crTaxCurrencyAmount,
                                 o_crTaxLossAmount:=crTaxLossAmount,
                                 o_crTaxBaseAmount:=crTaxBaseAmount,
                                 nClaimPerilID:=nClaimPerilID,
                                 nClaimPaymentID:=0,
                                 nClaimReceiptID:=1,
                                 nClaimPaymentItemID:=0,
                                 nClaimReceiptItemID:=1,
                                 o_dsTaxBandDetails:=dsTaxBandRate) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            If CalculateATSTax(sAdvancedTaxScript:=sAdvancedTaxScript,
                            crPaymentAdjustment:=0,
                            crPaymentAmount:=crReceiptAmount,
                            dsTaxBandRate:=dsTaxBandRate,
                            sPayeeName:=sPartyName,
                            sCurrencyCode:=sCurrencyCode,
                            oAdvancedTaxDetatail:=oAdvancedTaxArray,
                            o_crScriptedTaxAmount:=o_crScriptedTaxAmount,
                            o_aoUpdatedTaxArray:=o_aoUpdatedTaxArray) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        Finally
            dsTaxBandRate = Nothing
        End Try
    End Function
    ''' <summary>
    ''' Used get System Calculated tax based on the taxgroup applied.
    ''' </summary>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="crPaymentAmount"></param>
    ''' <param name="sTaxGroupCode"></param>
    ''' <param name="o_crScriptedTaxAmount"></param>
    ''' <param name="o_aoUpdatedTaxArray"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="bCalculateForReceipt"></param>
    ''' <returns></returns>
    Public Function GetSystemCalculatedTax(ByVal nClaimPerilID As Integer,
                                    ByVal crPaymentAmount As Double,
                                    ByVal sTaxGroupCode As String,
                                    Optional ByRef o_crScriptedTaxAmount As Double = 0,
                                    Optional ByRef o_aoUpdatedTaxArray As Object = Nothing,
                                    Optional ByVal sCurrencyCode As String = "",
                                    Optional ByVal bCalculateForReceipt As Boolean = False) As Integer

        Const kMethodName As String = "GetSystemCalculatedTax"

        Dim crTaxLossAmount As Decimal = 0
        Dim crTaxCurrencyAmount As Decimal = 0
        Dim crTaxBaseAmount As Decimal = 0
        Dim nCurrencyID As Integer
        Dim dsTaxBandRate As DataSet = Nothing
        Dim nTaxGroupID As Integer = 0
        Dim oTaxGroup As Object = Nothing
        Dim aTaxParameters(,) As Object

        Try

            If sCurrencyCode.Trim = "" Then
                nCurrencyID = LossCurrencyID
            Else
                If GetCurrencyIDFromCode(sCurrencyCode, nCurrencyID) <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If
            End If

            If IsValidCurrency(nCurrencyID:=nCurrencyID) = False Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                          sMsg:="Invalid Currency ID.")
                Throw New Exception
            End If

            If GetTableData(oTaxGroup, "Tax_Group", "Tax_Group_id", "code='" & sTaxGroupCode & "'") = PMEReturnCode.PMTrue _
                AndAlso Informations.IsArray(oTaxGroup) Then
                nTaxGroupID = oTaxGroup(0, 0)
            End If

            If CalculateTaxAmounts(nCompanyID:=0,
                                     nTaxGroupID:=nTaxGroupID,
                                     sTranstype:=kTaxTransTypeClaimPayment,
                                     nCurrencyID:=nCurrencyID,
                                     nLossCurrencyID:=LossCurrencyID,
                                     crAmount:=crPaymentAmount,
                                     o_crTaxCurrencyAmount:=crTaxCurrencyAmount,
                                     o_crTaxLossAmount:=crTaxLossAmount,
                                     o_crTaxBaseAmount:=crTaxBaseAmount,
                                     nClaimPerilID:=nClaimPerilID,
                                     nClaimPaymentID:=If(bCalculateForReceipt, 0, 1),
                                     nClaimReceiptID:=If(bCalculateForReceipt, 1, 0),
                                     nClaimPaymentItemID:=If(bCalculateForReceipt, 0, 1),
                                     nClaimReceiptItemID:=If(bCalculateForReceipt, 1, 0),
                                     o_dsTaxBandDetails:=dsTaxBandRate) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If


            Dim crRunningTotalOfTaxAmount As Decimal = 0
            Dim icount As Integer
            'ARRAY OF TAX BAND RATES
            ReDim aTaxParameters(12, dsTaxBandRate.Tables(0).Rows.Count - 1)

            For icount = 0 To dsTaxBandRate.Tables(0).Rows.Count - 1
                aTaxParameters(TaxArray.TaxGroupId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupId)
                aTaxParameters(TaxArray.TaxBandId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandId)
                aTaxParameters(TaxArray.TaxCurrencyCode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxCurrencyCode)
                aTaxParameters(TaxArray.Percentage, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Percentage)
                aTaxParameters(TaxArray.Value, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Value)
                aTaxParameters(TaxArray.IsValue, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.IsValue)
                aTaxParameters(TaxArray.ClassOfBusinessId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.ClassOfBusinessId)
                aTaxParameters(TaxArray.Sequence, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Sequence)
                aTaxParameters(TaxArray.IsManuallyChanged, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.IsManuallyChanged)
                aTaxParameters(TaxArray.TaxGroupDescription, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupDescription)
                aTaxParameters(TaxArray.TaxBandDescription, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandDescription)
                aTaxParameters(TaxArray.TaxBandcode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandcode)
                aTaxParameters(TaxArray.TaxGroupCode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupCode)

                crRunningTotalOfTaxAmount += ToSafeDecimal(dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Value))
            Next
            If dsTaxBandRate.Tables(0).Rows.Count = 0 Then
                o_aoUpdatedTaxArray = Nothing
            Else
                o_aoUpdatedTaxArray = aTaxParameters
            End If


            o_crScriptedTaxAmount = crRunningTotalOfTaxAmount

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        Finally
            dsTaxBandRate = Nothing
            aTaxParameters = Nothing
        End Try
    End Function
#End Region


#Region "Private Methods"
    Private Function IsValidCurrency(ByVal nCurrencyID As Integer) As Boolean
        Dim bIsvalidCurrency As Boolean = False

        If m_dsCurrenciesForClaimBranch Is Nothing Then
            RetrieveCurrenciesForClaimBranch(CurrentClaimKey, m_dsCurrenciesForClaimBranch)
        End If

        If m_dsCurrenciesForClaimBranch IsNot Nothing Then
            For iRecord As Integer = 0 To m_dsCurrenciesForClaimBranch.Tables(0).Rows.Count
                If ToSafeInteger(m_dsCurrenciesForClaimBranch.Tables(0).Rows(iRecord).Item(0)) = nCurrencyID Then
                    bIsvalidCurrency = True
                    Exit For
                End If
            Next
        End If
        'If Informations.IsArray(dsCurrenciesForClaimBranch) Then
        '    Dim nTotalCurrencies As Integer = UBound(oCurrenciesForClaimBranch, 2)
        '    For iRecord As Integer = 0 To nTotalCurrencies
        '        If oCurrenciesForClaimBranch(0, iRecord) = nCurrencyID Then
        '            bIsvalidCurrency = True
        '            Exit For
        '        End If
        '    Next
        'End If

        Return bIsvalidCurrency

    End Function

    Private Function GetAdvancedTaxScript(ByRef r_nTaxGroupID As Integer, ByRef o_sAdvancedTaxScript As String,
            Optional ByRef r_sTaxGroupCode As String = "") As Integer
        Const kMethodName As String = "GetAdvancedTaxScript"
        Try
            m_oDatabase.Parameters.Clear()
            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "taxGroupId", r_nTaxGroupID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "advancedTaxScript", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)
            If r_sTaxGroupCode <> "" Then
                AddParameterLite(m_oDatabase, "sTaxGroupCode", r_sTaxGroupCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            End If
            AddParameterLite(m_oDatabase, "o_sTaxGroupCode", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "o_nTaxGroupID", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)


            ' Execute Action Query
            If m_oDatabase.SQLAction(
                sSQL:=kGetAdvancedTaxScriptSQL,
                sSQLName:=kGetAdvancedTaxScriptName,
                bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            r_nTaxGroupID = ToSafeInteger(m_oDatabase.Parameters.Item("o_nTaxGroupID").Value)
            o_sAdvancedTaxScript = ToSafeString(m_oDatabase.Parameters.Item("advancedTaxScript").Value)
            r_sTaxGroupCode = ToSafeString(m_oDatabase.Parameters.Item("o_sTaxGroupCode").Value)

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                              iType:=gPMConstants.PMELogLevel.PMLogError,
                              sMsg:="Method Failed!", vClass:=ACClass,
                              vMethod:=kMethodName,
                              excep:=ex)
            Return PMEReturnCode.PMFalse
        End Try

    End Function
    ''' <summary>
    ''' Calculate tax amounts
    ''' </summary>
    ''' <param name="nCompanyID"></param>
    ''' <param name="nTaxGroupID"></param>
    ''' <param name="sTranstype"></param>
    ''' <param name="nCurrencyID"></param>
    ''' <param name="nLossCurrencyID"></param>
    ''' <param name="crAmount"></param>
    ''' <param name="o_crTaxCurrencyAmount"></param>
    ''' <param name="o_crTaxLossAmount"></param>
    ''' <param name="o_crTaxBaseAmount"></param>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="nClaimPaymentID"></param>
    ''' <param name="nClaimReceiptID"></param>
    ''' <param name="nClaimPaymentItemID"></param>
    ''' <param name="nClaimReceiptItemID"></param>
    ''' <param name="o_dsTaxBandDetails"></param>
    ''' <returns></returns>
    Private Function CalculateTaxAmounts(ByVal nCompanyID As Integer,
                            ByVal nTaxGroupID As Integer,
                            ByVal sTranstype As String,
                            ByVal nCurrencyID As Integer,
                            ByVal nLossCurrencyID As Integer,
                            ByVal crAmount As Decimal,
                            ByRef o_crTaxCurrencyAmount As Decimal,
                            ByRef o_crTaxLossAmount As Decimal,
                            ByRef o_crTaxBaseAmount As Decimal,
                            ByVal nClaimPerilID As Integer,
                            ByVal nClaimPaymentID As Integer,
                            ByVal nClaimReceiptID As Integer,
                            ByVal nClaimPaymentItemID As Integer,
                            ByVal nClaimReceiptItemID As Integer,
                            ByRef o_dsTaxBandDetails As DataSet) As Integer

        Const kMethodName As String = "CalculateTaxAmounts"

        Try

            m_oDatabase.Parameters.Clear()

            AddParameterLite(m_oDatabase, "claim_payment_id", If(nClaimPaymentID = 0, DBNull.Value, nClaimPaymentID), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "claim_receipt_id", If(nClaimReceiptID = 0, DBNull.Value, nClaimReceiptID), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "claim_payment_item_id", If(nClaimPaymentItemID = 0, DBNull.Value, nClaimPaymentItemID), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "claim_receipt_item_id", If(nClaimReceiptItemID = 0, DBNull.Value, nClaimReceiptItemID), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            AddParameterLite(m_oDatabase, "company_id", nCompanyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "tax_group_id", nTaxGroupID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "transtype", sTranstype, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "amount", crAmount, PMEParameterDirection.PMParamInput, PMEDataType.PMDecimal)
            AddParameterLite(m_oDatabase, "claim_peril_id", nClaimPerilID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "calculate_only", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMBoolean)
            AddParameterLite(m_oDatabase, "Currency_id", nCurrencyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "loss_currency_id", nLossCurrencyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            AddParameterLite(m_oDatabase, "tax_currency_amount", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDecimal)
            AddParameterLite(m_oDatabase, "tax_loss_amount", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDecimal)
            AddParameterLite(m_oDatabase, "tax_base_amount", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDecimal)

            If m_oDatabase.ExecuteDataSet(kCalculateTaxAmountsSQL,
                                          kCalculateTaxAmountsName,
                                          True, o_dsTaxBandDetails) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            ' get total tax amounts....
            o_crTaxBaseAmount = ToSafeDecimal(m_oDatabase.Parameters.Item("tax_base_amount").Value)
            o_crTaxLossAmount = ToSafeDecimal(m_oDatabase.Parameters.Item("tax_loss_amount").Value)
            o_crTaxCurrencyAmount = ToSafeDecimal(m_oDatabase.Parameters.Item("tax_currency_amount").Value)

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=gPMConstants.PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        End Try

    End Function
    ''' <summary>
    ''' Load advance payment script
    ''' </summary>
    ''' <param name="crPaymentAmount"></param>
    ''' <param name="dsTaxBandRate"></param>
    ''' <param name="oAdvancedTaxDetatail"></param>
    ''' <param name="sPayeeName"></param>
    ''' <param name="nPaymentPartyType"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="crExcessAmount"></param>
    ''' <param name="crPaymentAdjustment"></param>
    ''' <param name="r_aDataArray"></param>
    ''' <returns></returns>
    Private Function LoadAdvancedPaymentScript(ByVal crPaymentAmount As Decimal,
                                        ByVal dsTaxBandRate As DataSet,
                                        ByVal oAdvancedTaxDetatail As Object,
                                        ByVal sPayeeName As String,
                                        ByVal nPaymentPartyType As Integer,
                                        ByVal sCurrencyCode As String,
                                        ByVal crExcessAmount As Decimal,
                                        ByVal crPaymentAdjustment As Decimal,
                                        ByRef r_aDataArray As Object()) As Integer


        Try
            ReDim r_aDataArray(kPaymentParameters_FieldCount)
            Dim aTaxParameters(,) As Object

            r_aDataArray(kPaymentParameters_ProcessType) = oAdvancedTaxDetatail(kATSProcessMode)
            r_aDataArray(kPaymentParameters_Payee) = sPayeeName
            r_aDataArray(kPaymentParameters_PaymentToCode) = nPaymentPartyType
            r_aDataArray(kPaymentParameters_SafeHarbourCode) = oAdvancedTaxDetatail(kATSPayment_SafeHarbourCode)
            r_aDataArray(kPaymentParameters_SafeHarbourPercentage) = oAdvancedTaxDetatail(kATSPayment_SafeHarbourPercentage)
            r_aDataArray(kPaymentParameters_InsuredDomiciled) = oAdvancedTaxDetatail(kATSPayment_IsInsuredDomiciled)
            r_aDataArray(kPaymentParameters_InsuredPercentage) = oAdvancedTaxDetatail(kATSPayment_InsuredPercentage)
            r_aDataArray(kPaymentParameters_InsuredTaxNumber) = oAdvancedTaxDetatail(kATSPayment_InsuranceTaxNumber)
            r_aDataArray(kPaymentParameters_PayeeDomiciled) = oAdvancedTaxDetatail(kATSPayment_IsPayeeDomiciled)
            r_aDataArray(kPaymentParameters_PayeePercentage) = oAdvancedTaxDetatail(kATSPayment_PayeePercentage)
            r_aDataArray(kPaymentParameters_PayeeTaxNumber) = oAdvancedTaxDetatail(kATSPayment_PayeeTaxNumber)
            r_aDataArray(kPaymentParameters_IsTaxExempt) = oAdvancedTaxDetatail(kATSPayment_IsTaxExempt)
            r_aDataArray(kPaymentParameters_IsWHTExempt) = oAdvancedTaxDetatail(kATSPayment_IsWHTExempt)
            r_aDataArray(kPaymentParameters_IsSettlement) = oAdvancedTaxDetatail(kATSPayment_IsSettlement)
            r_aDataArray(kPaymentParameters_CurrencyCode) = sCurrencyCode
            r_aDataArray(kPaymentParameters_Amount) = crPaymentAmount
            r_aDataArray(kPaymentParameters_ExcessAmount) = crExcessAmount
            r_aDataArray(kPaymentParameters_PaymentAdjustment) = crPaymentAdjustment

            Dim icount As Integer
            ' array of tax band rates
            ReDim aTaxParameters(12, dsTaxBandRate.Tables(0).Rows.Count - 1)

            For icount = 0 To dsTaxBandRate.Tables(0).Rows.Count - 1
                aTaxParameters(TaxArray.TaxGroupId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupId)
                aTaxParameters(TaxArray.TaxBandId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandId)
                aTaxParameters(TaxArray.TaxCurrencyCode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxCurrencyCode)
                aTaxParameters(TaxArray.Percentage, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Percentage)
                aTaxParameters(TaxArray.Value, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Value)
                aTaxParameters(TaxArray.IsValue, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.IsValue)
                aTaxParameters(TaxArray.ClassOfBusinessId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.ClassOfBusinessId)
                aTaxParameters(TaxArray.Sequence, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Sequence)
                aTaxParameters(TaxArray.IsManuallyChanged, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.IsManuallyChanged)
                aTaxParameters(TaxArray.TaxGroupDescription, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupDescription)
                aTaxParameters(TaxArray.TaxBandDescription, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandDescription)
                aTaxParameters(TaxArray.TaxBandcode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandcode)
                aTaxParameters(TaxArray.TaxGroupCode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupCode)
            Next
            ' insert tax details at 18th position in the DataArray
            r_aDataArray(kPaymentParameters_TaxArray) = aTaxParameters
            r_aDataArray(kPaymentParameters_ErrorMessage) = String.Empty
            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            'No Error Loging Here it will be handled by calling code
            Return PMEReturnCode.PMError
        End Try
    End Function
    ''' <summary>
    ''' Load AdvanceReceiptScript
    ''' </summary>
    ''' <param name="crReceiptAmount"></param>
    ''' <param name="dsTaxBandRate"></param>
    ''' <param name="oAdvancedTaxDetatail"></param>
    ''' <param name="sPayeeName"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="r_aDataArray"></param>
    ''' <returns></returns>
    Private Function LoadAdvancedReceiptScript(ByVal crReceiptAmount As Decimal,
                                    ByVal dsTaxBandRate As DataSet,
                                    ByVal oAdvancedTaxDetatail As Object,
                                    ByVal sPayeeName As String,
                                    ByVal sCurrencyCode As String,
                                    ByRef r_aDataArray As Object()) As Integer


        Try
            ReDim r_aDataArray(kReceiptParameters_FieldCount)
            Dim aTaxParameters(,) As Object

            r_aDataArray(kReceiptParameters_ProcessType) = oAdvancedTaxDetatail(kATSProcessMode)
            r_aDataArray(kReceiptParameters_Payee) = sPayeeName
            r_aDataArray(kReceiptParameters_PaymentToCode) = oAdvancedTaxDetatail(kATSReceipt_PaymentToCode)
            r_aDataArray(kReceiptParameters_InsuredDomiciled) = oAdvancedTaxDetatail(kATSReceipt_IsInsuredDomiciled)
            r_aDataArray(kReceiptParameters_InsuredPercentage) = oAdvancedTaxDetatail(kATSReceipt_InsuredPercentage)
            r_aDataArray(kReceiptParameters_InsuredTaxNumber) = oAdvancedTaxDetatail(kATSPayment_InsuranceTaxNumber)
            r_aDataArray(kReceiptParameters_IsTaxExempt) = oAdvancedTaxDetatail(kATSReceipt_IsTaxExempt)
            r_aDataArray(kReceiptParameters_IsSettlement) = oAdvancedTaxDetatail(kATSReceipt_IsSettlement)
            r_aDataArray(kReceiptParameters_CurrencyCode) = sCurrencyCode
            r_aDataArray(kReceiptParameters_Amount) = crReceiptAmount
            ' ARRAY OF TAX BAND RATES
            Dim icount As Integer
            ' array of tax band rates
            ReDim aTaxParameters(12, dsTaxBandRate.Tables(0).Rows.Count - 1)

            For icount = 0 To dsTaxBandRate.Tables(0).Rows.Count - 1
                aTaxParameters(TaxArray.TaxGroupId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupId)
                aTaxParameters(TaxArray.TaxBandId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandId)
                aTaxParameters(TaxArray.TaxCurrencyCode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxCurrencyCode)
                aTaxParameters(TaxArray.Percentage, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Percentage)
                aTaxParameters(TaxArray.Value, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Value)
                aTaxParameters(TaxArray.IsValue, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.IsValue)
                aTaxParameters(TaxArray.ClassOfBusinessId, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.ClassOfBusinessId)
                aTaxParameters(TaxArray.Sequence, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.Sequence)
                aTaxParameters(TaxArray.IsManuallyChanged, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.IsManuallyChanged)
                aTaxParameters(TaxArray.TaxGroupDescription, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupDescription)
                aTaxParameters(TaxArray.TaxBandDescription, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandDescription)
                aTaxParameters(TaxArray.TaxBandcode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxBandcode)
                aTaxParameters(TaxArray.TaxGroupCode, icount) = dsTaxBandRate.Tables(0).Rows(icount).Item(TaxArray.TaxGroupCode)
            Next

            r_aDataArray(kReceiptParameters_TaxArray) = aTaxParameters
            r_aDataArray(kReceiptParameters_ReceivablePercentage) = oAdvancedTaxDetatail(kATSReceipt_ReceivablePercentage)
            r_aDataArray(kReceiptParameters_ErrorMessage) = String.Empty

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            'No Error Loging Here it will be handled by calling code
            Return PMEReturnCode.PMError
        End Try
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sAdvancedTaxScript"></param>
    ''' <param name="crPaymentAmount"></param>
    ''' <param name="dsTaxBandRate"></param>
    ''' <param name="oAdvancedTaxDetatail"></param>
    ''' <param name="sPayeeName"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="o_crScriptedTaxAmount"></param>
    ''' <param name="o_aoUpdatedTaxArray"></param>
    ''' <param name="bIsExcess"></param>
    ''' <param name="nPaymentPartyType"></param>
    ''' <param name="crPaymentAdjustment"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CalculateATSTax(ByVal sAdvancedTaxScript As String,
                                        ByVal crPaymentAmount As Decimal,
                                        ByVal dsTaxBandRate As DataSet,
                                        ByVal oAdvancedTaxDetatail As Object,
                                        ByVal sPayeeName As String,
                                        ByVal sCurrencyCode As String,
                                        ByRef o_crScriptedTaxAmount As Decimal,
                                        Optional ByRef o_aoUpdatedTaxArray As Object = Nothing,
                                        Optional ByVal bIsExcess As Boolean = False,
                                        Optional ByVal nPaymentPartyType As Integer = 0,
                                        Optional ByVal crPaymentAdjustment As Decimal = 0) As Integer

        Const kMethodName As String = "CalculateATSTax"
        Dim nTaxArrayItems As Integer
        'Scripted Tax
        Dim aUpdatedTaxParameters() As Object
        Dim aUpdatedTaxArray As Object(,)
        Dim aDataArray() As Object = Nothing
        Dim crExcessAmount As Decimal

        Dim oCLMPeril As bCLMPeril.Business
        Dim nlBound As Integer
        Dim nUbound As Integer
        Dim nTaxItem As Integer
        Dim crTaxAmount As Decimal
        Dim sScriptProcessMode As String
        Dim nTaxScriptMode As Integer = 0
        o_crScriptedTaxAmount = 0

        If bIsExcess Then
            crExcessAmount = crPaymentAmount
        End If

        If sAdvancedTaxScript.Trim = String.Empty Then
            'No Script is attached So response is true but th value is 0
            Return PMEReturnCode.PMTrue
        End If

        'Calculate Scripted amount and set it with structure
        sScriptProcessMode = ToSafeString(oAdvancedTaxDetatail(kATSProcessMode))

        If sScriptProcessMode = kProcessMode_Payment Then
            nTaxScriptMode = 1
            nTaxArrayItems = kPaymentParameters_TaxArray

            If LoadAdvancedPaymentScript(crPaymentAmount:=crPaymentAmount,
                                        dsTaxBandRate:=dsTaxBandRate,
                                        oAdvancedTaxDetatail:=oAdvancedTaxDetatail,
                                        sPayeeName:=sPayeeName,
                                        nPaymentPartyType:=nPaymentPartyType,
                                        sCurrencyCode:=sCurrencyCode,
                                        crExcessAmount:=crExcessAmount,
                                        crPaymentAdjustment:=crPaymentAdjustment,
                                        r_aDataArray:=aDataArray) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("CalculateATSTax - LoadAdvancedPaymentScript Failed.")
            End If

        Else
            nTaxArrayItems = kReceiptParameters_TaxArray
            If LoadAdvancedReceiptScript(crReceiptAmount:=crPaymentAmount,
                                         dsTaxBandRate:=dsTaxBandRate,
                                         oAdvancedTaxDetatail:=oAdvancedTaxDetatail,
                                         sPayeeName:=sPayeeName,
                                         sCurrencyCode:=sCurrencyCode,
                                         r_aDataArray:=aDataArray) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("CalculateATSTax - LoadAdvancedReceiptScript Failed.")
            End If
        End If
        ' initialise CLMPeril
        oCLMPeril = New bCLMPeril.Business
        If oCLMPeril.Initialise(sUsername:=m_sUsername,
                                   sPassword:=m_sPassword,
                                   iUserID:=m_iUserID,
                                   iSourceID:=m_iSourceID,
                                   iLanguageID:=m_iLanguageID,
                                   iCurrencyID:=m_iCurrencyID,
                                   iLogLevel:=m_iLogLevel,
                                   sCallingAppName:=m_sCallingAppName,
                                   vDatabase:=m_oDatabase) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("CalculateATSTax - bCLMPeril.Business.Initialise Failed.")
        End If

        ReDim aUpdatedTaxParameters(nTaxArrayItems)
        Dim oUpdatedTaxParameters As Object() = aUpdatedTaxParameters

        'called executeAdvancedScript function
        oCLMPeril.ExecuteAdvancedTaxScript(
                        v_lTaxScriptMode:=nTaxScriptMode,
                        v_sTaxScriptName:=sAdvancedTaxScript,
                        v_vTaxParameters:=aDataArray,
                        nTaxGroupID:=0,
                        r_vUpdatedTaxParameters:=oUpdatedTaxParameters)

        aUpdatedTaxParameters = DirectCast(oUpdatedTaxParameters, Object())

        aUpdatedTaxArray = DirectCast(aUpdatedTaxParameters(nTaxArrayItems), Object(,))

        nlBound = aUpdatedTaxArray.GetLowerBound(1) ' LBound(aUpdatedTaxArray, 2)
        nUbound = aUpdatedTaxArray.GetUpperBound(1) ' UBound(aUpdatedTaxArray, 2)

        For nTaxItem = nlBound To nUbound
            crTaxAmount = CDec(aUpdatedTaxArray(TaxArray.Value, nTaxItem))
            o_crScriptedTaxAmount = o_crScriptedTaxAmount + crTaxAmount
        Next
        o_aoUpdatedTaxArray = aUpdatedTaxArray

        aUpdatedTaxParameters = Nothing
        aUpdatedTaxArray = Nothing
        oCLMPeril.Dispose()
        oCLMPeril = Nothing
        Return PMEReturnCode.PMTrue
    End Function
    ''' <summary>
    ''' GetClaimCurrency
    ''' </summary>
    ''' <param name="nClaimID"></param>
    ''' <param name="o_nLossCurrencyID"></param>
    ''' <param name="o_sLossCurrencyName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClaimCurrency(ByVal nClaimID As Integer, ByRef o_nLossCurrencyID As Integer, ByRef o_sLossCurrencyName As String) As Integer
        Dim dsResult As DataSet = Nothing

        AddParameterLite(m_oDatabase, "claim_id", nClaimID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        If dsResult IsNot Nothing Then
            o_nLossCurrencyID = ToSafeInteger(dsResult.Tables(0).Rows(0).Item(0))
            o_sLossCurrencyName = ToSafeString(dsResult.Tables(0).Rows(0).Item(1))
        End If

        If m_oDatabase.ExecuteDataSet(kGetClaimCurrencySQL,
                                  kGetClaimCurrencyName,
                                  True, dsResult) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to Execute " + kGetClaimCurrencySQL)
        End If

        If dsResult IsNot Nothing Then
            o_nLossCurrencyID = ToSafeInteger(dsResult.Tables(0).Rows(0).Item(0))
            o_sLossCurrencyName = ToSafeString(dsResult.Tables(0).Rows(0).Item(1))
        End If

        Return PMEReturnCode.PMTrue


    End Function

    ''' <summary>
    ''' RetrieveCurrenciesForClaimBranch
    ''' </summary>
    ''' <param name="nClaimID"></param>
    ''' <param name="o_oCurrencies"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function RetrieveCurrenciesForClaimBranch(ByVal nClaimID As Integer, ByRef o_oCurrencies As DataSet) As Integer
        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "claim_id", nClaimID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        If m_oDatabase.ExecuteDataSet(kRetrieveCurrenciesForClaimBranchSQL,
                                    kRetrieveCurrenciesForClaimBranchName,
                                    True, o_oCurrencies) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to Execute " + kRetrieveCurrenciesForClaimBranchSQL)
        End If
        Return PMEReturnCode.PMTrue
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sCurrencyCode"></param>
    ''' <param name="o_nCurrencyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCurrencyIDFromCode(ByVal sCurrencyCode As String, ByRef o_nCurrencyID As Integer) As Integer
        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "currencyCode", sCurrencyCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
        AddParameterLite(m_oDatabase, "currencyId", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)

        If m_oDatabase.SQLAction(sSQL:=kGetCurrencyIdFromCurrencyCodeSQL,
                            sSQLName:=kGetCurrencyIdFromCurrencyCodeName,
                            bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to Execute " + kGetCurrencyIdFromCurrencyCodeSQL)
        End If

        o_nCurrencyID = m_oDatabase.Parameters.Item("currencyId").Value
        Return PMEReturnCode.PMTrue
    End Function

    ''' <summary>
    ''' SaveTaxBandInfo
    ''' </summary>
    ''' <param name="oTaxArray"></param>
    ''' <param name="nReserveID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SaveTaxBandInfo(ByVal oTaxArray(,) As Object, ByVal nReserveID As Integer) As Integer

        If Informations.IsArray(oTaxArray) Then
            Dim nlBound As Integer = oTaxArray.GetLowerBound(1) ' LBound(oTaxArray, 2)
            Dim nUbound As Integer = oTaxArray.GetUpperBound(1) ' UBound(oTaxArray, 2)

            AddParameterLite(m_oDatabase, "nReserveID", nReserveID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

            If m_oDatabase.SQLAction(sSQL:=kClearTaxBandInfoSQL,
                                    sSQLName:=kClearTaxBandInfoName,
                                    bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Execute " + kClearTaxBandInfoSQL)
            End If

            For iTaxBand As Integer = nlBound To nUbound

                Dim nTaxBandID As Integer = ToSafeInteger(oTaxArray(TaxArray.TaxBandId, iTaxBand))
                Dim dRate As Double = ToSafeDouble(oTaxArray(TaxArray.Percentage, iTaxBand))
                Dim bIsValue As Boolean = ToSafeBoolean(ToSafeInteger(oTaxArray(TaxArray.IsValue, iTaxBand)))
                Dim nClassOfBusinessID As Integer = ToSafeInteger(oTaxArray(TaxArray.ClassOfBusinessId, iTaxBand))
                Dim crTaxAmount As Decimal = ToSafeDecimal(oTaxArray(TaxArray.Value, iTaxBand))

                m_oDatabase.Parameters.Clear()
                AddParameterLite(m_oDatabase, "nReserveID", nReserveID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
                AddParameterLite(m_oDatabase, "nTaxBandID", nTaxBandID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "dRate", dRate, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
                AddParameterLite(m_oDatabase, "bIsValue", bIsValue, PMEParameterDirection.PMParamInput, PMEDataType.PMBoolean)
                AddParameterLite(m_oDatabase, "nClassOfBusinessID", nClassOfBusinessID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                AddParameterLite(m_oDatabase, "crTaxAmount", crTaxAmount, PMEParameterDirection.PMParamInput, PMEDataType.PMCurrency)

                If m_oDatabase.SQLAction(sSQL:=kAddTaxBandInfoSQL,
                                        sSQLName:=kAddTaxBandInfoName,
                                        bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("Failed to Execute " + kAddTaxBandInfoSQL)
                End If
            Next
        End If
        Return PMEReturnCode.PMTrue
    End Function
    Private Sub PutAllTaxOnFirstBand(ByRef o_aoUpdatedTaxArray As Object, ByVal crTaxAmount As Decimal)
        Dim icount As Integer
        Dim nTaxBands As Integer = UBound(o_aoUpdatedTaxArray, 2)
        'CLEAR ALL THE BANDS
        For icount = 0 To nTaxBands
            o_aoUpdatedTaxArray(TaxArray.Value, icount) = 0
            o_aoUpdatedTaxArray(TaxArray.IsValue, icount) = 1
            o_aoUpdatedTaxArray(TaxArray.Percentage, icount) = 0
        Next
        'PUT ALL THE TAX VALUE ON FIRST BAND 
        o_aoUpdatedTaxArray(TaxArray.Value, 0) = crTaxAmount
    End Sub

#End Region

End Class
