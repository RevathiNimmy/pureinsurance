Option Strict Off
Option Explicit On
Imports System.Data
Imports dPMDAO.PMConstants
Imports SSP.Shared

Partial Public Class Business


#Region "Public Types"

#End Region

#Region "Private Types"

#End Region

#Region "Public Constants"
    Public Const kPAYEETYPE_ClaimPayable As Integer = 1
    Public Const kPAYEETYPE_Party As Integer = 2
    Public Const kPAYEETYPE_Agent As Integer = 4
    Public Const kPAYEETYPE_Client As Integer = 8

    Public Const kTRANSACTIONTYPE_OpenClaim As String = "C_CO"
    Public Const kTRANSACTIONTYPE_MaintainClaim As String = "C_CR"
    Public Const kTRANSACTIONTYPE_PayClaim As String = "C_CP"
    Public Const kTRANSACTIONTYPE_MidTermAdjustment As String = "MTA"
    Public Const kTRANSACTIONTYPE_MidTermCancellation As String = "MTC"
    Public Const kTRANSACTIONTYPE_NewBusiness As String = "NB"

#End Region

#Region "Private Constants"
    Private Const kDefaultRulePathSubKey As String = "GIS\"
    Private Const kTaxTransTypeClaimPayment As String = "TTCP"
    Private Const kProcessMode_Payment As String = "CLP"
    Private Const kProcessMode_ReceiptFromRules As String = "CLR_RULES"
    Private Const kATSProcessMode As Integer = 0

    Private Const kIsManuallyChangedDefault As Integer = 0
    Private Const kIsManuallyChangedUser As Integer = 1
    Private Const kIsManuallyChangedScript As Integer = 2

#End Region

#Region "Public Variables"

#End Region

#Region "Private Variables"
    Private m_nPreviousInsuranceFileCnt As Integer = 0
    Private m_nPreviousClaimID As Integer = 0
    Private m_nDeclinedClaimKey As Integer = 0
    Private m_nPreviousValidClaimKey As Integer = 0
    Private m_bIsPreviousClaimDeclined? As Boolean
    Private m_sLossCurrencyName As String = String.Empty
    Private m_nLossCurrencyID As Integer = 0
    Private m_dsCurrenciesForClaimBranch As DataSet = Nothing
    Private m_bIsCalledViaSAM As Boolean = False
    Private m_sCurrentClaimNumber As String = String.Empty
    Private m_bIsPostTaxes As Boolean?
    Private m_bIsGrossClaimPayment As Boolean?
#End Region

#Region "Public Properties"
    'MAKE SURE IN SAM IT ADDCALLFROMSTS IS SET
    Public Property IsCalledViaSAM() As Boolean
        Get
            Return m_bIsCalledViaSAM
        End Get
        Set(ByVal value As Boolean)
            m_bIsCalledViaSAM = value
        End Set
    End Property

    Public ReadOnly Property CurrentClaimNumber() As String
        Get
            If m_sCurrentClaimNumber = String.Empty Then
                GetBasicClaimInformation()
            End If
            Return m_sCurrentClaimNumber
        End Get
    End Property

    Public ReadOnly Property CurrentClaimKey() As Integer
        Get
            If m_lClaimCnt = 0 AndAlso (m_sTransactionType = kTRANSACTIONTYPE_OpenClaim Or m_sTransactionType = kTRANSACTIONTYPE_PayClaim Or m_sTransactionType = kTRANSACTIONTYPE_MaintainClaim) Then
                m_lReturn = GetClaimCnt()
            End If
            Return m_lClaimCnt
        End Get
    End Property

    Public ReadOnly Property CurrentInsuranceFileKey() As Integer
        Get
            If m_lInsuranceFileCnt = 0 AndAlso Not (m_sTransactionType = kTRANSACTIONTYPE_OpenClaim Or m_sTransactionType = kTRANSACTIONTYPE_PayClaim Or m_sTransactionType = kTRANSACTIONTYPE_MaintainClaim) Then
                m_lReturn = GetInsuranceFileCnt()
            End If
            Return m_lInsuranceFileCnt
        End Get
    End Property

    Public ReadOnly Property PreviousClaimKey() As Integer
        Get
            If m_nPreviousClaimID = 0 Then
                m_lReturn = GetClaimKeys(m_nPreviousClaimID, 0)
            End If
            Return m_nPreviousClaimID
        End Get
    End Property

    Public ReadOnly Property PreviousInsuranceFileKey() As Integer
        Get
            If m_nPreviousInsuranceFileCnt = 0 Then
                m_lReturn = GetInsuranceFileKeys(m_nPreviousInsuranceFileCnt, 0)
            End If
            Return m_nPreviousInsuranceFileCnt
        End Get
    End Property

    Public ReadOnly Property IsPreviousClaimDeclined() As Boolean
        Get
            If Not m_bIsPreviousClaimDeclined.HasValue Then
                GetClaimPaymentDeclineDetails(m_bIsPreviousClaimDeclined, m_nDeclinedClaimKey, m_nPreviousValidClaimKey)
            End If
            Return m_bIsPreviousClaimDeclined
        End Get
    End Property

    Public ReadOnly Property DeclinedClaimKey() As Integer
        Get
            If IsPreviousClaimDeclined Then
                Return m_nDeclinedClaimKey
            Else
                Return 0
            End If
        End Get
    End Property

    Public ReadOnly Property PreviousValidClaimKey() As Integer
        Get
            If IsPreviousClaimDeclined Then
                Return m_nPreviousValidClaimKey
            Else
                Return 0
            End If
        End Get
    End Property
#End Region

#Region "Private Properties"
    Private ReadOnly Property IsPostTaxes() As String
        Get
            If Not m_bIsPostTaxes.HasValue Then
                GetBasicClaimInformation()
            End If
            Return m_bIsPostTaxes
        End Get
    End Property

    Private ReadOnly Property IsGrossClaimPayment() As String
        Get
            If Not m_bIsGrossClaimPayment.HasValue Then
                GetBasicClaimInformation()
            End If
            Return m_bIsGrossClaimPayment
        End Get
    End Property
#End Region

#Region "Public Methods"

    Public Function GetNewUID() As String
        Return Guid.NewGuid.ToString()
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="nReserveTypeID"></param>
    ''' <param name="o_oClaimPaymentItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNewClaimPaymentItem(ByVal nClaimPerilID As Integer, ByVal nReserveTypeID As Integer, ByRef o_oClaimPaymentItem As Object) As Integer
        Const kMethodName As String = "GetNewClaimPaymentItem"
        Dim dsResult As DataSet
        Try
            o_oClaimPaymentItem = Nothing

            m_oDatabase.Parameters.Clear()
            ' Add Required Stored Procedure Parameters
            AddParameterLite(m_oDatabase, "nClaimPerilID", nClaimPerilID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "nReserveTypeID", nReserveTypeID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            If m_oDatabase.ExecuteDataSet(kGetNewClaimPaymentItemSQL,
                                      kGetNewClaimPaymentItemName,
                                      True, dsResult) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to execute " & kGetNewClaimPaymentItemSQL)
            End If

            If dsResult Is Nothing Then
                Return PMEReturnCode.PMFalse
            Else
                ReDim o_oClaimPaymentItem(kRESERVE_CP_FieldCount)
                For iColumn As Integer = 0 To kRESERVE_CP_FieldCount
                    o_oClaimPaymentItem(iColumn) = dsResult.Tables(0).Rows(0).Item(iColumn)
                Next
            End If

            Return PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        Finally
            dsResult = Nothing
        End Try

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="o_oData"></param>
    ''' <param name="sObjectName"></param>
    ''' <param name="sColumns"></param>
    ''' <param name="sUIDValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPreviousRiskData(ByRef o_oData As Object, ByVal sObjectName As String, Optional ByVal sColumns As String = "",
                                      Optional ByVal sUIDValue As String = "") As Integer

        sObjectName = sObjectName.Trim()
        sColumns = sColumns.Trim()
        sUIDValue = sUIDValue.Trim()
        Dim nCurrentClaimKey As Integer = CurrentClaimKey
        Dim nPreviousValidClaimKey As Integer = PreviousValidClaimKey
        Dim nCurrentInsuranceFileKey As Integer = CurrentInsuranceFileKey
        If sObjectName = "" Or sColumns = "" Then
            Return PMEReturnCode.PMFalse
        End If

        m_oDatabase.Parameters.Clear()

        AddParameterLite(m_oDatabase, "sObjectName", sObjectName, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

        If sColumns <> "" Then
            AddParameterLite(m_oDatabase, "sColumns", sColumns, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        End If

        If sUIDValue <> "" Then
            AddParameterLite(m_oDatabase, "sUIDValue", sUIDValue, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        End If

        'FIRST CHECK IF IT IS CLAIM 
        If nCurrentClaimKey > 0 Then
            AddParameterLite(m_oDatabase, "nClaimKey", nCurrentClaimKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            If nPreviousValidClaimKey > 0 Then
                AddParameterLite(m_oDatabase, "nPreviousClaimKey", nPreviousValidClaimKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
                'THIS IS A PUBLIC METHOD AND SCRIPT WILL ONLY RECOGNISE ARRAY SO NOT USING DATASET
                If m_oDatabase.SQLSelect(sSQL:=kGetPreviousRiskDataSQL,
                              sSQLName:=kGetPreviousRiskDataName,
                              bStoredProcedure:=True,
                              vResultArray:=o_oData,
                              bKeepNulls:=True) <> PMEReturnCode.PMTrue Then
                    Throw New Exception
                End If

            ElseIf nCurrentInsuranceFileKey > 0 Then
                AddParameterLite(m_oDatabase, "nInsuranceFileKey", nCurrentInsuranceFileKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            Else
                'WE CANT HAVE SUPPORT TO RETRIVE PREVIOUS DATA AS THERE IS NO VERSIONING OF PARTY DATA
                Return PMEReturnCode.PMFalse
            End If
        End If

        'THIS IS A PUBLIC METHOD AND SCRIPT WILL ONLY RECOGNISE ARRAY SO NOT USING DATASET
        If m_oDatabase.SQLSelect(sSQL:=kGetPreviousRiskDataSQL,
                      sSQLName:=kGetPreviousRiskDataName,
                      bStoredProcedure:=True,
                      vResultArray:=o_oData, bKeepNulls:=True) <> PMEReturnCode.PMTrue Then
            Throw New Exception
        End If

        Return PMEReturnCode.PMTrue

    End Function

    Public Function GetTableData(ByRef o_oData As Object, ByVal sTableName As String, ByVal sColumns As String,
                                  ByVal sSQLCondition As String) As Integer
        Const kMethodName As String = "GetUDLData"
        Try
            o_oData = Nothing

            sTableName = sTableName.Trim().ToUpper()
            sColumns = sColumns.Trim().ToUpper()
            sSQLCondition = sSQLCondition.Trim().ToUpper()

            'GIVEN BELOW MESSAGES ARE FOR PRODUCT DEVELOPER TO PASS CORRECT INFORMATION
            If sTableName = String.Empty OrElse sTableName.Contains(" ") Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                        sMsg:="Incorrect or no Table name specified.",
                         vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Return PMEReturnCode.PMFalse
            ElseIf sColumns = String.Empty OrElse sColumns.Contains("*") Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                        sMsg:="Incorrect column name or names specified.",
                         vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Return PMEReturnCode.PMFalse
            ElseIf sSQLCondition = String.Empty Then
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                              sMsg:="No SQL condition specified.",
                               vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName)
                Return PMEReturnCode.PMFalse
            End If

            'STOP USER TO RUN UNWANTED COMMANDS
            sColumns = GetSQLSafeString(sColumns)
            sSQLCondition = GetSQLSafeString(sSQLCondition)
            sTableName = GetSQLSafeString(sTableName)

            m_oDatabase.Parameters.Clear()
            AddParameterLite(m_oDatabase, "sTableName", sTableName, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "sColumns", sColumns, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "sCondition", sSQLCondition, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            'THIS IS A PUBLIC METHOD AND SCRIPT WILL ONLY RECOGNISE ARRAY SO NOT USING DATASET
            If m_oDatabase.SQLSelect(sSQL:=kGetGetTableDataSQL,
                          sSQLName:=kGetGetTableDataName,
                          bStoredProcedure:=True,
                          vResultArray:=o_oData,
                          bKeepNulls:=True) <> PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername,
                               iType:=PMELogLevel.PMLogError,
                               sMsg:="Method Failed!", vClass:=ACClass,
                               vMethod:=kMethodName,
                               excep:=ex)
            Return PMEReturnCode.PMFalse
        End Try

    End Function

#End Region

#Region "Private Methods"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="o_nPreviousClaimKey"></param>
    ''' <param name="nCurrentClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClaimKeys(ByRef o_nPreviousClaimKey As Integer, ByVal nCurrentClaimKey As Integer) As Integer
        o_nPreviousClaimKey = 0
        If Not (m_sTransactionType = kTRANSACTIONTYPE_OpenClaim Or m_sTransactionType = kTRANSACTIONTYPE_PayClaim Or m_sTransactionType = kTRANSACTIONTYPE_MaintainClaim) Then
            'RETURN WITH 0 VALUE AS THE CALL IS NOT FOR CLAIMS
            Return PMEReturnCode.PMTrue
        End If

        If nCurrentClaimKey = 0 AndAlso CurrentClaimKey = 0 Then
            Return PMEReturnCode.PMFalse
        End If

        nCurrentClaimKey = if(nCurrentClaimKey = 0, m_lClaimCnt, nCurrentClaimKey)
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddParameterLite(m_oDatabase, "nClaimKey", nCurrentClaimKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "nPreviousClaimKey", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
        ' Execute Action Query
        If m_oDatabase.SQLAction(
            sSQL:=kGetPreviousClaimKeySQL,
            sSQLName:=kGetPreviousClaimKeyName,
            bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
            Throw New Exception
        End If

        o_nPreviousClaimKey = m_oDatabase.Parameters.Item("nPreviousClaimKey").Value
        Return PMEReturnCode.PMTrue

    End Function
    ''' <summary>
    ''' GetInsuranceFileKeys
    ''' </summary>
    ''' <param name="o_nPreviousInsuranceFileKey"></param>
    ''' <param name="nCurrentInsuranceFileKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInsuranceFileKeys(ByRef o_nPreviousInsuranceFileKey As Integer, ByVal nCurrentInsuranceFileKey As Integer) As Integer

        o_nPreviousInsuranceFileKey = 0
        If m_sTransactionType = kTRANSACTIONTYPE_OpenClaim Or m_sTransactionType = kTRANSACTIONTYPE_PayClaim Or m_sTransactionType = kTRANSACTIONTYPE_MaintainClaim Then
            'RETURN WITH 0 VALUE AS THE CALL IS FOR CLAIMS
            Return PMEReturnCode.PMTrue
        End If

        If nCurrentInsuranceFileKey = 0 AndAlso CurrentInsuranceFileKey = 0 Then
            Return PMEReturnCode.PMFalse
        End If

        nCurrentInsuranceFileKey = if(nCurrentInsuranceFileKey = 0, m_lInsuranceFileCnt, nCurrentInsuranceFileKey)
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddParameterLite(m_oDatabase, "nInsuranceFileKey", nCurrentInsuranceFileKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "nPreviousInsuranceFileKey", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=kGetPreviousInsuranceFileKeySQL,
                                sSQLName:=kGetPreviousInsuranceFileName,
                                bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to execute " + kGetPreviousInsuranceFileKeySQL)
        End If

        o_nPreviousInsuranceFileKey = m_oDatabase.Parameters.Item("nPreviousInsuranceFileKey").Value
        Return PMEReturnCode.PMTrue

    End Function
    ''' <summary>
    ''' GetClaimPaymentDeclineDetails
    ''' </summary>
    ''' <param name="o_bIsPaymentDeclined"></param>
    ''' <param name="o_nDeclinedClaimKey"></param>
    ''' <param name="o_nPreviousValidClaimKey"></param>
    ''' <param name="nClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClaimPaymentDeclineDetails(ByRef o_bIsPaymentDeclined? As Boolean, ByRef o_nDeclinedClaimKey As Integer,
                                                 ByRef o_nPreviousValidClaimKey As Integer, Optional ByVal nClaimKey As Integer = 0) As Integer
        nClaimKey = if(nClaimKey = 0, CurrentClaimKey, nClaimKey)
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddParameterLite(m_oDatabase, "nClaimKey", nClaimKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "bIsPaymentDeclined", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMBoolean)
        AddParameterLite(m_oDatabase, "nDeclinedClaimKey", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "nPreviousValidClaimKey", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
        ' Execute Action Query
        If m_oDatabase.SQLAction(sSQL:=kGetClaimPaymentDeclineDetailsSQL,
                                sSQLName:=kGetClaimPaymentDeclineDetailsName,
                                bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to execute " + kGetClaimPaymentDeclineDetailsSQL)
        End If

        o_bIsPaymentDeclined = m_oDatabase.Parameters.Item("bIsPaymentDeclined").Value
        o_nDeclinedClaimKey = m_oDatabase.Parameters.Item("nDeclinedClaimKey").Value
        o_nPreviousValidClaimKey = m_oDatabase.Parameters.Item("nPreviousValidClaimKey").Value

        Return PMEReturnCode.PMTrue

    End Function
    ''' <summary>
    ''' GetClassOfBusiness
    ''' </summary>
    ''' <param name="o_nCOBID"></param>
    ''' <param name="o_sCOBCode"></param>
    ''' <param name="nPerilTypeID"></param>
    ''' <param name="nClaimPerilID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClassOfBusiness(ByRef o_nCOBID As Integer, ByRef o_sCOBCode As String,
                                    ByVal nPerilTypeID As Integer, ByVal nClaimPerilID As Integer) As Integer

        ' Dim oResults As Object = Nothing
        Dim dsResult As DataSet = Nothing
        m_oDatabase.Parameters.Clear()
        AddParameterLite(m_oDatabase, "peril_type_id", nPerilTypeID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "claim_peril_id", nClaimPerilID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        If m_oDatabase.ExecuteDataSet(kGetClassOfBusinessSQL,
                           kGetClassOfBusinessName, True, dsResult) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("GetClassOfBusiness-ExecuteDataSet Failed.")
        End If

        If dsResult IsNot Nothing Then
            o_nCOBID = ToSafeInteger(dsResult.Tables(0).Rows(0).Item(0))
            o_sCOBCode = ToSafeString(dsResult.Tables(0).Rows(0).Item(1))
        End If

        dsResult = Nothing
        Return PMEReturnCode.PMTrue
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nPerilID"></param>
    ''' <param name="crTotalThisReserveTrans"></param>
    ''' <param name="sPerilTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PostReservesToOrion(ByVal nPerilID As Integer, ByVal crTotalThisReserveTrans As Decimal, ByVal sPerilTypeCode As String) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim nTransactionTypeID As Integer
        Dim sTransactionTypeCode As String = String.Empty
        Dim nDebitAccountID, nCreditAccountID As Integer
        Dim sDebitAccountCode, sCreditAccountCode As String
        Dim nStatsFolderCnt As Integer
        Dim sCOBCode As String = String.Empty
        Dim nCOBID As Integer
        Dim oClaimTrans As bControlTransClaims.Automated = Nothing

        If ClearStatsDetails(sPerilTypeCode:=sPerilTypeCode, nLossID:=CurrentClaimKey, bDeleteStatsFolder:=True) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("PostReservesToOrion-ClearStatsDetails Failed.")
        End If

        If crTotalThisReserveTrans = 0 Then
            Return PMEReturnCode.PMTrue
        End If

        If gPMComponentServices.CreateBusinessObject(r_oObject:=oClaimTrans, v_sClassName:="bControlTransClaims.Automated",
                             v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID,
                             v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID,
                             v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("PostReservesToOrion-CreateStatsFolder Failed.")
        End If

        If GetClassOfBusiness(o_nCOBID:=nCOBID, o_sCOBCode:=sCOBCode, nClaimPerilID:=nPerilID, nPerilTypeID:=0) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("PostReservesToOrion-GetClassOfBusiness Failed.")
        End If

        'initialising debit/credit account
        sDebitAccountCode = "CLMEXP" & sCOBCode.Trim()
        sCreditAccountCode = "CLMRES" & sCOBCode.Trim()

        Select Case m_sTransactionType
            Case kTRANSACTIONTYPE_OpenClaim
                nTransactionTypeID = 26
                sTransactionTypeCode = kTRANSACTIONTYPE_OpenClaim
            Case kTRANSACTIONTYPE_MaintainClaim, kTRANSACTIONTYPE_PayClaim
                nTransactionTypeID = 28
                sTransactionTypeCode = kTRANSACTIONTYPE_MaintainClaim
        End Select

        oClaimTrans.DebitAccountID = nDebitAccountID 'claim expense
        oClaimTrans.CreditAccountID = nCreditAccountID 'claim reserve
        oClaimTrans.TransactionTypeID = nTransactionTypeID
        oClaimTrans.TransactionTypeCode = sTransactionTypeCode
        oClaimTrans.DocumentTypeID = 35 'Transferred Debit
        oClaimTrans.InsuranceFileCnt = CurrentInsuranceFileKey
        oClaimTrans.ClaimID = CurrentClaimKey
        oClaimTrans.PerilID = nPerilID
        oClaimTrans.DebitCredit = "D"
        oClaimTrans.DocumentComment = "Reserve for claim number " & CurrentClaimKey
        oClaimTrans.TransactionAmount = crTotalThisReserveTrans

        oClaimTrans.DebitAccountID = nDebitAccountID 'claim expense
        oClaimTrans.CreditAccountID = nCreditAccountID 'claim reserve
        oClaimTrans.TransactionTypeID = nTransactionTypeID
        oClaimTrans.TransactionTypeCode = sTransactionTypeCode
        oClaimTrans.DocumentTypeID = 35 'Transferred Debit
        oClaimTrans.InsuranceFileCnt = CurrentInsuranceFileKey
        oClaimTrans.ClaimID = CurrentClaimKey
        oClaimTrans.PerilID = nPerilID
        oClaimTrans.DebitCredit = "D"
        oClaimTrans.DocumentComment = "Reserve for claim number " & CurrentClaimKey
        oClaimTrans.TransactionAmount = crTotalThisReserveTrans

        nReturn = oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=sTransactionTypeCode)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("PostReservesToOrion-CreateStatsFolder Failed.")
        End If

        nReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=nStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=nCOBID, v_sClassOfBusCode:=sCOBCode, v_lRIPartyCnt:=nCreditAccountID, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("PostReservesToOrion-CreateStatsDetails Failed.")
        End If

        oClaimTrans.Dispose()
        oClaimTrans = Nothing

        Return nReturn
    End Function
    ''' <summary>
    ''' use to Prepare safe SQL string.
    ''' </summary>
    ''' <param name="sSQLString"></param>
    ''' <returns></returns>
    Private Function GetSQLSafeString(ByVal sSQLString) As String
        Const kSpace As String = " "
        sSQLString = sSQLString.Replace("CREATE" & kSpace, String.Empty).Replace("ALTER" & kSpace, String.Empty).Replace("DROP" & kSpace, String.Empty)
        sSQLString = sSQLString.Replace("SELECT" & kSpace, String.Empty).Replace("UPDATE" & kSpace, String.Empty).Replace("DELETE" & kSpace, String.Empty)
        sSQLString = sSQLString.Replace("WHERE" & kSpace, String.Empty)
        Return sSQLString
    End Function

    ''' <summary>
    ''' SavePaymentToAccounts
    ''' </summary>
    ''' <param name="nClaimPaymentID"></param>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="sPayeeShortName"></param>
    ''' <param name="crThispayment"></param>
    ''' <param name="crTaxAmount"></param>
    ''' <param name="sPerilTypeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SavePaymentToAccounts(ByVal nClaimPaymentID As Integer, ByVal nClaimPerilID As Integer, ByVal sPayeeShortName As String, _
                                           ByVal crThispayment As Decimal, ByVal crTaxAmount As Decimal, ByVal sPerilTypeCode As String) As Integer
        Const kMethodName As String = "SavePaymentToAccounts"

        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim crTotalPaymentInPaymentCurrency As Decimal = 0
        Dim sCOBCode As String
        Dim nCOBID As Integer
        Dim nPayeeID As Integer
        Dim oData As Object
        Dim oCLMPeril As bCLMPeril.Business

        If IsPostTaxes Then
            crTotalPaymentInPaymentCurrency = crThispayment
        Else
            ' calculate the total amount
            crTotalPaymentInPaymentCurrency = crThispayment + crTaxAmount
        End If

        If GetClassOfBusiness(o_nCOBID:=nCOBID, o_sCOBCode:=sCOBCode, nClaimPerilID:=nClaimPerilID, nPerilTypeID:=0) <> PMEReturnCode.PMTrue OrElse sCOBCode = String.Empty Then
            Throw New ApplicationException("Failed to Execute GetClassOfBusiness")
        End If

        If sPayeeShortName <> String.Empty Then
            'get party cnt
            If GetTableData(oData, "Party", "party_cnt", "shortname='" & sPayeeShortName & "'") <> PMEReturnCode.PMTrue OrElse Not Informations.IsArray(oData) Then
                Throw New ApplicationException("SavePaymentToAccounts-GetTableData Failed.")
            End If

            nPayeeID = ToSafeInteger(oData(0, 0))
        Else
            sPayeeShortName = "CLMPAYABLE"
            nPayeeID = 0
        End If

        ' initialise CLMPeril
        oCLMPeril = New bCLMPeril.Business
        If oCLMPeril.Initialise(sUsername:=m_sUsername, _
                                   sPassword:=m_sPassword, _
                                   iUserID:=m_iUserID, _
                                   iSourceID:=m_iSourceID, _
                                   iLanguageID:=m_iLanguageID, _
                                   iCurrencyID:=m_iCurrencyID, _
                                   iLogLevel:=m_iLogLevel, _
                                   sCallingAppName:=m_sCallingAppName, _
                                   vDatabase:=m_oDatabase) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("oCLMPeril.Initialise failed.")
        End If

        If ClearStatsDetails(sPerilTypeCode:=sPerilTypeCode, nPaymentID:=nClaimPaymentID) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("SavePaymentToAccounts-ClearStatsDetails Failed.")
        End If

        'post(payment And taxes)
        If oCLMPeril.PostPaymentToOrion(v_lClaimPaymentId:=nClaimPaymentID,
                                                 v_sClaimNumber:=CurrentClaimNumber,
                                                 v_lInsuranceFileCnt:=CInt(m_lInsuranceFileCnt),
                                                 v_lClaimId:=CurrentClaimKey,
                                                 v_lClaimPerilId:=nClaimPerilID,
                                                 v_cPayAmount:=crTotalPaymentInPaymentCurrency,
                                                 v_sCreditAccountCode:=sPayeeShortName,
                                                 v_sCOBCode:=sCOBCode,
                                                 v_lCOBId:=nCOBID,
                                                 v_bPostClaimTax:=IsPostTaxes,
                                                 v_lPartyCnt:=nPayeeID) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("SavePaymentToAccounts-PostPaymentToOrion Failed.")
        End If

        oCLMPeril.Dispose()
        oCLMPeril = Nothing
        oData = Nothing

        Return nReturn
    End Function
    ''' <summary>
    ''' GetBasicClaimInformation
    ''' </summary>
    ''' <param name="nClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBasicClaimInformation(Optional ByVal nClaimKey As Integer = 0) As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Dim dsResult As DataSet = Nothing
        nClaimKey = if(nClaimKey = 0, CurrentClaimKey, nClaimKey)
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        AddParameterLite(m_oDatabase, "nClaimKey", nClaimKey, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)

        If m_oDatabase.ExecuteDataSet(kGetBasicClaimInformationSQL, kGetBasicClaimInformationName, True, dsResult) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to Execute " + kGetBasicClaimInformationSQL)
        End If

        If dsResult IsNot Nothing Then
            m_sCurrentClaimNumber = ToSafeString(dsResult.Tables(0).Rows(0).Item(0))
            m_bIsPostTaxes = (ToSafeInteger(dsResult.Tables(0).Rows(0).Item(1)) = 1)
            m_bIsGrossClaimPayment = (ToSafeInteger(dsResult.Tables(0).Rows(0).Item(2)) = 1)
            m_lPartyCnt = ToSafeInteger(dsResult.Tables(0).Rows(0).Item(3))
            m_lInsuranceFileCnt = ToSafeInteger(dsResult.Tables(0).Rows(0).Item(4))
            m_lInsuranceFolderCnt = ToSafeInteger(dsResult.Tables(0).Rows(0).Item(5))
        End If

        Return nReturn

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sPerilTypeCode"></param>
    ''' <param name="nPaymentID"></param>
    ''' <param name="nLossID"></param>
    ''' <param name="bDeleteStatsFolder"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClearStatsDetails(ByVal sPerilTypeCode As String, Optional ByVal nPaymentID As Integer = 0, _
                                       Optional ByVal nLossID As Integer = 0, Optional ByVal bDeleteStatsFolder As Boolean = False) As Integer

        Dim nReturn As Integer = PMEReturnCode.PMTrue

        AddParameterLite(m_oDatabase, "nPaymentID", nPaymentID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
        AddParameterLite(m_oDatabase, "nLossID", nLossID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "sPerilTypeCode", sPerilTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "bDeleteStatsFolder", bDeleteStatsFolder, PMEParameterDirection.PMParamInput, PMEDataType.PMBoolean)

        If m_oDatabase.SQLAction( _
                    sSQL:=kClearStatsDetailSQL, _
                    sSQLName:=kClearStatsDetailName, _
                    bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Failed to Execute " + kClearStatsDetailSQL)
        End If

        Return nReturn
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nReserveID"></param>
    ''' <param name="r_crPreviousReserve"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPreviousReserve(ByVal nReserveID As Integer, ByRef r_crPreviousReserve As Double) As Integer

        Const kMethodName As String = "GetPreviousReserve"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()

            AddParameterLite(m_oDatabase, "nReserveID", nReserveID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "crPreviousReserve", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)

            If m_oDatabase.SQLAction( _
                            sSQL:=kGetPreviousReserveSQL, _
                            sSQLName:=kGetPreviousReserveName, _
                            bStoredProcedure:=True) <> PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Failed to Execute " + kGetPreviousReserveSQL)
            End If

            r_crPreviousReserve = ToSafeDecimal(m_oDatabase.Parameters.Item("crPreviousReserve").Value)

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, _
                               iType:=PMELogLevel.PMLogError, _
                               sMsg:="Method Failed!", vClass:=ACClass, _
                               vMethod:=kMethodName, excep:=ex)
            nReturn = PMEReturnCode.PMFalse
        End Try
        Return nReturn
    End Function

#End Region
End Class
