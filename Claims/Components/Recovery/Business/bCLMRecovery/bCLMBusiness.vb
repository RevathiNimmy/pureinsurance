Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24/08/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMRecovery.
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"


    ' ************************************************
    ' Added to replace global variables 19/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Collection of CLMRecoverys
    Private m_oRecoverys As bCLMRecovery.CLMRecoverys

    ' Database Class
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date




    ' ***************************************************************** '
    '                       PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public Property CurrentRecord() As Integer
        Get
            Return m_lCurrentRecord
        End Get
        Set(ByVal Value As Integer)
            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oRecoverys.Count()
                    m_lCurrentRecord = m_oRecoverys.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select
        End Set
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oRecoverys.Count()
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property



    ' ***************************************************************** '
    '                       PUBLIC FUNCTIONS
    ' ***************************************************************** '

    ' Add payment details
    Public Function AddReceiptAndPayments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddReceiptAndPayments"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we haven't already started a transaction start one.
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans()", "Failed to start transaction")
            End If
            bTransStarted = True

            ' Add receipts
            lReturn = CType(AddReceiptDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddReceiptDetails", "Failed to add receipt details")
            End If

            ' Add payments
            lReturn = CType(AddPaymentDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddPaymentDetails", "Failed to add payment details")
            End If

            ' If we're in a transaction commit it.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans()", "Failed to commit transaction")
                End If
                bTransStarted = False
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If we're in a transaction roll it back.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

        Finally
            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    ' Balances the recoveries on the specified claim
    Public Function BalanceRecovery(Optional ByRef vClaimId As Object = Nothing, Optional ByRef vIsSalvage As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BalanceRecovery"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", vClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", Math.Abs(CInt(gPMFunctions.ToSafeBoolean(vIsSalvage))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLAction(sSQL:=ACBalanceRecoverySQL, sSQLName:=ACBalanceRecoveryName, bStoredProcedure:=ACBalanceRecoveryStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to balance recovery details")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    ' Checks the Collection to see if Cancel is OK.
    Public Function CheckCancel() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For Each oRecovery As CLMRecovery In m_oRecoverys
                ' If any record is marked changed return as such
                If oRecovery.IsNew Or oRecovery.IsDirty Or oRecovery.IsDeleted Then
                    Return gPMConstants.PMEReturnCode.PMDataChanged
                End If
            Next oRecovery

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' We need to be able to close a claim from salvage/recovery.
    Public Function CloseClaim(ByVal v_lClaimID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimID", v_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCloseClaimSQL, sSQLName:=ACCloseClaimName, bStoredProcedure:=ACCloseClaimStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return gPMConstants.PMEReturnCode.PMFalse

        End Try
    End Function


    ' Add payment details
    Public Function DeleteReceiptAndPayments() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteReceiptAndPayments"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we haven't already started a transaction start one.
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans()", "Failed to start transaction")
            End If
            bTransStarted = True

            ' Add receipts
            lReturn = CType(DeleteReceiptDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteReceiptDetails", "Failed to delete receipt details")
            End If

            ' Add payments
            lReturn = CType(DeletePaymentDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeletePaymentDetails", "Failed to delete payment details")
            End If

            ' If we're in a transaction commit it.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans()", "Failed to commit transaction")
                End If
                bTransStarted = False
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If we're in a transaction roll it back.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

        Finally
            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    ' Delete  claim records
    Public Function DeleteClaim(ByVal v_lClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "status", lStatus, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check status
            lStatus = m_oDatabase.Parameters.Item("status").Value
            If lStatus <> 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Creates a new CLMRecovery and adds it into the Collection.
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function EditAdd(ByVal vClaimId As Integer, ByVal vPerilId As Integer, ByVal vRecoveryType As String, ByVal vRecoveryTypeID As Integer, ByVal vLossCurrency As String, ByVal vLossCurrencyID As Integer, ByVal vInitialReserve As Decimal, ByRef rUniqueId As String, Optional ByVal v_lRecoveryPartyTypeId As Integer = 0, Optional ByVal v_lRecoveryPartyCnt As Integer = 0, Optional ByVal v_sRecoveryParty As String = "", Optional ByVal v_sRecoveryPartyDesc As String = "") As Integer
        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

        Dim result As Integer = 0
        Dim oCLMRecovery As bCLMRecovery.CLMRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CLMRecovery
            oCLMRecovery = New bCLMRecovery.CLMRecovery()
            m_lReturn = CType(oCLMRecovery.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Mark record as new and edited
            oCLMRecovery.IsNew = True
            oCLMRecovery.IsDirty = True

            ' Populate attributes
            oCLMRecovery.ClaimID = vClaimId
            oCLMRecovery.PerilID = vPerilId
            oCLMRecovery.RecoveryType = vRecoveryType
            oCLMRecovery.RecoveryTypeID = vRecoveryTypeID
            oCLMRecovery.LossCurrency = vLossCurrency
            oCLMRecovery.LossCurrencyID = CStr(vLossCurrencyID)
            oCLMRecovery.InitialReserve = vInitialReserve
            oCLMRecovery.RevisionCount = 0
            oCLMRecovery.CurrencyRate = 1
            'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
            oCLMRecovery.lRecoveryPartyTypeId = v_lRecoveryPartyTypeId
            oCLMRecovery.lRecoveryPartyCnt = v_lRecoveryPartyCnt
            oCLMRecovery.sRecoveryParty = v_sRecoveryParty
            oCLMRecovery.sRecoveryPartyDesc = v_sRecoveryPartyDesc
            'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

            ' Add to collection
            m_lReturn = CType(m_oRecoverys.Add(oNewCLMRecovery:=oCLMRecovery), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return false
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Set the assigned unique id
                rUniqueId = oCLMRecovery.UniqueId
            End If

            oCLMRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Validate that the specified CLMRecovery can be deleted and delete
    Public Function EditDelete(ByVal vUniqueId As String) As Integer

        Dim result As Integer = 0
        Dim oCLMRecovery As bCLMRecovery.CLMRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Even new items will have a temporary ID
            oCLMRecovery = m_oRecoverys.Item(vUniqueId)

            ' If the item is new we can delete it, otherwise we can't
            If oCLMRecovery.IsNew Then
                ' Just remove from the collection, we don't need it anymore
                m_oRecoverys.Remove(vUniqueId)
            Else
                ' If we haven't received any payments mark for deletion
                If oCLMRecovery.ReceivedToDate = 0 Then
                    oCLMRecovery.IsDeleted = True
                Else
                    ' The item has had receipts processed so we can't delete it
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Release reference to CLMRecovery
            oCLMRecovery = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Updates the CLMRecovery with the new values.
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function EditUpdate(ByVal vUniqueId As String, Optional ByVal vRecoveryId As Object = Nothing, Optional ByVal vThisReserve As Decimal = 0, Optional ByVal vThisReceipt As Decimal = 0, Optional ByVal vTaxType As String = "", Optional ByVal vTaxTypeID As Integer = 0, Optional ByVal vTaxTypeCode As String = "", Optional ByVal vTaxBand As String = "", Optional ByVal vTaxBandID As Integer = 0, Optional ByVal vTaxAmount As Decimal = 0, Optional ByVal v_lRecoveryPartyTypeId As Integer = 0, Optional ByVal v_lRecoveryPartyCnt As Integer = 0, Optional ByVal v_sRecoveryParty As String = "", Optional ByVal v_sRecoveryPartyDesc As String = "") As Integer '
        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
        Dim result As Integer = 0
        Dim oRecovery As CLMRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Validate that the row specified is valid.
            oRecovery = m_oRecoverys.Item(vUniqueId)
            If oRecovery Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Check the Status of the CLMRecovery
            If oRecovery.IsDeleted Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set dirty
            oRecovery.IsDirty = True

            ' Set properties
            'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
            If v_lRecoveryPartyTypeId >= 0 Then oRecovery.lRecoveryPartyTypeId = v_lRecoveryPartyTypeId
            If v_lRecoveryPartyCnt >= 0 Then oRecovery.lRecoveryPartyCnt = v_lRecoveryPartyCnt

            If Not String.IsNullOrEmpty(v_sRecoveryParty) Then oRecovery.sRecoveryParty = v_sRecoveryParty

            If Not String.IsNullOrEmpty(v_sRecoveryPartyDesc) Then oRecovery.sRecoveryPartyDesc = v_sRecoveryPartyDesc
            'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

            If Not Information.IsNothing(vThisReserve) Then oRecovery.ThisReserve = vThisReserve

            If Not Information.IsNothing(vThisReceipt) Then oRecovery.ThisReceipt = vThisReceipt

            If Not Information.IsNothing(vTaxType) Then oRecovery.TaxType = vTaxType

            If Not Information.IsNothing(vTaxTypeID) Then oRecovery.TaxTypeID = vTaxTypeID

            If Not Information.IsNothing(vTaxTypeCode) Then oRecovery.TaxTypeCode = vTaxTypeCode

            If Not Information.IsNothing(vTaxBand) Then oRecovery.TaxBand = vTaxBand

            If Not Information.IsNothing(vTaxBandID) Then oRecovery.TaxBandID = vTaxBandID

            If Not Information.IsNothing(vTaxAmount) Then oRecovery.TaxAmount = vTaxAmount
            oRecovery.RevisionCount += 1

            ' Recalculate reinsurance splits
            oRecovery.RecalcCoReinsurance()

            ' Release reference to CLMRecovery
            oRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Get client and agent details for this claim
    Public Function GetClientAgentID(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute call

            Return m_oDatabase.SQLSelect(sSQL:=ACGetClientAgentSQL, sSQLName:=ACGetClientAgentName, bStoredProcedure:=ACGetClientAgentStored, vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientAgentID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAgentID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function


    ' Gets the required CLMRecoverys and populate the Collection
    Public Function GetDetails(Optional ByRef vPerilId As Object = Nothing, Optional ByRef vIsSalvage As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim lInner As Integer

        Dim oRecovery As bCLMRecovery.CLMRecovery

        ' Array of coinsurance and reinsurance details
        Dim vCoinsurance, vReinsurance(,) As Object

        ' Array of individual coinsurance and reinsurance details
        Dim vICoinsurance, vIReinsurance As Object

        Const kMethodName As String = "GetDetails"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the collection and current record
            m_oRecoverys.Clear()
            m_lCurrentRecord = 0

            ' Get coinsurance details


            'developer guide no.98
            lReturn = GetCoinsuranceRecoveries(r_vResultArray:=vCoinsurance, v_lPerilID:=vPerilId, v_bIsSalvage:=vIsSalvage)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError("GetCoinsuranceRecoveries()", "Failed to get coinsurance details")
            End If

            ' Get reinsurance details


            'developer guide no.98
            lReturn = GetReinsuranceRecoveries(r_vResultArray:=vReinsurance, v_lPerilID:=vPerilId, v_bIsSalvage:=vIsSalvage)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError("GetReinsuranceRecoveries()", "Failed to get reinsurance details")
            End If

            ' Prep the individual arrays
            ReDim vICoinsurance(ACCIMAX, 0)
            ReDim vIReinsurance(ACRIMAX, 0)

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "peril_id", vPerilId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", Math.Abs(CInt(gPMFunctions.ToSafeBoolean(vIsSalvage))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRecoverySQL, sSQLName:=ACGetRecoveryName, bStoredProcedure:=ACGetRecoveryStored, vResultArray:=vResultArray, lNumberRecords:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to get recovery details")
            End If

            ' Do we have any records ?
            If Not Information.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            For lCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                ' Yes, populate the objects
                oRecovery = New bCLMRecovery.CLMRecovery()

                lReturn = CType(oRecovery.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set properties

                oRecovery.UniqueId = "r" & CStr(vResultArray(ACRRecoveryID, lCount))

                oRecovery.RecoveryId = CInt(vResultArray(ACRRecoveryID, lCount))

                oRecovery.ClaimID = CInt(vResultArray(ACRClaimID, lCount))

                oRecovery.PerilID = CInt(vResultArray(ACRPerilID, lCount))

                oRecovery.RecoveryTypeID = CInt(vResultArray(ACRRecoveryTypeID, lCount))

                oRecovery.RecoveryType = CStr(vResultArray(ACRRecoveryType, lCount))

                oRecovery.LossCurrencyID = CStr(vResultArray(ACRCurrencyID, lCount))

                oRecovery.LossCurrency = CStr(vResultArray(ACRCurrency, lCount))

                oRecovery.InitialReserve = CDec(vResultArray(ACRInitialReserve, lCount))

                oRecovery.RevisedReserve = CDec(vResultArray(ACRRevisedReserve, lCount))

                oRecovery.ReceivedToDate = CDec(vResultArray(ACRReceivedToDate, lCount))

                oRecovery.RevisionCount = CInt(vResultArray(ACRRevisionCount, lCount))

                oRecovery.TaxToDate = CDec(vResultArray(ACRTaxToDate, lCount))

                oRecovery.IsPostTaxes = CBool(vResultArray(ACRIsPostTaxes, lCount))

                'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

                If CStr(vResultArray(ACRRecoveryPartyTypeID, lCount)) = "" Then
                    oRecovery.lRecoveryPartyTypeId = 0
                Else

                    oRecovery.lRecoveryPartyTypeId = CInt(vResultArray(ACRRecoveryPartyTypeID, lCount))
                End If


                If CStr(vResultArray(ACRRecoveryPartyTypeCnt, lCount)) = "" Then
                    oRecovery.lRecoveryPartyCnt = 0
                Else

                    oRecovery.lRecoveryPartyCnt = CInt(vResultArray(ACRRecoveryPartyTypeCnt, lCount))
                End If


                oRecovery.sRecoveryParty = CStr(vResultArray(ACRRecoveryParty, lCount)).Trim()

                oRecovery.sRecoveryPartyDesc = CStr(vResultArray(ACRRecoveryPartyDesc, lCount)).Trim()

                'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

                ' Default receipt currency to loss currency

                oRecovery.ReceiptCurrencyID = CInt(vResultArray(ACRCurrencyID, lCount))

                oRecovery.ReceiptCurrency = CStr(vResultArray(ACRCurrency, lCount))
                oRecovery.CurrencyRate = 1

                ' Set the post taxes flag on the collection

                m_oRecoverys.IsPostTaxes = CBool(vResultArray(ACRIsPostTaxes, lCount))

                ' Add coinsurance
                If Information.IsArray(vCoinsurance) Then
                    lInner = 0

                    ' Walk the next portion of the array
                    ' Note: We are starting where we left off last time, the source array
                    ' and recovery array MUST be sorted by recovery id for this to work.

                    For lCoins As Integer = lCoins To vCoinsurance.GetUpperBound(1)
                        ' As long as we are on the right recovery id copy the rows

                        If CDbl(vCoinsurance(ACCIRecoveryID, lCoins)) = oRecovery.RecoveryId Then
                            ReDim Preserve vICoinsurance(ACCIMAX, lInner)

                            ' Copy this row


                            vICoinsurance(ACCIRecoveryID, lInner) = vCoinsurance(ACCIRecoveryID, lCoins)


                            vICoinsurance(ACCIPartyCnt, lInner) = vCoinsurance(ACCIPartyCnt, lCoins)


                            vICoinsurance(ACCIDescription, lInner) = vCoinsurance(ACCIDescription, lCoins)


                            vICoinsurance(ACCISharePercent, lInner) = vCoinsurance(ACCISharePercent, lCoins)

                            vICoinsurance(ACCIPaidToDate, lInner) = gPMFunctions.ToSafeCurrency(vCoinsurance(ACCIPaidToDate, lCoins))

                            vICoinsurance(ACCIIsTaxShared, lInner) = gPMFunctions.ToSafeBoolean(vCoinsurance(ACCIIsTaxShared, lCoins))

                            vICoinsurance(ACCIThisPayment, lInner) = 0

                            vICoinsurance(ACCIThisPaymentLoss, lInner) = 0

                            vICoinsurance(ACCITaxAmount, lInner) = 0

                            vICoinsurance(ACCITaxAmountLoss, lInner) = 0

                            ' Increment the counter
                            lInner += 1
                        Else
                            Exit For
                        End If
                    Next lCoins

                    ' Attach to the recovery object

                    oRecovery.Coinsurance = vICoinsurance
                Else
                    oRecovery.Coinsurance = VB6.CopyArray(Nothing)
                End If

                ' Add reinsurance
                If Information.IsArray(vReinsurance) Then
                    lInner = 0

                    ' Walk the next portion of the array
                    ' Note: We are starting where we left off last time, the source array
                    ' and recovery array MUST be sorted by recovery id for this to work.

                    For lReins As Integer = lReins To vReinsurance.GetUpperBound(1)
                        ' As long as we are on the right recovery id copy the rows

                        If CDbl(vReinsurance(ACRIRecoveryID, lReins)) = oRecovery.RecoveryId Then
                            ReDim Preserve vIReinsurance(ACRIMAX, lInner)

                            ' Copy this row


                            vIReinsurance(ACRIRecoveryID, lInner) = vReinsurance(ACRIRecoveryID, lReins)


                            vIReinsurance(ACRIArrangmentLineID, lInner) = vReinsurance(ACRIArrangmentLineID, lReins)


                            vIReinsurance(ACRITreatyID, lInner) = vReinsurance(ACRITreatyID, lReins)


                            vIReinsurance(ACRIFACPartyCnt, lInner) = vReinsurance(ACRIFACPartyCnt, lReins)


                            vIReinsurance(ACRIDescription, lInner) = vReinsurance(ACRIDescription, lReins)


                            vIReinsurance(ACRISharePercent, lInner) = vReinsurance(ACRISharePercent, lReins)

                            vIReinsurance(ACRIIsTaxShared, lInner) = gPMFunctions.ToSafeBoolean(vReinsurance(ACRIIsTaxShared, lReins))

                            If CBool(vIsSalvage) Then

                                vIReinsurance(ACRIPaidToDate, lInner) = gPMFunctions.ToSafeCurrency(vReinsurance(9, lReins)) 'salvage

                                vIReinsurance(ACRIThisPayment, lInner) = gPMFunctions.ToSafeCurrency(vReinsurance(10, lReins)) 'this_ salvage
                            Else

                                vIReinsurance(ACRIPaidToDate, lInner) = gPMFunctions.ToSafeCurrency(vReinsurance(11, lReins)) 'recovery

                                vIReinsurance(ACRIThisPayment, lInner) = gPMFunctions.ToSafeCurrency(vReinsurance(12, lReins)) 'this_ recovery
                            End If

                            vIReinsurance(ACRIThisPaymentLoss, lInner) = 0

                            vIReinsurance(ACRITaxAmount, lInner) = 0

                            vIReinsurance(ACRITaxAmountLoss, lInner) = 0

                            ' Increment the counter
                            lInner += 1
                        Else
                            Exit For
                        End If
                    Next lReins

                    ' Attach to the recovery object

                    oRecovery.Reinsurance = vIReinsurance
                Else
                    oRecovery.Reinsurance = VB6.CopyArray(Nothing)
                End If

                ' Add to the collection
                lReturn = CType(m_oRecoverys.Add(oRecovery), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oRecoverys.Add()", "Failed to add recovery details to collection")
                End If

                ' Release variable
                oRecovery = Nothing
            Next lCount

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' Gets the total outstanding reserve and recovery reserve values for the supplied claim
    Public Function GetCurrentReserveRecovery(ByVal v_lClaimID As Integer, ByRef r_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurrentReserveRecovery"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimID", v_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute the stored procedure.
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrentReserveRecoverySQL, sSQLName:=ACGetCurrentReserveRecoveryName, bStoredProcedure:=ACGetCurrentReserveRecoveryStored, vResultArray:=r_vDataArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Failed to get reserve and recovery information")
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' Gets data from the next (or a specific) record
    Public Function GetNext(Optional ByRef vGetUniqueID As String = "", Optional ByRef vUniqueId As String = "", Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vRecoveryType As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vInitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vThisReserve As Object = Nothing, Optional ByRef vBalance As Object = Nothing, Optional ByRef vReceiptCurrency As Object = Nothing, Optional ByRef vReceiptCurrencyID As Object = Nothing, Optional ByRef vCurrencyRate As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vThisReceipt As Object = Nothing, Optional ByRef vTaxType As Object = Nothing, Optional ByRef vTaxTypeID As Object = Nothing, Optional ByRef vTaxBand As Object = Nothing, Optional ByRef vTaxBandID As Object = Nothing, Optional ByRef vTaxAmount As Object = Nothing, Optional ByRef vCoinsurance As Object = Nothing, Optional ByRef vReinsurance As Object = Nothing, Optional ByRef vIsPostTaxes As Object = Nothing, Optional ByRef lRecoveryPartyTypeId As Object = Nothing, Optional ByRef lRecoveryPartyCnt As Object = Nothing, Optional ByRef sRecoveryParty As Object = Nothing, Optional ByRef sRecoveryPartyDesc As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oRecovery As bCLMRecovery.CLMRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If vGetUniqueID.Length = 0 And m_oRecoverys.Count() = 0 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else

                ' If we have been asked for a specific record get it
                If vGetUniqueID.Length Then
                    oRecovery = m_oRecoverys.Item(vGetUniqueID)
                Else

                    ' Check to see that we are not at the end of the Collection
                    If m_lCurrentRecord < m_oRecoverys.Count() Then
                        ' Increment current record pointer
                        m_lCurrentRecord += 1
                    Else
                        result = gPMConstants.PMEReturnCode.PMEOF
                    End If

                    oRecovery = m_oRecoverys.Item(m_lCurrentRecord)
                End If

                ' Set return properties
                vUniqueId = oRecovery.UniqueId

                vRecoveryId = oRecovery.RecoveryId

                vRevisionCount = oRecovery.RevisionCount
                vRecoveryType = oRecovery.RecoveryType

                vRecoveryTypeID = oRecovery.RecoveryTypeID
                vInitialReserve = oRecovery.InitialReserve
                vRevisedReserve = oRecovery.RevisedReserve
                vThisReserve = oRecovery.ThisReserve
                vReceiptCurrency = oRecovery.ReceiptCurrency

                vReceiptCurrencyID = oRecovery.ReceiptCurrencyID
                vCurrencyRate = oRecovery.CurrencyRate
                vReceivedToDate = oRecovery.ReceivedToDate
                vThisReceipt = oRecovery.ThisReceipt
                vBalance = oRecovery.Balance
                vTaxType = oRecovery.TaxType
                vTaxTypeID = oRecovery.TaxTypeID
                vTaxBand = oRecovery.TaxBand
                vTaxBandID = oRecovery.TaxBandID
                vTaxAmount = oRecovery.TaxAmount
                vCoinsurance = VB6.CopyArray(oRecovery.Coinsurance)
                vReinsurance = VB6.CopyArray(oRecovery.Reinsurance)
                vIsPostTaxes = oRecovery.IsPostTaxes
                'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                lRecoveryPartyCnt = oRecovery.lRecoveryPartyCnt
                lRecoveryPartyTypeId = oRecovery.lRecoveryPartyTypeId
                sRecoveryParty = oRecovery.sRecoveryParty
                sRecoveryPartyDesc = oRecovery.sRecoveryPartyDesc
                'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
                oRecovery = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Get the original claim ID from  table
    Public Function GetOriginalClaimID(ByVal v_lClaimID As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute sql
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalClaimIDSQL, sSQLName:=ACGetOriginalClaimIDName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lOriginalClaimID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Gets the GetPerilDetails for the given Claim Id
    Public Function GetPerilDetails(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_id", v_lClaim_Id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPerilDetailsSQL, sSQLName:=ACGetPerilDetailsName, bStoredProcedure:=ACGetPerilDetailsStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Return the current receipt total
    Public Function GetReceiptTotal(ByRef r_cReceiptTotal As Decimal) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get receipt total from recovery collection, if we have one
        If Not (m_oRecoverys Is Nothing) Then
            r_cReceiptTotal = m_oRecoverys.ThisReceiptTotal
        Else
            r_cReceiptTotal = 0
        End If
        Return result
    End Function
    ' Get recovery types
    Public Function GetRecoveryTypes(ByVal v_bIsSalvage As Boolean, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", Math.Abs(CInt(v_bIsSalvage)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRecoveryTypesSQL, sSQLName:=ACGetRecoveryTypesName, bStoredProcedure:=ACGetRecoveryTypesStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecoveryTypes")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If NO record was found return Not Found
            If Not Information.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRecoveryTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecoveryTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Returns list of tax types and associated tax bands
    Public Function GetTaxTypesTaxBands(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Get records
            result = m_oDatabase.SQLSelect(sSQL:=ACGetTaxTypesBandsSQL, sSQLName:=ACGetTaxTypesBandsName, bStoredProcedure:=ACGetTaxTypesBandsStored, vResultArray:=r_vResultArray, bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Following stored procedure failed to run:" & ACGetTaxTypesBandsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxTypesTaxBands", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            If Not Information.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaxTypesTaxBands Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxTypesTaxBands", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Entry point for any initialisation code for this object.
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create CLMRecoverys Collection
            m_oRecoverys = New bCLMRecovery.CLMRecoverys()
            ' Initialise object
            m_lReturn = CType(m_oRecoverys.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oNewCLMRecovery.Initialise", "Failed to initialise recovery object")
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Post receipts payments and stats.
    Public Function PostReceipt(ByVal bIsSalvage As Integer, ByRef lInsuranceFileCnt As Integer, ByRef lClaimID As Integer, ByRef lPerilID As Integer, ByRef lReceiptPartyCnt As Integer, ByRef sAccountCode As String, ByRef sMappingCode As String, ByRef sReceiptComments As String, ByRef lCOBID As Integer, ByRef sCOBCode As String) As Integer

        Dim result As Integer = 0
        Dim bTransStarted As Boolean
        Dim oRecovery As CLMRecovery

        Const kMethodName As String = "PostReceipt"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Don't do anything if we have no receipt
            If m_oRecoverys.ThisReceiptTotal = 0 Then
                Return result
            End If

            ' If we haven't already started a transaction start one.
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans()", "Failed to start transaction")
            End If
            bTransStarted = True

            ' Add receipts
            lReturn = CType(UpdateReceiptDetails(lReceiptPartyCnt, sReceiptComments), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateReceiptDetails", "Failed to update receipt details")
            End If

            ' Add reinsurance details
            lReturn = CType(AddReinsuranceDetails(lPerilID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("AddReinsuranceDetails", "Failed to add reinsurance details")
            End If

            ' Post details to orion
            lReturn = CType(PostToOrion(bIsSalvage, lInsuranceFileCnt, lClaimID, lPerilID, lReceiptPartyCnt, sAccountCode, sMappingCode, lCOBID, sCOBCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PostToOrion", "Failed to post details to orion")
            End If

            ' If we're in a transaction commit it.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans()", "Failed to commit transaction")
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Update(), excep:=ex)

            ' If we're in a transaction roll it back.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

        Finally
        End Try
        Return result
    End Function


    ' Set the optional process modes.
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' Set the receipt currency on all recovery objects
    Public Function SetReceiptCurrency(ByVal vReceiptCurrency As String, ByVal vReceiptCurrencyID As Integer, ByVal vCurrencyRate As Double) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Protect against invalid currency rate
        If vCurrencyRate = 0 Then
            vCurrencyRate = 1
        End If

        ' Update receipt currenc/y on all objects
        For Each oRecovery As CLMRecovery In m_oRecoverys
            oRecovery.ReceiptCurrency = vReceiptCurrency
            oRecovery.ReceiptCurrencyID = vReceiptCurrencyID
            oRecovery.CurrencyRate = vCurrencyRate
        Next oRecovery

        Return result
    End Function


    ' Entry point for any termination code for this object.
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oRecoverys = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub



    ' Loops round the collection, doing any required Adds, Deletes or Updates.
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim bTransStarted As Boolean
        Dim oRecovery As CLMRecovery

        Const kMethodName As String = "Update"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Just a quick check to see if there's anything to do
            If CheckCancel() <> gPMConstants.PMEReturnCode.PMDataChanged Then
                Return result
            End If

            ' If we haven't already started a transaction start one.
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans()", "Failed to start transaction")
            End If
            bTransStarted = True

            ' Process each recovery
            For Each oRecovery2 As CLMRecovery In m_oRecoverys
                oRecovery = oRecovery2
                ' See which state we are in
                Select Case True
                    Case oRecovery.IsDeleted
                        ' We just need to delete this record
                        lReturn = CType(oRecovery.Delete(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oRecovery.Delete()", "Failed to delete item")
                        End If

                    Case oRecovery.IsNew
                        ' We just need to add this record
                        lReturn = CType(oRecovery.Add(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oRecovery.Add()", "Failed to add item")
                        End If

                    Case oRecovery.IsDirty
                        ' We may just need to update this record
                        lReturn = CType(oRecovery.Update(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("oRecovery.Update()", "Failed to update item")
                        End If

                End Select
            Next oRecovery2

            ' If we're in a transaction commit it.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans()", "Failed to commit transaction")
                End If
                bTransStarted = False
            End If

            ' Now we've committed tidy up our objects
            ' Note: We cannot use "for each" as we may be removing objects
            Do While lCount < m_oRecoverys.Count()
                oRecovery = m_oRecoverys.Item(lCount + 1)
                ' If record is deleted remove from collection else reset all flags
                If oRecovery.IsDeleted Then
                    m_oRecoverys.Remove(lCount + 1)
                Else
                    oRecovery.IsNew = False
                    oRecovery.IsDirty = False
                    lCount += 1
                End If
            Loop


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If we're in a transaction roll it back.
            If bTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

        Finally
        End Try

        Return result
    End Function



    ' ***************************************************************** '
    '                       PRIVATE FUNCTIONS
    ' ***************************************************************** '

    ' Add payment details
    Private Function AddPaymentDetails() As Integer

        Dim result As Integer = 0
        Dim dPaymentDate As Date
        Dim sComments As String = ""

        Dim vArray(,) As Object ' work array

        Const kMethodName As String = "AddPaymentDetails"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set payment date and get comments
        dPaymentDate = DateTime.Now
        sComments = m_oRecoverys.Comments

        ' Save each receipt
        For Each oRecovery As CLMRecovery In m_oRecoverys
            ' Check we have a receipt amount
            If oRecovery.ThisReceipt <> 0 Then
                ' Check for coinsurance details
                If Information.IsArray(oRecovery.Coinsurance) Then
                    ' grab working copy and process

                    vArray = VB6.CopyArray(oRecovery.Coinsurance)

                    For lCount As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                        ' Add database parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", oRecovery.RecoveryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_peril_id", oRecovery.PerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_type_id", oRecovery.RecoveryTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", oRecovery.ClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "amount", vArray(ACCIThisPayment, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "date_of_payment", dPaymentDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", oRecovery.ReceiptCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", vArray(ACCIPartyCnt, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "comments", oRecovery.RecoveryType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "tax_amount", vArray(ACCITaxAmount, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment_loss_xrate", oRecovery.CurrencyRate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        ' Execute the command
                        lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPaymentSQL, sSQLName:=ACAddPaymentName, bStoredProcedure:=ACAddPaymentStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to save coinsurance payment details")
                        End If

                        ' Store payment ID

                        vArray(ACCIPaymentID, lCount) = m_oDatabase.Parameters.Item("payment_id").Value
                    Next lCount

                    ' Store array back with any payment ID updates

                    oRecovery.Coinsurance = vArray
                End If

                ' Check for coinsurance details
                If Information.IsArray(oRecovery.Reinsurance) Then
                    ' grab working copy and process

                    vArray = VB6.CopyArray(oRecovery.Reinsurance)

                    For lCount As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                        ' Add database parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", oRecovery.RecoveryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_peril_id", oRecovery.PerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_type_id", oRecovery.RecoveryTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", oRecovery.ClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "amount", vArray(ACRIThisPayment, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "date_of_payment", dPaymentDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", oRecovery.ReceiptCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(vArray(ACRIFACPartyCnt, lCount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", vArray(ACRIFACPartyCnt, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        Else

                            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        End If
                        bPMAddParameter.AddParameterLite(m_oDatabase, "comments", oRecovery.RecoveryType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "tax_amount", vArray(ACRITaxAmount, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment_loss_xrate", oRecovery.CurrencyRate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                        Dim dbNumericTemp2 As Double
                        If Double.TryParse(CStr(vArray(ACRITreatyID, lCount)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", vArray(ACRITreatyID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        Else

                            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        End If

                        ' Execute the command
                        lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPaymentSQL, sSQLName:=ACAddPaymentName, bStoredProcedure:=ACAddPaymentStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to save reinsurance payment details")
                        End If

                        ' Store payment ID

                        vArray(ACRIPaymentID, lCount) = m_oDatabase.Parameters.Item("payment_id").Value
                    Next lCount

                    ' Store array back with any payment ID updates

                    oRecovery.Reinsurance = vArray
                End If
            End If
        Next oRecovery

        Return result
        ' This is for debugging only
    End Function


    ' Add receipt details
    Private Function AddReceiptDetails() As Integer

        Dim result As Integer = 0
        Dim dReceiptDate As Date

        Const kMethodName As String = "AddReceiptDetails"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set receipt date
        dReceiptDate = DateTime.Now

        ' Save each receipt
        For Each oRecovery As CLMRecovery In m_oRecoverys
            If oRecovery.ThisReceipt <> 0 Then
                ' Add database parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "receipt_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", oRecovery.RecoveryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_type_id", oRecovery.RecoveryTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "claim_peril_id", oRecovery.PerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "amount", oRecovery.NetReceipt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", oRecovery.ReceiptCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "date_of_receipt", dReceiptDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", oRecovery.ClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "comments", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                bPMAddParameter.AddParameterLite(m_oDatabase, "receipt_loss_xrate", oRecovery.CurrencyRate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                bPMAddParameter.AddParameterLite(m_oDatabase, "tax_amount", oRecovery.TaxAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                ' Execute the command
                lReturn = m_oDatabase.SQLAction(sSQL:=ACAddReceiptSQL, sSQLName:=ACAddReceiptName, bStoredProcedure:=ACAddReceiptStored)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to save receipt details")
                End If

                ' Store receipt ID
                oRecovery.ReceiptID = m_oDatabase.Parameters.Item("receipt_id").Value
            End If
        Next oRecovery

        Return result
        ' This is for debugging only
    End Function


    ' Add reinsurance salvage and recovery details
    Private Function AddReinsuranceDetails(ByVal v_lPerilID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddReinsuranceDetails"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "peril_id", v_lPerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        ' Execute SQL Statement
        lReturn = m_oDatabase.SQLAction(sSQL:=ACAddReinsuranceDetailsSQL, sSQLName:=ACAddReinsuranceDetailsName, bStoredProcedure:=ACAddReinsuranceDetailsStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to add reinsurance details")
        End If

        Return result
        ' This is for debugging only
    End Function

    ' Add payment details
    Private Function DeletePaymentDetails() As Integer

        Dim result As Integer = 0

        Dim vArray(,) As Object ' work array

        Const kMethodName As String = "DeletePaymentDetails"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Save each receipt
        For Each oRecovery As CLMRecovery In m_oRecoverys
            ' Check for coinsurance details
            If Information.IsArray(oRecovery.Coinsurance) Then
                ' grab working copy and process

                vArray = VB6.CopyArray(oRecovery.Coinsurance)

                For lCount As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    If CDbl(vArray(ACCIPaymentID, lCount)) <> 0 Then
                        ' Add database parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment_id", vArray(ACCIPaymentID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                        ' Execute the command
                        lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPaymentSQL, sSQLName:=ACDelPaymentName, bStoredProcedure:=ACDelPaymentStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to delete coinsurance payment details")
                        End If
                    End If
                Next lCount
            End If

            ' Check for coinsurance details
            If Information.IsArray(oRecovery.Reinsurance) Then
                ' grab working copy and process

                vArray = VB6.CopyArray(oRecovery.Reinsurance)

                For lCount As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                    If CDbl(vArray(ACRIPaymentID, lCount)) <> 0 Then
                        ' Add database parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "payment_id", vArray(ACRIPaymentID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                        ' Execute the command
                        lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPaymentSQL, sSQLName:=ACDelPaymentName, bStoredProcedure:=ACDelPaymentStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to delete reinsurance payment details")
                        End If
                    End If
                Next lCount
            End If

        Next oRecovery

        Return result
        ' This is for debugging only
    End Function


    ' Add receipt details
    Private Function DeleteReceiptDetails() As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "DeleteReceiptDetails"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Delete each receipt
        For Each oRecovery As CLMRecovery In m_oRecoverys
            If oRecovery.ReceiptID <> 0 Then
                ' Add database parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "receipt_id", oRecovery.ReceiptID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                ' Execute the command
                lReturn = m_oDatabase.SQLAction(sSQL:=ACDelReceiptSQL, sSQLName:=ACDelReceiptName, bStoredProcedure:=ACDelReceiptStored)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to delete receipt details")
                End If
            End If
        Next oRecovery

        Return result
        ' This is for debugging only
    End Function

    ' Gets the CoinsuranceRecoveries for the given Claim Id
    Private Function GetCoinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lPerilID As Integer, ByVal v_bIsSalvage As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCoinsuranceRecoveries"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "peril_id", v_lPerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", Math.Abs(CInt(v_bIsSalvage)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

        'Execute SQL Statement
        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCoinsurerDetailsSQL, sSQLName:=ACGetCoinsurerDetailsName, bStoredProcedure:=ACGetCoinsurerDetailsStored, vResultArray:=r_vResultArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to get Coinsurance Recoveries")
        End If

        ' If NO records were found return Not Found
        If Not Information.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result
        ' This is for debugging only
    End Function


    ' Gets the ReinsuranceRecoveries for the given Claim Id
    Private Function GetReinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lPerilID As Integer, ByVal v_bIsSalvage As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReinsuranceRecoveries"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "peril_id", v_lPerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", Math.Abs(CInt(v_bIsSalvage)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

        'Execute SQL Statement
        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReinsurerDetailsSQL, sSQLName:=ACGetReinsurerDetailsName, bStoredProcedure:=ACGetReinsurerDetailsStored, vResultArray:=r_vResultArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to get Reinsurance Recoveries")
        End If

        ' If NO records were found return Not Found
        If Not Information.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result
        ' This is for debugging only
    End Function


    ' Post total amount of salvage to orion for this peril
    Private Function PostToOrion(ByVal bIsSalvage As Integer, ByVal lInsuranceFileCnt As Integer, ByVal lClaimID As Integer, ByVal lPerilID As Integer, ByVal lDebitPartyCnt As Integer, ByVal sDebitAccountCode As String, ByVal sDebitMappingCode As String, ByVal lCOBID As Integer, ByVal sCOBCode As String) As Integer

        Dim result As Integer = 0
        Dim oControlTrans As bControlTransClaims.Automated
        Dim oTaxes As CLMRecoveryTaxes

        Dim sCreditAccountCode, sCreditMappingCode As String

        Dim lCreditAccountID, lDebitAccountID As Integer

        Dim lStatsFolderCnt As Integer

        Const kMethodName As String = "Form_Load"
        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set credit codes
            sCreditAccountCode = "CLAIMEXP"
            sCreditMappingCode = "CLMEXP" & sCOBCode

            ' Create object to send to orion

            oControlTrans = New bControlTransClaims.Automated
            lReturn = oControlTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance()", "Failed to get instance of bControlTransClaims.Automated")
            End If

            ' Get debit account id - use party count if we have it
            If lDebitPartyCnt <> 0 Then

                lReturn = oControlTrans.GetAccountID(r_lAccountID:=lDebitAccountID, v_lPartyCnt:=lDebitPartyCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oControlTrans.GetAccountID()", "Failed to get account id")
                End If
            End If

            ' Data which goes in stats folder/detail and transaction detail

            oControlTrans.DebitAccountID = lDebitAccountID

            oControlTrans.CreditAccountID = lCreditAccountID

            oControlTrans.TransactionTypeID = 28

            oControlTrans.TransactionTypeCode = IIf(bIsSalvage, "C_SA", "C_RV") ' Set appropriate code

            oControlTrans.DocumentTypeID = 29 ' Claim receipt

            oControlTrans.InsuranceFileCnt = lInsuranceFileCnt

            oControlTrans.ClaimID = lClaimID

            oControlTrans.PerilID = lPerilID

            oControlTrans.DebitCredit = "C"

            oControlTrans.DocumentComment = IIf(bIsSalvage, "Salvage", "TP Recovery")

            ' Transaction amount depends if we are posting taxes
            If m_oRecoverys.IsPostTaxes Then
                ' Only post net, we will post taxes shortly

                oControlTrans.TransactionAmount = m_oRecoverys.NetReceiptTotal
            Else
                ' Post everything to gross

                oControlTrans.TransactionAmount = m_oRecoverys.ThisReceiptTotal
            End If

            ' Need to create stats separately now for each record to account for reins and coins.

            lReturn = oControlTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=IIf(bIsSalvage, "C_SA", "C_RV"))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oControlTrans.CreateStatsFolder", "Failed to create stats folder")
            End If

            ' Create stats_detail for main payment.

            lReturn = oControlTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=lCOBID, v_sClassOfBusCode:=sCOBCode, v_lRIPartyCnt:=lDebitPartyCnt, v_sRIShortName:=sDebitMappingCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oControlTrans.CreateStatsDetails()", "Failed to create stats detail for GRS line")
            End If

            ' Only post taxes if we are supposed to
            If m_oRecoverys.IsPostTaxes Then
                ' Pass total tax amount and set account code

                oControlTrans.TransactionAmount = m_oRecoverys.TaxTotal

                ' Create stats for TAG amount

                lReturn = oControlTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAG", v_lClassOfBusId:=lCOBID, v_sClassOfBusCode:=sCOBCode, v_lRIPartyCnt:=lDebitPartyCnt, v_sRIShortName:=sDebitMappingCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("oControlTrans.CreateStatsDetails()", "Failed to create stats detail for TAG line")
                End If

                ' If there is any tax post the NET lines for it
                oTaxes = m_oRecoverys.TaxTypeCollection

                ' Save each tax type
                For Each oTax As CLMRecoveryTax In oTaxes
                    ' Pass tax amount and set account code

                    oControlTrans.TransactionAmount = -oTax.TaxTotal
                    sCreditAccountCode = "NOTA" & oTax.TaxTypeCode.Trim() & "IN"

                    ' Create stats for TAN amount

                    lReturn = oControlTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAN", v_lClassOfBusId:=lCOBID, v_sClassOfBusCode:=sCOBCode, v_lRIPartyCnt:=0, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("oControlTrans.CreateStatsDetails()", "Failed to create stats detail for TAG line")
                    End If
                Next oTax
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            If Not (oControlTrans Is Nothing) Then

                oControlTrans.Dispose()
                oControlTrans = Nothing

            End If

        End Try

        Return result
    End Function


    ' Add receipt details
    Private Function UpdateReceiptDetails(ByVal lPartyCnt As Integer, ByVal sComments As String) As Integer

        Dim result As Integer = 0

        Const kMethodName As String = "UpdateReceiptDetails"
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Save each receipt
        For Each oRecovery As CLMRecovery In m_oRecoverys
            If oRecovery.ReceiptID <> 0 Then
                ' Add database parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "receipt_id", oRecovery.ReceiptID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "comments", sComments, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                ' Execute the command
                lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdReceiptSQL, sSQLName:=ACUpdReceiptName, bStoredProcedure:=ACUpdReceiptStored)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction()", "Failed to update receipt details")
                End If
            End If
        Next oRecovery

        Return result
        ' This is for debugging only
    End Function


    ' ***************************************************************** '
    '                         CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()


        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.3.5.6.1)
    'This  method will get agent and client details based on the ClaimID
    Public Function GetAttachedParties(ByRef r_lAgentCnt As Integer, ByRef r_sAgentCode As String, ByRef r_sAgentName As String, ByRef r_lClientCnt As Integer, ByRef r_sClientCode As String, ByRef r_sClientName As String, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAttachedParties"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_id", v_lClaim_Id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAttachedPartiesOnClaimSQL, sSQLName:=ACGetAttachedPartiesOnClaimName, bStoredProcedure:=ACGetAttachedPartiesOnClaimStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Failed to Retrieve the Parties on Claim", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Information.IsArray(vResultArray) Then


                    If CStr(vResultArray(ACRRecoveryClientID, ACRRecoveryClientAgentDefault)) <> "" Then
                        'Sankar - PN 71071 - Added ToSafeLong

                        r_lClientCnt = gPMFunctions.ToSafeLong(CStr(vResultArray(ACRRecoveryClientID, ACRRecoveryClientAgentDefault)))
                    Else
                        r_lClientCnt = ACRRecoveryClientAgentDefault
                    End If


                    r_sClientCode = CStr(vResultArray(ACRRecoveryClientCode, ACRRecoveryClientAgentDefault))

                    r_sClientName = CStr(vResultArray(ACRRecoveryClientName, ACRRecoveryClientAgentDefault))


                    If CStr(vResultArray(ACRRecoveryAgentID, ACRRecoveryClientAgentDefault)) <> "" Then
                        'Sankar - PN 71071 - Added ToSafeLong

                        r_lAgentCnt = gPMFunctions.ToSafeLong(CStr(vResultArray(ACRRecoveryAgentID, ACRRecoveryClientAgentDefault)))
                    Else
                        r_lAgentCnt = ACRRecoveryClientAgentDefault
                    End If


                    r_sAgentCode = CStr(vResultArray(ACRRecoveryAgentCode, ACRRecoveryClientAgentDefault))

                    r_sAgentName = CStr(vResultArray(ACRRecoveryAgentName, ACRRecoveryClientAgentDefault))

                End If

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally





        End Try
        Return result
    End Function
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.3.5.6.1)

    ' Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
    Public Function UpdateRecoveryPartyLink(ByVal lRecoveryId As Integer, ByVal lRecoveryPartyTypeId As Integer, ByVal lRecoveryPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRecoveryPartyLink"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", lRecoveryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery_Party_Type_Id", lRecoveryPartyTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery_Party_cnt", lRecoveryPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePartyLinkSQL, sSQLName:=ACUpdatePartyLinkName, bStoredProcedure:=ACUpdatePartyLinkStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to update party link")
            End If

            Return result

        Catch ex As Exception


            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)



            Return result
        End Try
    End Function
    ' End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)
End Class
