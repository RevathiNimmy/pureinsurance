Option Strict On

Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge

'Namespace CoreImplementation
Namespace DataTransfer

    Public Class General

        Public Enum DTLinkKey
            ClaimPeril = 1
        End Enum

        Friend _SiriusUser As New SIRIUSUSER

        Public Sub AddDataTransferLink(
        ByVal con As SiriusConnection,
        ByVal siriusKey As Integer,
        ByVal samStagingKey As Integer,
        ByVal keyType As DTLinkKey)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_DT_AddDTLink")
                cmd.AddInParameter("@siriuskey", SqlDbType.Int).Value = siriusKey
                cmd.AddInParameter("@samstagingkey", SqlDbType.Int).Value = samStagingKey
                cmd.AddInParameter("@keytype", SqlDbType.Int).Value = keyType

                con.ExecuteNonQuery(cmd)

            End Using

        End Sub

        Public Sub AddDataTransferLink(ByVal con As SiriusConnection,
                  ByVal siriusBaseClaimPerilKey As Integer,
                  ByVal samStagingClaimPerilKey As Integer,
                  ByVal versionId As Integer)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_Add_Claim_Peril_DT_Link")
                cmd.AddInParameter("@siriusBaseClaimPerilKey", SqlDbType.Int).Value = siriusBaseClaimPerilKey
                cmd.AddInParameter("@samstagingClaimPerilKey", SqlDbType.Int).Value = samStagingClaimPerilKey
                cmd.AddInParameter("@versionid", SqlDbType.Int).Value = versionId
                cmd.AddInParameter("@keytype", SqlDbType.Int).Value = DTLinkKey.ClaimPeril

                con.ExecuteNonQuery(cmd)

            End Using

        End Sub

        Public Overloads Function GetDataTransferLink(
        ByVal con As SiriusConnection,
        ByVal samStagingKey As Integer,
        ByVal keyType As DTLinkKey) As Integer

            Dim returnValue As Integer = 0

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_DT_GetDTLink")
                cmd.AddOutParameter("@siriuskey", SqlDbType.Int)
                cmd.AddInParameter("@samstagingkey", SqlDbType.Int).Value = samStagingKey
                cmd.AddInParameter("@keytype", SqlDbType.Int).Value = keyType

                con.ExecuteNonQuery(cmd)

                returnValue = Cast.ToInt32(cmd.Parameters.Item("@siriuskey").Value, 0)

            End Using

            Return returnValue

        End Function

        Public Overloads Function GetDataTransferLink(
            ByVal samStagingKey As Integer,
            ByVal keyType As DTLinkKey) As Integer

            Dim returnValue As Integer = 0

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_DT_GetDTLink")
                    cmd.AddOutParameter("@siriuskey", SqlDbType.Int)
                    cmd.AddInParameter("@samstagingkey", SqlDbType.Int).Value = samStagingKey
                    cmd.AddInParameter("@keytype", SqlDbType.Int).Value = keyType

                    con.ExecuteNonQuery(cmd)

                    returnValue = Cast.ToInt32(cmd.Parameters.Item("@siriuskey").Value, 0)

                End Using

                Return returnValue
            End Using

        End Function

        Public Function ClaimDataImport(ByVal request As BaseCDTRequestType) As BaseCDTResponseType

            Dim response As BaseImplementationTypes.BaseCDTResponseType = Nothing
            Dim typeOfPackage As CoreSAMBusiness.enumTypeOfPackage

            Dim samErrorCollection As SAMErrorCollection = New SAMErrorCollection

            ' determine the type of package and thus the type of response
            If request.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ClaimDataImportRequestType) Then
                typeOfPackage = CoreSAMBusiness.enumTypeOfPackage.SAMForInsurancePackage
                response = New SAMForInsuranceV2ImplementationTypes.ClaimDataImportResponseType
            End If

            ' validate the mandatory structure data
            request.Validate(CType(samErrorCollection, Object))

            ' if there were any errors throw an exception
            samErrorCollection.CheckForErrors()

            If request.UseFullClaimVersioning Then
                response = ClaimDataImportWithFullVersioning(request)
            Else
                response = ClaimDataImportWithPartialVersioning(request)
            End If

            Return response

        End Function
        ''' <summary>
        ''' ClaimDataImportWithFullVersioning
        ''' </summary>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ClaimDataImportWithFullVersioning(ByVal request As BaseCDTRequestType) As BaseCDTResponseType

            ' Declare the Response object
            Dim response As New BaseImplementationTypes.BaseCDTResponseType
            Dim typeOfPackage As CoreSAMBusiness.enumTypeOfPackage

            ' Declare the Core SAM business object
            Dim business As New CoreSAMBusiness
            Dim coreBusiness As New CoreBusiness
            Dim claimGeneral As New Claims.General
            Dim coreSAMBusiness As New CoreSAMBusiness

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            ' determine the type of package and thus the type of response
            If request.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ClaimDataImportRequestType) Then
                typeOfPackage = CoreSAMBusiness.enumTypeOfPackage.SAMForInsurancePackage
                response = New SAMForInsuranceV2ImplementationTypes.ClaimDataImportResponseType
            End If

            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                       _SiriusUser.Username, _SiriusUser.SourceID,
                                        _SiriusUser.LanguageID, SiriusUserDefaults.AppName)

                Dim iInsuranceFileKey As Integer
                iInsuranceFileKey = coreSAMBusiness.GetAndValidateSpecifiedTableCode(con, "insurance_file", "insurance_file_cnt", "insurance_file_cnt", request.Claim.SiriusInsuranceFileKey.ToString, oSAMErrorCollection, "SiriusInsuranceFileKey")
                oSAMErrorCollection.CheckForErrors()
                ' determine if the claims has a payment or 
                ' a receipt against it...
                Try
                    Dim listOfTransactions As List(Of ClaimPaymentAndReceiptTransaction)

                    ' build a list of payments and receipt and order by transaction date 
                    listOfTransactions = BuildTransactionList(request.Claim)
                    'con.BeginTransaction()

                    ' there are no payments or receipts 
                    If listOfTransactions.Count = 0 Then

                        ' so this is either open claim or maintain claim

                        ' if no claim number is provided then

                        Dim versionId As Integer = 0
                        Dim baseClaimId As Integer = 0

                        If claimGeneral.ClaimExists(request.Claim.ClaimNumber, baseClaimId, versionId) Then

                            ' Maintain Claim
                            ProcessMaintainClaim(con, coreSAMBusiness, request.BranchCode, request.Claim, baseClaimId, versionId, True)

                        Else

                            ' Open Claim
                            ProcessOpenClaim(con, claimGeneral, coreSAMBusiness, request.BranchCode, request.Claim, True)

                        End If
                    Else

                        ' there is either a single payment or a single receipt
                        ' any more and this is an error
                        If listOfTransactions.Count = 1 Then
                            SetBaseSiriusKeysIntoRequest(con, coreSAMBusiness, request.Claim, request)
                            For Each transaction As ClaimPaymentAndReceiptTransaction In listOfTransactions

                                If transaction.ClaimPayment IsNot Nothing Then
                                    ProcessClaimPaymentFullVersioning(con, coreSAMBusiness, request.BranchCode, transaction.Claim, transaction.ClaimPeril, transaction.ClaimPayment)
                                End If

                                If transaction.ClaimReceipt IsNot Nothing Then
                                    ProcessClaimReceiptFullVersioning(con, coreSAMBusiness, request.BranchCode, transaction.Claim, transaction.ClaimPeril, transaction.ClaimReceipt)
                                End If

                                ' add reserve / recovery revision entries
                                ProcessTransactionReservesAndRecoveries(con, coreSAMBusiness, transaction)

                            Next

                            ' create dt links for claim peril records - to enable 
                            For Each claimPeril As BaseCDTClaimPerilType In request.Claim.ClaimPeril
                                AddDataTransferLink(con, claimPeril.SiriusBaseClaimPerilKey, claimPeril.SAMStagingClaimPerilKey, request.Claim.VersionId)
                            Next

                        Else
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.MultiplePaymentsOrReceiptsSpecifiedAgainstASingleClaimVersion, SAMBusinessErrors.MultiplePaymentsOrReceiptsSpecifiedAgainstASingleClaimVersion.ToString, "Claim Number : " + request.Claim.ClaimNumber + " has multiple payments and or receipts specified against it.")
                            oSAMErrorCollection.CheckForErrors()
                        End If

                    End If

                    ' Set Sirius Keys Into Request For Additional Processing
                    SetSiriusKeysIntoRequest(con, claimGeneral, coreSAMBusiness, request.Claim, request)

                    ProcessClaimReinsurance(con, coreSAMBusiness, request.Claim.ClaimReinsuranceForDTU, request.Claim.SiriusClaimKey, request.Claim.SiriusRiskKey)

                    ProcessClaimRiskData(con, coreSAMBusiness, request.Claim)

                    If request.Claim.TransactionDateSpecified Then
                        UpdateClaimTransactionDate(con, request.Claim.SiriusClaimKey, request.Claim.TransactionDate)
                    End If
                    'con.CommitTransaction()

                    Return response
                Catch ex As Exception
                    'con.RollbackTransaction()
                    Throw
                End Try
            End Using
        End Function

        Private Sub ProcessTransactionReservesAndRecoveries(ByVal con As SiriusConnection,
        ByRef coreSAMBusiness As CoreSAMBusiness,
        ByRef transaction As ClaimPaymentAndReceiptTransaction)

            'claim.SiriusBaseClaimKey = claimReceiptResponse.BaseClaimKey
            'claim.SiriusClaimKey = claimReceiptResponse.ClaimKey
            'claim.ClaimNumber = claimReceiptResponse.ClaimNumber
            'claim.VersionId = claimReceiptResponse.Version

            If transaction.Claim IsNot Nothing AndAlso
                transaction.Claim.ClaimPeril IsNot Nothing Then

                For Each claimPeril As BaseCDTClaimPerilType In transaction.Claim.ClaimPeril

                    If claimPeril.Reserve IsNot Nothing Then

                        For Each reserve As BaseCDTReserveType In claimPeril.Reserve

                            If reserve.RevisionAmount <> 0 Then

                                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CDT_Transaction_Reserve_Update")

                                    cmd.AddInParameter("@base_claim_peril_id", SqlDbType.Int).Value = claimPeril.SiriusBaseClaimPerilKey
                                    cmd.AddInParameter("@version_id", SqlDbType.Int).Value = transaction.Claim.VersionId
                                    cmd.AddInParameter("@type_code", SqlDbType.VarChar, 50).Value = reserve.TypeCode
                                    cmd.AddInParameter("@this_revision", SqlDbType.Money).Value = reserve.RevisionAmount

                                    Dim numberOfRowsAffected As Integer = con.ExecuteNonQuery(cmd)

                                    If numberOfRowsAffected <> 1 Then
                                        Dim samErrorsCollection As New SAMErrorCollection
                                        samErrorsCollection.AddBusinessRule(
                                            SAMBusinessErrors.ClaimDataTransferFailedToUpdateReserveItem,
                                            SAMBusinessErrors.ClaimDataTransferFailedToUpdateRecoveryItem.ToString)
                                        samErrorsCollection.CheckForErrors()
                                    End If

                                End Using

                            End If

                        Next

                    End If

                    If claimPeril.Recovery IsNot Nothing Then

                        For Each recovery As BaseCDTRecoveryType In claimPeril.Recovery

                            If recovery.RevisionAmount <> 0 Then

                                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CDT_Transaction_Recovery_Update")

                                    cmd.AddInParameter("@base_claim_peril_id", SqlDbType.Int).Value = claimPeril.SiriusBaseClaimPerilKey
                                    cmd.AddInParameter("@version_id", SqlDbType.Int).Value = transaction.Claim.VersionId
                                    cmd.AddInParameter("@type_code", SqlDbType.VarChar, 50).Value = recovery.TypeCode
                                    cmd.AddInParameter("@this_revision", SqlDbType.Money).Value = recovery.RevisionAmount

                                    Dim numberOfRowsAffected As Integer = con.ExecuteNonQuery(cmd)

                                    If numberOfRowsAffected <> 1 Then
                                        Dim samErrorsCollection As New SAMErrorCollection
                                        samErrorsCollection.AddBusinessRule(
                                            SAMBusinessErrors.ClaimDataTransferFailedToUpdateRecoveryItem,
                                            SAMBusinessErrors.ClaimDataTransferFailedToUpdateRecoveryItem.ToString)
                                        samErrorsCollection.CheckForErrors()
                                    End If

                                End Using

                            End If

                        Next

                    End If

                Next

            End If

        End Sub

        Private Sub UpdateClaimTransactionDate(ByVal con As SiriusConnection, ByVal claimId As Integer, ByVal transactionDate As Date)

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_UpdateClaimTransactionDate")
                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = claimId
                cmd.AddInParameter("@transaction_date", SqlDbType.DateTime).Value = transactionDate

                con.ExecuteNonQuery(cmd)

            End Using

        End Sub
        ''' <summary>
        ''' ClaimDataImportWithPartialVersioning
        ''' </summary>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ClaimDataImportWithPartialVersioning(ByVal request As BaseCDTRequestType) As BaseCDTResponseType

            ' Declare the Response object
            Dim response As New BaseImplementationTypes.BaseCDTResponseType
            Dim typeOfPackage As CoreSAMBusiness.enumTypeOfPackage

            ' Declare the Core SAM business object
            Dim business As New CoreSAMBusiness
            Dim coreBusiness As New CoreBusiness
            Dim claimGeneral As New Claims.General
            Dim coreSAMBusiness As New CoreSAMBusiness

            Dim oSAMErrorCollection As SAMErrorCollection = New SAMErrorCollection

            ' determine the type of package and thus the type of response
            If request.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.ClaimDataImportRequestType) Then
                typeOfPackage = CoreSAMBusiness.enumTypeOfPackage.SAMForInsurancePackage
                response = New SAMForInsuranceV2ImplementationTypes.ClaimDataImportResponseType
            End If

            Dim iInsuranceFileKey As Integer

            Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                        _SiriusUser.Username, _SiriusUser.SourceID,
                                         _SiriusUser.LanguageID, SiriusUserDefaults.AppName)

                iInsuranceFileKey = coreSAMBusiness.GetAndValidateSpecifiedTableCode(con, "insurance_file", "insurance_file_cnt", "insurance_file_cnt", request.Claim.SiriusInsuranceFileKey.ToString, oSAMErrorCollection, "SiriusInsuranceFileKey")
                oSAMErrorCollection.CheckForErrors()

                Dim versionId As Integer = 0
                Dim baseClaimId As Integer = 0

                Try
                    'con.BeginTransaction()

                    If claimGeneral.ClaimExists(con, request.Claim.ClaimNumber, baseClaimId, versionId) Then

                        ' Maintain Claim
                        ProcessMaintainClaim(con, coreSAMBusiness, request.BranchCode, request.Claim, baseClaimId, versionId, False)

                    Else

                        ' Open Claim
                        ProcessOpenClaim(con, claimGeneral, coreSAMBusiness, request.BranchCode, request.Claim, False)

                    End If

                    ' Set Sirius Keys Into Request For Additional Processing
                    SetSiriusKeysIntoRequest(con, claimGeneral, coreSAMBusiness, request.Claim, request)

                    ProcessClaimReinsurance(con, coreSAMBusiness, request.Claim.ClaimReinsuranceForDTU, request.Claim.SiriusClaimKey, request.Claim.SiriusRiskKey)

                    ProcessClaimRiskData(con, coreSAMBusiness, request.Claim)

                    ProcessClaimPaymentsAndReceipts(con, coreSAMBusiness, request.BranchCode, request.Claim)
                    'con.CommitTransaction()
                Catch ex As Exception
                    Throw
                    Return Nothing
                End Try

                Return response
            End Using
        End Function

        Private Sub SetSiriusKeysIntoRequest(ByVal con As SiriusConnection, _
     ByVal claimGeneral As Claims.General, _
     ByVal coreSAMBusiness As CoreSAMBusiness, _
     ByVal claim As BaseCDTClaimType, _
     ByVal request As BaseCDTRequestType)

            Dim reserveDetails As DataTable = Nothing
            Dim recoveryDetails As DataTable = Nothing

            ' process claim peril level items
            For Each claimperil As BaseCDTClaimPerilType In claim.ClaimPeril

                ' get sirius claim peril details using dtlinks table
                Dim siriusClaimPerilKey As Integer = GetDataTransferLink(con, claimperil.SAMStagingClaimPerilKey, DTLinkKey.ClaimPeril)
                Dim siriusBaseClaimPerilKey As Integer = coreSAMBusiness.GetClaimPerilDetails(con, siriusClaimPerilKey)

                ' store the claim peril key on this record only 
                ' store the base claim peril key on all records
                claimperil.SiriusClaimPerilKey = siriusClaimPerilKey

                ' get sirius reserve details
                reserveDetails = claimGeneral.GetReserveDetailsForClaimPeril(con, siriusClaimPerilKey)

                ' get sirius recovery details
                recoveryDetails = claimGeneral.GetRecoveryDetailsForClaimPeril(con, siriusClaimPerilKey)

                ' need to tie up all claim peril records with the same samstagingbaseclaimperilkey
                ' if this is another claim record with the same claim number
                If request.Claim.ClaimNumber = claim.ClaimNumber Then

                    If claim.SiriusBaseClaimKey <> 0 Then
                        request.Claim.SiriusBaseClaimKey = claim.SiriusBaseClaimKey
                    End If

                    For Each requestperil As BaseCDTClaimPerilType In request.Claim.ClaimPeril

                        ' if the peril matches and hasnt yet been processed
                        If requestperil.SAMStagingBaseClaimPerilKey = claimperil.SAMStagingBaseClaimPerilKey _
                            AndAlso requestperil.SiriusBaseClaimPerilKey = 0 Then

                            requestperil.SiriusBaseClaimPerilKey = siriusBaseClaimPerilKey

                            ' set reserve details
                            SetReserveSiriusKeys(requestperil.Reserve, reserveDetails)

                            ' set recovery details
                            SetRecoverySiriusKeys(requestperil.Recovery, recoveryDetails)

                            ' set claim payment reserve details
                            SetClaimPaymentSiriusKeys(requestperil.ClaimPayment, reserveDetails)

                            ' set claim receipt recovery details
                            SetClaimReceiptSiriusKeys(requestperil.ClaimReceipt, recoveryDetails)

                        End If

                    Next

                End If

            Next

        End Sub

        ''' <summary>
        ''' Use for set BaseClaimkey,BaseResrveKey,BasePerilKey,BaseRecoveryKey for DTU Claim Payment
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="coreSAMBusiness"></param>
        ''' <param name="claim"></param>
        ''' <param name="request"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>

        Private Sub SetBaseSiriusKeysIntoRequest(ByVal con As SiriusConnection, _
                                            ByVal coreSAMBusiness As CoreSAMBusiness, _
                                            ByVal claim As BaseCDTClaimType, _
                                            ByVal request As BaseCDTRequestType)

            Dim dtReserveDetails As DataTable = Nothing
            Dim dtRecoveryDetails As DataTable = Nothing
            Dim oclaimGeneral As New Claims.General
            ' process claim peril level items
            For Each claimperil As BaseCDTClaimPerilType In claim.ClaimPeril

                ' get sirius claim peril details using dtlinks table
                Dim nSiriusClaimPerilKey As Integer = GetDataTransferLink(con, claimperil.SAMStagingBaseClaimPerilKey, DTLinkKey.ClaimPeril)
                Dim nSiriusBaseClaimPerilKey As Integer = coreSAMBusiness.GetClaimPerilDetails(con, nSiriusClaimPerilKey)

                ' store the claim peril key on this record only 
                ' store the base claim peril key on all records
                claimperil.SiriusClaimPerilKey = nSiriusClaimPerilKey

                ' get sirius reserve details
                dtReserveDetails = oclaimGeneral.GetReserveDetailsForClaimPeril(con, nSiriusClaimPerilKey)

                ' get sirius recovery details
                dtRecoveryDetails = oclaimGeneral.GetRecoveryDetailsForClaimPeril(con, nSiriusClaimPerilKey)

                claim.SiriusBaseClaimKey = oclaimGeneral.GetBaseClaimKeyForClaimPeril(con, nSiriusClaimPerilKey)
                ' need to tie up all claim peril records with the same samstagingbaseclaimperilkey
                ' if this is another claim record with the same claim number
                If request.Claim.ClaimNumber = claim.ClaimNumber Then

                    If claim.SiriusBaseClaimKey <> 0 Then
                        request.Claim.SiriusBaseClaimKey = claim.SiriusBaseClaimKey
                    End If

                    For Each requestperil As BaseCDTClaimPerilType In request.Claim.ClaimPeril

                        ' if the peril matches and hasnt yet been processed
                        If requestperil.SAMStagingBaseClaimPerilKey = claimperil.SAMStagingBaseClaimPerilKey _
                            AndAlso requestperil.SiriusBaseClaimPerilKey = 0 Then

                            requestperil.SiriusBaseClaimPerilKey = nSiriusBaseClaimPerilKey

                            ' set reserve details
                            SetReserveSiriusKeys(requestperil.Reserve, dtReserveDetails)

                            ' set recovery details
                            SetRecoverySiriusKeys(requestperil.Recovery, dtRecoveryDetails)

                            ' set claim payment reserve details
                            SetClaimPaymentSiriusKeys(requestperil.ClaimPayment, dtReserveDetails)

                            ' set claim receipt recovery details
                            SetClaimReceiptSiriusKeys(requestperil.ClaimReceipt, dtRecoveryDetails)

                        End If
                    Next
                End If
            Next
        End Sub

        Private Sub SetReserveSiriusKeys(ByVal reserves As BaseCDTReserveType(), _
        ByVal reserveDetails As DataTable)

            If reserveDetails IsNot Nothing AndAlso reserves IsNot Nothing Then

                For Each reserveRow As DataRow In reserveDetails.Rows

                    Dim typeCode As String = Cast.ToStringTrim(reserveRow.Item("name"))
                    Dim reserveKey As Integer = Cast.ToInt32(reserveRow.Item("base_reserve_id"), 0)

                    For Each reserve As BaseCDTReserveType In reserves

                        If typeCode = reserve.TypeCode Then

                            reserve.SiriusBaseReserveKey = reserveKey

                            Exit For
                        End If

                    Next

                Next

            End If

        End Sub

        Private Sub SetClaimPaymentSiriusKeys(ByVal claimPayments As BaseCDTClaimPaymentType(),
        ByVal reserveDetails As DataTable)

            If reserveDetails IsNot Nothing AndAlso claimPayments IsNot Nothing Then

                For Each reserveRow As DataRow In reserveDetails.Rows

                    Dim typeCode As String = Cast.ToStringTrim(reserveRow.Item("name"))
                    Dim reserveKey As Integer = Cast.ToInt32(reserveRow.Item("base_reserve_id"), 0)

                    For Each claimpayment As BaseCDTClaimPaymentType In claimPayments

                        For Each claimpaymentitem As BaseCDTClaimPaymentItemType In claimpayment.ClaimPaymentItem

                            If typeCode = claimpaymentitem.ReserveTypeCode Then

                                claimpaymentitem.SiriusBaseReserveKey = reserveKey

                            End If

                        Next

                    Next

                Next

            End If

        End Sub

        Private Sub SetRecoverySiriusKeys(ByVal recoveries As BaseCDTRecoveryType(),
        ByVal recoveryDetails As DataTable)

            If recoveryDetails IsNot Nothing AndAlso recoveries IsNot Nothing Then

                For Each RecoveryRow As DataRow In recoveryDetails.Rows

                    Dim typeCode As String = Cast.ToStringTrim(RecoveryRow.Item("code"))
                    Dim RecoveryKey As Integer = Cast.ToInt32(RecoveryRow.Item("base_Recovery_id"), 0)

                    For Each Recovery As BaseCDTRecoveryType In recoveries

                        If typeCode = Recovery.TypeCode Then

                            Recovery.SiriusBaseRecoveryKey = RecoveryKey

                            Exit For
                        End If

                    Next

                Next

            End If

        End Sub

        Private Sub SetClaimReceiptSiriusKeys(ByVal claimReceipts As BaseCDTClaimReceiptType(),
        ByVal recoveryDetails As DataTable)

            If recoveryDetails IsNot Nothing AndAlso claimReceipts IsNot Nothing Then

                For Each RecoveryRow As DataRow In recoveryDetails.Rows

                    Dim typeCode As String = Cast.ToStringTrim(RecoveryRow.Item("code"))
                    Dim RecoveryKey As Integer = Cast.ToInt32(RecoveryRow.Item("base_Recovery_id"), 0)

                    For Each claimReceipt As BaseCDTClaimReceiptType In claimReceipts

                        For Each claimReceiptitem As BaseCDTReceiptItemType In claimReceipt.ClaimReceiptItem

                            If typeCode = claimReceiptitem.RecoveryTypeCode Then

                                claimReceiptitem.SiriusBaseRecoveryKey = RecoveryKey

                            End If

                        Next

                    Next

                Next

            End If

        End Sub
        ''' <summary>
        ''' ProcessOpenClaim
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="claimGeneral"></param>
        ''' <param name="coreSAMBusines"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="claim"></param>
        ''' <param name="useFullClaimVersioning"></param>
        ''' <remarks></remarks>
        Private Sub ProcessOpenClaim(ByVal con As SiriusConnection, _
      ByVal claimGeneral As Claims.General, _
      ByVal coreSAMBusines As CoreSAMBusiness, _
      ByVal branchCode As String, _
      ByVal claim As BaseCDTClaimType, _
      ByVal useFullClaimVersioning As Boolean)

            ' build open claim request
            Dim openClaimRequest As New BaseClaimOpenRequestType
            Dim claimResponse As BaseClaimResponseType
            Dim coreSAMBusiness As New CoreSAMBusiness

            openClaimRequest.BranchCode = branchCode
            openClaimRequest.Claim = ToBaseImpBaseClaimOpenType(claim)
            openClaimRequest.IsDataTransferClaim = True
            openClaimRequest.DataTransferIsUsingFullClaimVersioning = useFullClaimVersioning

            openClaimRequest.DataTransferClaimHasSpecifiedReinsurance = (claim.ClaimReinsuranceForDTU IsNot Nothing)

            ' open claim 
            claimResponse = coreSAMBusiness.OpenClaim(con, openClaimRequest)

            claim.SiriusBaseClaimKey = claimResponse.BaseClaimKey
            claim.SiriusClaimKey = claimResponse.ClaimKey
            claim.ClaimNumber = claimResponse.ClaimNumber
            claim.VersionId = claimResponse.Version

        End Sub
        ''' <summary>
        ''' ProcessMaintainClaim
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="coreSAMBusiness"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="claim"></param>
        ''' <param name="baseClaimId"></param>
        ''' <param name="versionId"></param>
        ''' <param name="useFullClaimVersioning"></param>
        ''' <remarks></remarks>
        Private Sub ProcessMaintainClaim(ByVal con As SiriusConnection, _
        ByVal coreSAMBusiness As CoreSAMBusiness, _
        ByVal branchCode As String, _
        ByVal claim As BaseCDTClaimType, _
        ByVal baseClaimId As Integer, _
        ByVal versionId As Integer, _
        ByVal useFullClaimVersioning As Boolean)

            ' build open claim request
            Dim maintainClaimRequest As New BaseClaimMaintainRequestType
            Dim claimResponse As BaseClaimResponseType

            maintainClaimRequest.BranchCode = branchCode
            maintainClaimRequest.IsDataTransferClaim = True
            maintainClaimRequest.DataTransferClaimHasSpecifiedReinsurance = (claim.ClaimReinsuranceForDTU IsNot Nothing)
            maintainClaimRequest.Claim = ToBaseImpBaseClaimMaintainType(claim)
            maintainClaimRequest.Claim.BaseClaimKey = baseClaimId
            maintainClaimRequest.DataTransferClaimHasClaimRiskDataSpecified = Not String.IsNullOrEmpty(claim.XMLDATASET)
            maintainClaimRequest.DataTransferIsUsingFullClaimVersioning = useFullClaimVersioning

            ' maintain claim 
            claimResponse = coreSAMBusiness.MaintainClaim(con, maintainClaimRequest)

            ' get sirius details from the response
            claim.SiriusBaseClaimKey = claimResponse.BaseClaimKey
            claim.SiriusClaimKey = claimResponse.ClaimKey
            claim.ClaimNumber = claimResponse.ClaimNumber
            claim.VersionId = claimResponse.Version

        End Sub

        Private Sub ProcessClaimPaymentsAndReceipts(ByVal con As SiriusConnection,
        ByVal coreSAMBusiness As CoreSAMBusiness,
        ByVal branchCode As String,
        ByVal claim As BaseCDTClaimType)

            Dim listOfTransactions As List(Of ClaimPaymentAndReceiptTransaction)

            ' build a list of payments and receipt and order by transaction date 
            listOfTransactions = BuildTransactionList(claim)

            If listOfTransactions IsNot Nothing Then

                Debug.Print("claim : " & claim.ClaimNumber.ToString & " claim id :" & claim.SiriusBaseClaimKey.ToString & " has " & listOfTransactions.Count.ToString & " transactions")

                If listOfTransactions.Count > 0 Then

                    For Each transaction As ClaimPaymentAndReceiptTransaction In listOfTransactions

                        If transaction.ClaimPayment IsNot Nothing Then
                            ProcessClaimPayment(con, coreSAMBusiness, branchCode, transaction.Claim, transaction.ClaimPeril, transaction.ClaimPayment)
                        End If

                        If transaction.ClaimReceipt IsNot Nothing Then
                            ProcessClaimReceipt(con, coreSAMBusiness, branchCode, transaction.Claim, transaction.ClaimPeril, transaction.ClaimReceipt)
                        End If

                    Next

                End If

            End If

        End Sub

        Private Function BuildTransactionList( _
        ByVal claim As BaseCDTClaimType) As List(Of ClaimPaymentAndReceiptTransaction)

            Dim listOfTransactions As New List(Of ClaimPaymentAndReceiptTransaction)

            For Each claimperil As BaseCDTClaimPerilType In claim.ClaimPeril

                If claimperil.ClaimPayment IsNot Nothing Then
                    For Each claimpayment As BaseCDTClaimPaymentType In claimperil.ClaimPayment

                        Dim transaction As New ClaimPaymentAndReceiptTransaction

                        transaction.Claim = claim
                        transaction.ClaimPeril = claimperil
                        transaction.ClaimPayment = claimpayment
                        transaction.TransactionDate = claimpayment.TransactionDate
                        transaction.ClaimReceipt = Nothing

                        listOfTransactions.Add(transaction)

                    Next
                End If

                If claimperil.ClaimReceipt IsNot Nothing Then

                    For Each claimreceipt As BaseCDTClaimReceiptType In claimperil.ClaimReceipt

                        Dim Transaction As New ClaimPaymentAndReceiptTransaction

                        Transaction.Claim = claim
                        Transaction.ClaimPeril = claimperil
                        Transaction.ClaimReceipt = claimreceipt
                        Transaction.TransactionDate = claimreceipt.TransactionDate
                        Transaction.ClaimPayment = Nothing

                        listOfTransactions.Add(Transaction)

                    Next

                End If

            Next

            If listOfTransactions.Count > 0 Then
                Dim transactionSort As New TransactionDateSort
                listOfTransactions.Sort(transactionSort)
            End If

            Return listOfTransactions

        End Function

        Private Sub BuildClaimReceiptRequest(
        ByVal request As BaseClaimReceiptRequestType,
        ByVal branchCode As String,
        ByVal claim As BaseCDTClaimType,
        ByVal claimperil As BaseCDTClaimPerilType,
        ByVal claimReceipt As BaseCDTClaimReceiptType)

            request.BranchCode = branchCode
            request.ClaimReceipt = New BaseImplementationTypes.BaseClaimReceiptType
            request.ClaimReceipt.BaseClaimKey = claim.SiriusBaseClaimKey
            request.ClaimReceipt.BaseClaimPerilKey = claimperil.SiriusBaseClaimPerilKey
            request.ClaimReceipt.ClaimVersionDescription = "DT - Claim Receipt"
            request.ClaimReceipt.CurrencyCode = claimReceipt.CurrencyCode
            request.ClaimReceipt.PartyKey = claimReceipt.PartyKey
            request.ClaimReceipt.ReceiptPartyType = claimReceipt.ReceiptPartyType

            request.ClaimReceipt.IsSalvageRecovery = claimReceipt.IsSalvageRecovery
            request.ClaimReceipt.TransactionDate = claimReceipt.TransactionDate

            ' claim receipt items
            If claimReceipt.ClaimReceiptItem IsNot Nothing Then
                request.ClaimReceipt.ReceiptItem = Array.ConvertAll(claimReceipt.ClaimReceiptItem,
                                                            New Converter(Of BaseImplementationTypes.BaseCDTReceiptItemType, 
                                                            BaseImplementationTypes.BaseClaimReceiptItemType) _
                                                            (AddressOf ToBaseImpBaseClaimReceiptItemType))
            End If

            ' advanced tax options
            request.ClaimReceipt.AdvancedTaxDetails = Nothing

            ' payee
            request.ClaimReceipt.Payee = New BaseImplementationTypes.BaseClaimPayeeType
            request.ClaimReceipt.Payee.BankCode = claimReceipt.Payee.BankCode
            request.ClaimReceipt.Payee.BankName = claimReceipt.Payee.BankName
            request.ClaimReceipt.Payee.BankNumber = claimReceipt.Payee.BankNumber
            request.ClaimReceipt.Payee.MediaReference = claimReceipt.Payee.MediaReference
            request.ClaimReceipt.Payee.MediaTypeCode = claimReceipt.Payee.MediaTypeCode
            request.ClaimReceipt.Payee.Name = claimReceipt.Payee.Name
            request.ClaimReceipt.Payee.Comments = claimReceipt.Payee.Comments
            request.ClaimReceipt.Payee.TheirReference = claimReceipt.Payee.TheirReference

            ' payee address
            If claimReceipt.Payee.Address IsNot Nothing Then
                request.ClaimReceipt.Payee.Address = New BaseImplementationTypes.BaseAddressType
                request.ClaimReceipt.Payee.Address.AddressLine1 = claimReceipt.Payee.Address.AddressLine1
                request.ClaimReceipt.Payee.Address.AddressLine2 = claimReceipt.Payee.Address.AddressLine2
                request.ClaimReceipt.Payee.Address.AddressLine3 = claimReceipt.Payee.Address.AddressLine3
                request.ClaimReceipt.Payee.Address.AddressLine4 = claimReceipt.Payee.Address.AddressLine4
                request.ClaimReceipt.Payee.Address.AddressTypeCode = claimReceipt.Payee.Address.AddressTypeCode
                request.ClaimReceipt.Payee.Address.PostCode = claimReceipt.Payee.Address.PostCode
                request.ClaimReceipt.Payee.Address.CountryCode = claimReceipt.Payee.Address.CountryCode
            End If

        End Sub

        Private Sub BuildClaimPaymentRequest(
        ByVal request As BaseClaimPaymentRequestType,
        ByVal branchCode As String,
        ByVal claim As BaseCDTClaimType,
        ByVal claimperil As BaseCDTClaimPerilType,
        ByVal claimPayment As BaseCDTClaimPaymentType)

            request.BranchCode = branchCode
            request.ClaimPayment = New BaseImplementationTypes.BaseClaimPaymentType
            request.ClaimPayment.BaseClaimKey = claim.SiriusBaseClaimKey
            request.ClaimPayment.BaseClaimPerilKey = claimperil.SiriusBaseClaimPerilKey

            '                request.ClaimPayment.samstagingclaimperilkey()

            request.ClaimPayment.ClaimVersionDescription = "DT - Claim Payment"
            request.ClaimPayment.CurrencyCode = claimPayment.CurrencyCode
            request.ClaimPayment.PartyKey = claimPayment.PartyKey
            request.ClaimPayment.PaymentPartyType = claimPayment.PaymentPartyType

            ' default the transaction date to todays date
            ' the only case this will not be true is via a data transfer
            ' where this process needs to support historical payments
            request.ClaimPayment.TransactionDate = claimPayment.TransactionDate

            request.ClaimPayment.AdvancedTaxDetails = Nothing

            request.ClaimPayment.Payee = New BaseImplementationTypes.BaseClaimPayeeType
            request.ClaimPayment.Payee.BankCode = claimPayment.Payee.BankCode
            request.ClaimPayment.Payee.BankName = claimPayment.Payee.BankName
            request.ClaimPayment.Payee.BankNumber = claimPayment.Payee.BankNumber
            request.ClaimPayment.Payee.MediaReference = claimPayment.Payee.MediaReference
            request.ClaimPayment.Payee.MediaTypeCode = claimPayment.Payee.MediaTypeCode
            request.ClaimPayment.Payee.Name = claimPayment.Payee.Name
            request.ClaimPayment.Payee.TheirReference = claimPayment.Payee.TheirReference
            request.ClaimPayment.Payee.Comments = claimPayment.Payee.Comments

            If claimPayment.Payee.Address IsNot Nothing Then
                request.ClaimPayment.Payee.Address = New BaseImplementationTypes.BaseAddressType
                request.ClaimPayment.Payee.Address.AddressLine1 = claimPayment.Payee.Address.AddressLine1
                request.ClaimPayment.Payee.Address.AddressLine2 = claimPayment.Payee.Address.AddressLine2
                request.ClaimPayment.Payee.Address.AddressLine3 = claimPayment.Payee.Address.AddressLine3
                request.ClaimPayment.Payee.Address.AddressLine4 = claimPayment.Payee.Address.AddressLine4
                request.ClaimPayment.Payee.Address.AddressTypeCode = CType([Enum].ToObject(GetType(AddressTypeType), claimPayment.Payee.Address.AddressTypeCode), BaseImplementationTypes.AddressTypeType)
                request.ClaimPayment.Payee.Address.PostCode = claimPayment.Payee.Address.PostCode
                request.ClaimPayment.Payee.Address.CountryCode = claimPayment.Payee.Address.CountryCode
            End If

            If claimPayment.ClaimPaymentItem IsNot Nothing Then
                request.ClaimPayment.ClaimPaymentItem = Array.ConvertAll(claimPayment.ClaimPaymentItem,
                                        New Converter(Of BaseCDTClaimPaymentItemType, BaseClaimPaymentItemType) _
                                        (AddressOf ToBaseImpBaseClaimPaymentItemType))
            End If

        End Sub
        ''' <summary>
        ''' ProcessClaimPaymentFullVersioning
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="coreSAMBusiness"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="claim"></param>
        ''' <param name="claimPeril"></param>
        ''' <param name="claimPayment"></param>
        ''' <remarks></remarks>
        Private Sub ProcessClaimPaymentFullVersioning(ByVal con As SiriusConnection, _
       ByVal coreSAMBusiness As CoreSAMBusiness, _
       ByVal branchCode As String, _
       ByVal claim As BaseCDTClaimType, _
       ByVal claimPeril As BaseCDTClaimPerilType, _
       ByVal claimPayment As BaseCDTClaimPaymentType)

            Dim payClaimRequest As New BaseClaimPaymentRequestType
            Dim payClaimResponse As BaseClaimResponseType

            BuildClaimPaymentRequest(payClaimRequest, branchCode, claim, claimPeril, claimPayment)

            payClaimRequest.IsDataTransferClaim = True
            payClaimRequest.DataTransferClaimHasSpecifiedReinsurance = (claim.ClaimReinsuranceForDTU IsNot Nothing)
            payClaimRequest.DataTransferClaimHasClaimRiskDataSpecified = Not String.IsNullOrEmpty(claim.XMLDATASET)
            payClaimRequest.DataTransferIsUsingFullClaimVersioning = True
            payClaimResponse = DirectCast(coreSAMBusiness.PayClaim(con, payClaimRequest), BaseClaimResponseType)

            claim.SiriusBaseClaimKey = payClaimResponse.BaseClaimKey
            claim.SiriusClaimKey = payClaimResponse.ClaimKey
            claim.ClaimNumber = payClaimResponse.ClaimNumber
            claim.VersionId = payClaimResponse.Version

            ' TODO : MEVANS : Process Claim Payment Reinsurance : Plug in Function When RTaylor Provides It
            Debug.Print("Called Process Claim Payment for claim number: " & claim.ClaimNumber & " claim :" & claim.SiriusClaimKey.ToString & " claim payment " & payClaimRequest.ClaimPayment.BaseClaimPerilKey.ToString)

        End Sub

        Private Sub ProcessClaimPayment(ByVal con As SiriusConnection,
        ByVal coreSAMBusiness As CoreSAMBusiness,
        ByVal branchCode As String,
        ByVal claim As BaseCDTClaimType,
        ByVal claimPeril As BaseCDTClaimPerilType,
        ByVal claimPayment As BaseCDTClaimPaymentType)

            Dim payClaimRequest As New BaseClaimPaymentRequestType
            Dim payClaimResponse As BaseClaimResponseType

            'Static MyCounter As Integer

            BuildClaimPaymentRequest(payClaimRequest, branchCode, claim, claimPeril, claimPayment)
            payClaimRequest.IsDataTransferClaim = True
            payClaimRequest.DataTransferClaimHasSpecifiedReinsurance = (claim.ClaimReinsuranceForDTU IsNot Nothing)
            payClaimRequest.DataTransferIsUsingFullClaimVersioning = False
            payClaimResponse = DirectCast(coreSAMBusiness.PayClaim(con, payClaimRequest), BaseClaimResponseType)

            ' TODO : MEVANS : Process Claim Payment Reinsurance : Plug in Function When RTaylor Provides It
            Debug.Print("Called Process Claim Payment for claim number: " & claim.ClaimNumber & " claim :" & claim.SiriusClaimKey.ToString & " claim payment " & payClaimRequest.ClaimPayment.BaseClaimPerilKey.ToString)
        End Sub
        ''' <summary>
        ''' ProcessClaimReceiptFullVersioning
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="coreSAMBusiness"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="claim"></param>
        ''' <param name="claimPeril"></param>
        ''' <param name="claimReceipt"></param>
        ''' <remarks></remarks>
        Private Sub ProcessClaimReceiptFullVersioning(ByVal con As SiriusConnection, _
                   ByVal coreSAMBusiness As CoreSAMBusiness, _
                   ByVal branchCode As String, _
                   ByVal claim As BaseCDTClaimType, _
                   ByVal claimPeril As BaseCDTClaimPerilType, _
                   ByVal claimReceipt As BaseCDTClaimReceiptType)

            Dim claimReceiptRequest As New BaseClaimReceiptRequestType
            Dim claimReceiptResponse As BaseClaimResponseType

            BuildClaimReceiptRequest(claimReceiptRequest, branchCode, claim, claimPeril, claimReceipt)

            claimReceiptRequest.IsDataTransferClaim = True
            claimReceiptRequest.DataTransferClaimHasSpecifiedReinsurance = (claim.ClaimReinsuranceForDTU IsNot Nothing)
            claimReceiptRequest.DataTransferClaimHasClaimRiskDataSpecified = Not String.IsNullOrEmpty(claim.XMLDATASET)
            claimReceiptRequest.DataTransferIsUsingFullClaimVersioning = True
            claimReceiptResponse = DirectCast(coreSAMBusiness.ClaimReceipt(con, claimReceiptRequest), BaseClaimResponseType)

            claim.SiriusBaseClaimKey = claimReceiptResponse.BaseClaimKey
            claim.SiriusClaimKey = claimReceiptResponse.ClaimKey
            claim.ClaimNumber = claimReceiptResponse.ClaimNumber
            claim.VersionId = claimReceiptResponse.Version

            ' TODO : MEVANS : Process Claim Payment Reinsurance : Plug in Function When RTaylor Provides It
            Debug.Print("Called Process Claim Receipt for claim number : " & claim.ClaimNumber & " claim_Id : " & claim.SiriusClaimKey.ToString & " claim peril id : " & claimReceiptRequest.ClaimReceipt.BaseClaimPerilKey.ToString)
        End Sub
        ''' <summary>
        ''' ProcessClaimReceipt
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="coreSAMBusiness"></param>
        ''' <param name="branchCode"></param>
        ''' <param name="claim"></param>
        ''' <param name="claimPeril"></param>
        ''' <param name="claimReceipt"></param>
        ''' <remarks></remarks>
        Private Sub ProcessClaimReceipt(ByVal con As SiriusConnection, _
        ByVal coreSAMBusiness As CoreSAMBusiness, _
        ByVal branchCode As String, _
        ByVal claim As BaseCDTClaimType, _
        ByVal claimPeril As BaseCDTClaimPerilType, _
        ByVal claimReceipt As BaseCDTClaimReceiptType)

            Dim claimReceiptRequest As New BaseClaimReceiptRequestType
            Dim claimReceiptResponse As BaseClaimResponseType

            BuildClaimReceiptRequest(claimReceiptRequest, branchCode, claim, claimPeril, claimReceipt)

            claimReceiptRequest.IsDataTransferClaim = True
            claimReceiptRequest.DataTransferClaimHasSpecifiedReinsurance = (claim.ClaimReinsuranceForDTU IsNot Nothing)
            claimReceiptRequest.DataTransferIsUsingFullClaimVersioning = False

            claimReceiptResponse = DirectCast(coreSAMBusiness.ClaimReceipt(con, claimReceiptRequest), BaseClaimResponseType)

            ' TODO : MEVANS : Process Claim Payment Reinsurance : Plug in Function When RTaylor Provides It
            Debug.Print("Called Process Claim Receipt for claim number : " & claim.ClaimNumber & " claim_Id : " & claim.SiriusClaimKey.ToString & " claim peril id : " & claimReceiptRequest.ClaimReceipt.BaseClaimPerilKey.ToString)

        End Sub
        ''' <summary>
        ''' ProcessClaimReinsurance
        ''' </summary>
        ''' <param name="con"></param>
        ''' <param name="coreSAMBusiness"></param>
        ''' <param name="claimReinsurance"></param>
        ''' <param name="claimKey"></param>
        ''' <param name="riskKey"></param>
        ''' <remarks></remarks>
        Private Sub ProcessClaimReinsurance(ByVal con As SiriusConnection, _
            ByVal coreSAMBusiness As CoreSAMBusiness, _
             ByVal claimReinsurance As BaseCDTClaimReinsuranceTypeForDTU,
            ByVal claimKey As Integer, _
            ByVal riskKey As Integer)
            Dim i As Integer = 0
            If claimReinsurance IsNot Nothing AndAlso claimReinsurance.ClaimRIArrangement IsNot Nothing Then
                For i = 0 To claimReinsurance.ClaimRIArrangement.Count - 1
                    claimReinsurance.ClaimRIArrangement(i).ClaimKey = claimKey
                    claimReinsurance.ClaimRIArrangement(i).RiskKey = riskKey

                    coreSAMBusiness.SaveClaimRIArrangement(con, claimReinsurance.ClaimRIArrangement(i))
                Next
            End If

        End Sub

        Private Sub ProcessClaimRiskData(ByVal con As SiriusConnection,
        ByVal coreSAMBusiness As CoreSAMBusiness,
        ByVal claim As BaseCDTClaimType)

            ' only save the claim risk when risk data has been specified
            ' otherwise it will use the default
            If Not String.IsNullOrEmpty(claim.XMLDATASET) Then
                coreSAMBusiness.SaveClaimRiskData(con, claim, claim.XMLDATASET)
            End If

        End Sub

        Private Function ToBaseImpBaseClaimReceiptItemType( _
        ByVal fromType As BaseCDTReceiptItemType) _
            As BaseClaimReceiptItemType

            Dim toType As New BaseImplementationTypes.BaseClaimReceiptItemType

            If fromType IsNot Nothing Then

                toType.ReceiptAmount = fromType.ReceiptAmount
                toType.TaxGroupCode = fromType.TaxGroupCode
                toType.BaseRecoveryKey = fromType.SiriusBaseRecoveryKey
                toType.RecoveryTypeCode = fromType.RecoveryTypeCode
            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimPaymentItemType(
        ByVal fromType As BaseCDTClaimPaymentItemType) _
            As BaseClaimPaymentItemType

            Dim toType As New BaseImplementationTypes.BaseClaimPaymentItemType

            If fromType IsNot Nothing Then

                toType.PaymentAmount = fromType.PaymentAmount
                toType.ReverseExcess = fromType.ReverseExcess
                toType.TaxGroupCode = fromType.TaxGroupCode
                toType.BaseReserveKey = fromType.SiriusBaseReserveKey

            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimPerilRecoveryType(
        ByVal fromType As BaseCDTRecoveryType) _
            As BaseClaimPerilRecoveryType

            Dim toType As New BaseImplementationTypes.BaseClaimPerilRecoveryType

            If fromType IsNot Nothing Then

                toType.RevisionAmount = fromType.RevisionAmount
                toType.SamStagingRecoveryKey = fromType.SAMStagingRecoveryKey
                toType.TypeCode = fromType.TypeCode

            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimPerilReserveType(
        ByVal fromType As BaseCDTReserveType) _
            As BaseClaimPerilReserveType

            Dim toType As New BaseImplementationTypes.BaseClaimPerilReserveType

            If fromType IsNot Nothing Then

                toType.RevisionAmount = fromType.RevisionAmount
                toType.SamStagingReserveKey = fromType.SAMStagingReserveKey

                toType.TypeCode = fromType.TypeCode

            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimPerilType(ByVal fromType As BaseCDTClaimPerilType) _
            As BaseClaimPerilType

            Dim toType As New BaseImplementationTypes.BaseClaimPerilType

            If fromType IsNot Nothing Then

                toType.Description = fromType.Description
                toType.SamStagingClaimPerilKey = fromType.SAMStagingClaimPerilKey
                toType.TypeCode = fromType.TypeCode

                If fromType.Recovery IsNot Nothing Then
                    toType.Recovery = Array.ConvertAll(fromType.Recovery,
                                            New Converter(Of BaseImplementationTypes.BaseCDTRecoveryType, 
                                            BaseImplementationTypes.BaseClaimPerilRecoveryType) _
                                            (AddressOf ToBaseImpBaseClaimPerilRecoveryType))
                End If

                If fromType.Reserve IsNot Nothing Then
                    toType.Reserve = Array.ConvertAll(fromType.Reserve,
                                        New Converter(Of BaseImplementationTypes.BaseCDTReserveType, 
                                        BaseImplementationTypes.BaseClaimPerilReserveType) _
                                        (AddressOf ToBaseImpBaseClaimPerilReserveType))
                End If

            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimPerilMaintainType(ByVal fromType As BaseCDTClaimPerilType) _
            As BaseClaimPerilMaintainType

            Dim toType As New BaseImplementationTypes.BaseClaimPerilMaintainType

            If fromType IsNot Nothing Then

                toType.Description = fromType.Description
                toType.SamStagingClaimPerilKey = fromType.SAMStagingClaimPerilKey
                toType.TypeCode = fromType.TypeCode
                If fromType.SiriusBaseClaimPerilKey <> 0 Then
                    toType.BaseClaimPerilKey = fromType.SiriusBaseClaimPerilKey
                    toType.BaseClaimPerilKeySpecified = (fromType.SiriusBaseClaimPerilKey <> 0)
                Else
                    toType.BaseClaimPerilKey = GetDataTransferLink(fromType.SAMStagingBaseClaimPerilKey, DTLinkKey.ClaimPeril)
                    toType.BaseClaimPerilKeySpecified = (toType.BaseClaimPerilKey <> 0)
                End If
                If fromType.Recovery IsNot Nothing Then

                    toType.Recovery = Array.ConvertAll(fromType.Recovery,
                                            New Converter(Of BaseImplementationTypes.BaseCDTRecoveryType, 
                                            BaseImplementationTypes.BaseClaimPerilRecoveryType) _
                                            (AddressOf ToBaseImpBaseClaimPerilRecoveryType))
                End If

                If fromType.Reserve IsNot Nothing Then
                    toType.Reserve = Array.ConvertAll(fromType.Reserve,
                                        New Converter(Of BaseImplementationTypes.BaseCDTReserveType, 
                                        BaseImplementationTypes.BaseClaimPerilReserveType) _
                                        (AddressOf ToBaseImpBaseClaimPerilReserveType))
                End If

            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimOpenType(ByVal fromType As BaseCDTClaimType) _
            As BaseClaimOpenType

            Dim toType As New BaseImplementationTypes.BaseClaimOpenType

            If fromType IsNot Nothing Then

                toType.CatastropheCode = fromType.CatastropheCode
                toType.ClaimNumber = fromType.ClaimNumber

                toType.ClaimPeril = Array.ConvertAll(fromType.ClaimPeril,
                                        New Converter(Of BaseImplementationTypes.BaseCDTClaimPerilType, BaseImplementationTypes.BaseClaimPerilType) _
                                            (AddressOf ToBaseImpBaseClaimPerilType))

                toType.ClaimVersionDescription = fromType.ClaimVersionDescription
                toType.Comments = fromType.Comments
                toType.CurrencyCode = fromType.CurrencyCode
                toType.Description = fromType.Description
                toType.HandlerCode = fromType.HandlerCode
                toType.InfoOnly = fromType.InfoOnly
                toType.LikelyClaim = fromType.LikelyClaim
                toType.Location = fromType.Location
                toType.LossFromDate = fromType.LossFromDate
                toType.LossToDate = fromType.LossToDate
                toType.LossToDateSpecified = fromType.LossToDateSpecified
                toType.PrimaryCauseCode = fromType.PrimaryCauseCode
                toType.ProgressStatusCode = fromType.ProgressStatusCode
                toType.ReportedDate = fromType.ReportedDate
                toType.SamStagingClaimKey = fromType.SAMStagingClaimKey
                toType.SecondaryCauseCode = fromType.SecondaryCauseCode
                toType.InsuranceFileKey = fromType.SiriusInsuranceFileKey
                toType.RiskKey = fromType.SiriusRiskKey
                toType.TownCode = fromType.TownCode
                toType.UnderwritingYearCode = fromType.UnderwritingYearCode
                toType.IgnoreWarnings = True

            End If

            Return toType

        End Function

        Private Function ToBaseImpBaseClaimMaintainType(ByVal fromType As BaseCDTClaimType) _
                            As BaseClaimMaintainType

            Dim toType As New BaseImplementationTypes.BaseClaimMaintainType

            If fromType IsNot Nothing Then

                toType.CatastropheCode = fromType.CatastropheCode
                toType.ClaimNumber = fromType.ClaimNumber

                toType.ClaimPeril = Array.ConvertAll(fromType.ClaimPeril,
                                        New Converter(Of BaseImplementationTypes.BaseCDTClaimPerilType, BaseImplementationTypes.BaseClaimPerilMaintainType) _
                                            (AddressOf ToBaseImpBaseClaimPerilMaintainType))

                toType.ClaimVersionDescription = fromType.ClaimVersionDescription
                toType.Comments = fromType.Comments
                toType.CurrencyCode = fromType.CurrencyCode
                toType.Description = fromType.Description
                toType.HandlerCode = fromType.HandlerCode
                toType.InfoOnly = fromType.InfoOnly
                toType.LikelyClaim = fromType.LikelyClaim
                toType.Location = fromType.Location
                toType.LossFromDate = fromType.LossFromDate
                toType.LossToDate = fromType.LossToDate
                toType.LossToDateSpecified = fromType.LossToDateSpecified
                toType.PrimaryCauseCode = fromType.PrimaryCauseCode
                toType.ProgressStatusCode = fromType.ProgressStatusCode
                toType.ReportedDate = fromType.ReportedDate
                toType.SamStagingClaimKey = fromType.SAMStagingClaimKey
                toType.SecondaryCauseCode = fromType.SecondaryCauseCode
                toType.InsuranceFileKey = fromType.SiriusInsuranceFileKey
                toType.RiskKey = fromType.SiriusRiskKey
                toType.TownCode = fromType.TownCode
                toType.IgnoreWarnings = True

            End If

            Return toType

        End Function

    End Class

    Friend Class TransactionDateSort
        Implements IComparer(Of ClaimPaymentAndReceiptTransaction)

        Public Function Compare(ByVal x As ClaimPaymentAndReceiptTransaction, ByVal y As ClaimPaymentAndReceiptTransaction) As Integer Implements System.Collections.Generic.IComparer(Of ClaimPaymentAndReceiptTransaction).Compare
            If x.TransactionDate < y.TransactionDate Then
                Return -1
            ElseIf x.TransactionDate = y.TransactionDate Then
                Return 0
            Else
                Return 1
            End If
        End Function
    End Class

    Friend Class ClaimImportDataSort
        Implements IComparer(Of BaseCDTClaimType)

        Public Function Compare(ByVal x As BaseCDTClaimType, ByVal y As BaseCDTClaimType) As Integer Implements System.Collections.Generic.IComparer(Of BaseCDTClaimType).Compare

            If x.ClaimNumber = String.Empty Then
                Return -1
            ElseIf x.ClaimNumber <> y.ClaimNumber Then
                Return -1
            ElseIf x.ClaimNumber = y.ClaimNumber Then
                If x.TransactionDate < y.TransactionDate Then
                    Return -1
                ElseIf x.TransactionDate = y.TransactionDate Then
                    If x.VersionNo < y.VersionNo Then
                        Return -1
                    ElseIf x.VersionNo = y.VersionNo Then
                        Return 0
                    ElseIf x.VersionNo > y.VersionNo Then
                        Return 1
                    End If
                ElseIf x.TransactionDate > y.TransactionDate Then
                    Return 1
                End If
            End If

        End Function
    End Class

    Friend Class ClaimPaymentAndReceiptTransaction

        Private _claim As BaseCDTClaimType
        Public Property Claim() As BaseCDTClaimType
            Get
                Return _claim
            End Get
            Set(ByVal value As BaseCDTClaimType)
                _claim = value
            End Set
        End Property
        Private _claimPeril As BaseCDTClaimPerilType
        Public Property ClaimPeril() As BaseCDTClaimPerilType
            Get
                Return _claimPeril
            End Get
            Set(ByVal value As BaseCDTClaimPerilType)
                _claimPeril = value
            End Set
        End Property
        Private _claimPayment As BaseCDTClaimPaymentType
        Public Property ClaimPayment() As BaseCDTClaimPaymentType
            Get
                Return _claimPayment
            End Get
            Set(ByVal value As BaseCDTClaimPaymentType)
                _claimPayment = value
            End Set
        End Property
        Private _claimReceipt As BaseCDTClaimReceiptType
        Public Property ClaimReceipt() As BaseCDTClaimReceiptType
            Get
                Return _claimReceipt
            End Get
            Set(ByVal value As BaseCDTClaimReceiptType)
                _claimReceipt = value
            End Set
        End Property
        Private _transactionDate As Date
        Public Property TransactionDate() As Date
            Get
                Return _transactionDate
            End Get
            Set(ByVal value As Date)
                _transactionDate = value
            End Set
        End Property

    End Class

End Namespace
'End Namespace

