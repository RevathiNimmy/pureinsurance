Option Strict On
Option Explicit On

Imports dPMDAOBridge
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports SSP.Shared.gPMConstants
Imports SSP.Shared
Partial Public Class CoreSAMBusiness
    Private Const DateInFuture As Date = #1/1/3000#

    ''' <summary>  
    '''  This method is used to Add Paynow Receipt
    '''<param name="oRequest" type="BaseAddReceiptRequestType"></param>   
    '''<returns>BaseAddReceiptResponseType</returns>
    '''<remarks></remarks>  
    Public Function AddPayNowReceipt(ByVal oRequest As BaseAddReceiptRequestType) As BaseAddReceiptResponseType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                             _SiriusUser.Username, _SiriusUser.SourceID,
                                              _SiriusUser.LanguageID,
                                                  SiriusUserDefaults.AppName)
            Return AddPayNowReceipt(con, oRequest)

        End Using

    End Function
    ''' <summary>  
    '''  This method is used to Add Paynow Receipt
    '''<param name="con" type="SiriusConnection"></param>   
    '''<param name="oRequest" type="BaseAddReceiptRequestType"></param> 
    '''<returns>BaseAddReceiptResponseType</returns>

    Public Function AddPayNowReceipt(ByVal con As SiriusConnection,
        ByVal oRequest As BaseAddReceiptRequestType) As BaseAddReceiptResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseAddReceiptResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        If oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddPayNowReceiptRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddPayNowReceiptResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        If oRequest.PartyKey = 0 Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing,
                                        SAMInvalidData.MandatoryInputMissing.ToString,
                                        "PartyKey")
        End If
        '*******************
        ' STRUCTURE VALIDATION
        '*******************
        oRequest.Validate(CType(oSAMErrorCollection, Object))

        ' throw any structure related errors
        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' DATA VALIDATION
        '*******************
        ValidateAddPayNowReceiptRequestData(con, oBusiness, oSAMErrorCollection, oRequest)

        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        '***************************
        ' BUSINESS RULE VALIDATION
        '***************************

        '***************************
        ' PROCESS ADD AGENT RECEIPT
        '***************************
        Using con

            con.BeginTransaction()

            Try

                ' get account code for party
                GetAccountDetailsForParty(con,
                    oRequest.PartyKey,
                    oRequest.Receipt.AccountId,
                     oRequest.Receipt.AccountCode,
                     oRequest.Receipt.AccountShortCode)

                'Create the cash list for receipt  
                CreateAgentReceiptCashList(con, oRequest.Receipt, oRequest.SourceId)

                'Create the cash list item for receipt
                CreateAgentReceiptCashListItem(con, oRequest.Receipt)

                'Post the cashlistitem for receipt
                PostAgentReceiptCashListItem(con, oRequest.Receipt)

                con.CommitTransaction()

                Return oResponse

            Catch ex As Exception

                con.RollbackTransaction()

                Throw

            End Try

        End Using

    End Function

    ''' <summary>  
    '''  This method is used for Data validation for 'Branch Code' and 'Party Key'
    '''<param name="con" type="SiriusConnection"></param> 
    '''<param name="oBusiness" type="CoreBusiness"></param>   
    '''<param name="oSAMErrorCollection" type="SAMErrorCollection"></param>   
    '''<param name="oRequest" type="BaseAddReceiptRequestType"></param>  
    Private Sub ValidateAddPayNowReceiptRequestData(
    ByVal con As SiriusConnection,
    ByVal oBusiness As CoreBusiness,
    ByVal oSAMErrorCollection As SAMErrorCollection,
    ByVal oRequest As BaseAddReceiptRequestType)
        Dim sPartyKey As String

        ' source 
        oRequest.SourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
            PMLookupTable.Source, oRequest.BranchCode, "BranchCode", oSAMErrorCollection)

        sPartyKey = GetAndValidateDescriptionById(con, "Party", "party_cnt", "party_cnt", oRequest.PartyKey.ToString)

        ValidateBaseReceiptTypeData(con, oBusiness, oSAMErrorCollection, oRequest.Receipt)

    End Sub

    Public Function AddAgentReceipt(ByVal oRequest As BaseAddReceiptRequestType) As BaseAddReceiptResponseType

        'Rk modifies as part of SAM SFI Interop conversion as required by SSP
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                             _SiriusUser.Username, _SiriusUser.SourceID,
                                              _SiriusUser.LanguageID,
                                                  SiriusUserDefaults.AppName)

            Return AddAgentReceipt(con, oRequest)

        End Using

    End Function

    Public Function AddAgentReceipt(ByVal con As SiriusConnection,
    ByVal oRequest As BaseAddReceiptRequestType) As BaseAddReceiptResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseAddReceiptResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        If oRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.AddAgentReceiptRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.AddAgentReceiptResponseType
        ElseIf oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.AddAgentReceiptRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.AddAgentReceiptResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        '*******************
        ' STRUCTURE VALIDATION
        '*******************
        oRequest.Validate(CType(oSAMErrorCollection, Object))

        ' throw any structure related errors
        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' DATA VALIDATION
        '*******************
        ValidateAddAgentRequestData(con, oBusiness, oSAMErrorCollection, oRequest)

        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        '***************************
        ' BUSINESS RULE VALIDATION
        '***************************

        ' validate that the user calling this routine has a valid association with a live agent
        ' before continuing with the request
        If Not oBusiness.CheckAgentKey(oRequest.Receipt.AgentKey) Then
            oSAMErrorCollection.AddBusinessRule(
                SAMBusinessErrors.AgentOnlyMethodUserIsNotAValidAgent,
                SAMBusinessErrors.AgentOnlyMethodUserIsNotAValidAgent.ToString)
            oSAMErrorCollection.CheckForErrors()
        End If

        '***************************
        ' PROCESS ADD AGENT RECEIPT
        '***************************
        Using con

            con.BeginTransaction()

            Try

                ' get account code for party
                GetAccountDetailsForParty(con,
                    _SiriusUser.PartyCnt,
                    oRequest.Receipt.AccountId,
                     oRequest.Receipt.AccountCode,
                     oRequest.Receipt.AccountShortCode)

                'Previously, methods for handling reciept had generic names such as create cashlist. Now new methods created to
                'handle reciepts and create cashlist method that handled reciept now changed to handle payments. So calls to 
                'those functions changed accordingly. 
                'Create the cash list for receipt  
                CreateAgentReceiptCashList(con, oRequest.Receipt, oRequest.SourceId)

                'Create the cash list item for receipt
                CreateAgentReceiptCashListItem(con, oRequest.Receipt)

                'Post the cashlistitem for receipt
                PostAgentReceiptCashListItem(con, oRequest.Receipt)

                con.CommitTransaction()

                Return oResponse

            Catch ex As Exception

                con.RollbackTransaction()

                Throw

            End Try

        End Using

    End Function

    Private Sub ValidateAddAgentRequestData(
    ByVal con As SiriusConnection,
    ByVal oBusiness As CoreBusiness,
    ByVal oSAMErrorCollection As SAMErrorCollection,
    ByVal oRequest As BaseAddReceiptRequestType)

        ' source 
        oRequest.SourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
            PMLookupTable.Source, oRequest.BranchCode, "BranchCode", oSAMErrorCollection)

        ValidateBaseReceiptTypeData(con, oBusiness, oSAMErrorCollection, oRequest.Receipt)

    End Sub

    Private Sub ValidateBaseReceiptTypeData(
    ByVal con As SiriusConnection,
    ByVal oBusiness As CoreBusiness,
    ByVal oSAMErrorCollection As SAMErrorCollection,
    ByVal receipt As BaseReceiptType)

        ' currency 
        receipt.CurrencyId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.Currency, receipt.CurrencyCode, "CurrencyCode", oSAMErrorCollection)

        ' cashlistitem_receipt_type
        receipt.CashListItemReceiptTypeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
            PMLookupTable.CashListItemReceiptType, receipt.ReceiptTypeCode, "ReceiptTypeCode", oSAMErrorCollection)

        ' mediatype
        receipt.MediaTypeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
            PMLookupTable.MediaType, receipt.MediaTypeCode, "MediaTypeCode", oSAMErrorCollection)

        ' mediatype_issuer
        If Not String.IsNullOrEmpty(receipt.MediaTypeIssuerCode) Then
            receipt.MediaTypeIssuerId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.MediaTypeIssuer, receipt.MediaTypeIssuerCode, "MediaTypeIssuerCode", oSAMErrorCollection)
        End If

        ' country
        If Not String.IsNullOrEmpty(receipt.CountryCode) Then
            receipt.CountryId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.Country, receipt.CountryCode, "CountryCode", oSAMErrorCollection)
        End If

        'Payment Status Code
        If Not String.IsNullOrEmpty(receipt.PaymentStatusCode) Then
            receipt.PaymentStatusId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.CashListItemPaymentStatus, receipt.PaymentStatusCode, "PaymentStatusCode", oSAMErrorCollection)
        End If
        ' Payment Type Code
        If Not String.IsNullOrEmpty(receipt.PaymentTypeCode) Then
            receipt.PaymentTypeId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.CashListItemPaymentType, receipt.PaymentTypeCode, "PaymentTypeCode", oSAMErrorCollection)
        End If

        ' Alocation Status Code
        If Not String.IsNullOrEmpty(receipt.AllocationStatusCode) Then
            receipt.AllocationStatusId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                PMLookupTable.AllocationStatus, receipt.AllocationStatusCode, "AllocationStatusCode", oSAMErrorCollection)
        End If

        ' bankaccount 
        receipt.BankAccountId = GetAndValidateSpecifiedTableCode(con,
        NonPMLookupTable.BankAccount,
        NonPMLookupTableKeyFields.BankAccount,
        "bank_account_name",
        receipt.BankAccountName,
        oSAMErrorCollection, "BankAccountName")

        If receipt.CollectionDateSpecified = True Then
            If receipt.CollectionDate > Date.MinValue Then
                If receipt.CollectionDate > Today.Date Then
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CollectionDateError,
                                                                               SAMBusinessErrors.CollectionDateError.ToString,
                                                                               "Collection Date cannot be a Future date")
                    oSAMErrorCollection.CheckForErrors()
                End If
                If receipt.CollectionDate < Today.Date Then
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_CollectionDate_Override_Authority")
                        cmd.AddInParameter("@user_id", SqlDbType.Int).Value = _SiriusUser.UserID
                        Dim iCollectionDate As Integer
                        iCollectionDate = Cast.ToInt32(con.ExecuteScalar(cmd), 0)
                        If iCollectionDate = 1 Then
                            If String.IsNullOrEmpty(receipt.Comments) Then
                                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.MissingComment,
                                                          SAMBusinessErrors.MissingComment.ToString,
                                                          "Comments")
                                oSAMErrorCollection.CheckForErrors()

                            End If
                        Else
                            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.UserDontHaveCollectionDateOverrideAccess,
                                                                                      SAMBusinessErrors.UserDontHaveCollectionDateOverrideAccess.ToString,
                                                                                      "User Dont Access Have Access to Override Collection Date.")
                            oSAMErrorCollection.CheckForErrors()
                        End If
                    End Using
                End If
            End If
            If receipt.ChequeDateSpecified = True Then
                If receipt.ChequeDate > Date.MinValue Then
                    If receipt.ChequeDate > Today.Date OrElse receipt.ChequeDate > receipt.CollectionDate Then
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidChequeDate,
                                                                       SAMBusinessErrors.InvalidChequeDate.ToString,
                                                                       "ChequeDate Cannot Be a Future Date")
                        oSAMErrorCollection.CheckForErrors()
                    End If
                    Dim iDatediff As Integer
                    iDatediff = Cast.ToInt32(Informations.DateDiff(DateInterval.Day, receipt.ChequeDate, Today), 0)
                    If (iDatediff > 150) Then
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.InvalidChequeDate,
                                                                           SAMBusinessErrors.InvalidChequeDate.ToString,
                                                                           "ChequeDate Cannot be More Than 150 Days Old")
                        oSAMErrorCollection.CheckForErrors()
                    End If
                End If
            End If
        End If

        If Not String.IsNullOrEmpty(receipt.CCTypeCode) Then
            receipt.CCTypeID =
                oBusiness.GetAndValidateListItemFromCode(
                    Core.STSListType.PMLookup,
                    PMLookupTable.TypeOfCard,
                    receipt.CCTypeCode,
                    "TypeCode",
                    oSAMErrorCollection)
        End If
        If Not String.IsNullOrEmpty(receipt.CCCashListItemBankCode) Then
            receipt.ccCashListItemBankID =
                oBusiness.GetAndValidateListItemFromCode(
                    Core.STSListType.PMLookup,
                    PMLookupTable.CashListItemBank,
                    receipt.CCCashListItemBankCode,
                    "CashListItemBankCode",
                    oSAMErrorCollection)
        End If
        If Not (receipt.Bank Is Nothing) Then
            ValidateBaseBankReceiptTypeData(receipt.Bank, oSAMErrorCollection)
        End If

        'PMLookup - Sub Branch 
        If Not Information.IsNothing(receipt.SubbranchCode) AndAlso Not String.IsNullOrEmpty(receipt.SubbranchCode.ToString) Then
            receipt.SubBranchID = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup,
                                                                    PMLookupTable.SubBranch,
                                                                    receipt.SubbranchCode.ToString,
                                                                    "SubBranchCode",
                                                                    oSAMErrorCollection)
        End If

    End Sub

    ''' <summary>
    ''' Creates an agent receipt cashlist for this transaction based on the band account and currency code.
    ''' If a cashlist matching the given criteria already exists then it's id is returned so
    ''' the new cashlistitem can be appended to it's existing items.
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oReceipt">An object of BaseReceiptType class</param>
    Private Sub CreateAgentReceiptCashList(ByVal con As SiriusConnection,
                                           ByVal oReceipt As BaseReceiptType,
                                           ByVal sourceId As Integer)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_Receipt_CreateCashList")
            'As told by Gaurav, changing the parameter size from 10 to 60 because the undelying column in database is of type varchar(60)
            'cmd.AddInParameter("@Party_cnt", SqlDbType.Int).Value = 430
            cmd.AddInParameter("@bank_account_name", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oReceipt.BankAccountName, Nothing)
            cmd.AddInParameter("@currency_code", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.CurrencyCode, Nothing)
            cmd.AddInParameter("@username", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(_SiriusUser.Username, Nothing)
            cmd.AddInParameter("@cashlist_ref", SqlDbType.VarChar, 25).Value = Cast.DefaultIfNull(oReceipt.CashListRef, Nothing)
            cmd.AddOutParameter("@cashlist_id", SqlDbType.Int)
            cmd.AddInParameter("@company_id", SqlDbType.Int).Value = sourceId
            cmd.AddInParameter("@subbranch_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.SubBranchID, 0)
            con.ExecuteNonQuery(cmd)

            oReceipt.CashlistId = Cast.ToInt32(cmd.Parameters.Item("@cashlist_id").Value, 0)

        End Using

    End Sub

    ''' <summary>
    ''' Creates a cashlist item for agent receipt
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oReceipt">An object of BaseReceiptType class</param>
    Private Sub CreateAgentReceiptCashListItem(ByVal con As SiriusConnection,
                                               ByVal oReceipt As BaseReceiptType)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Import_Receipt_CreateCashListItem")

            cmd.AddOutParameter("@cashlistitem_id", SqlDbType.Int)
            cmd.AddOutParameter("@account_id", SqlDbType.Int)
            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = oReceipt.CashlistId
            cmd.AddInParameter("@receipt_type_code", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.ReceiptTypeCode, Nothing)
            cmd.AddInParameter("@mediatype_code", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.MediaTypeCode, Nothing)
            cmd.AddInParameter("@mediatype_issuer_code", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.MediaTypeIssuerCode, Nothing)
            cmd.AddInParameter("@account_code", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.AccountShortCode, Nothing)
            If oReceipt.TransactionDate <> Date.MinValue Then
                cmd.AddInParameter("@transaction_date", SqlDbType.DateTime).Value = oReceipt.TransactionDate
            Else
                cmd.AddInParameter("@transaction_date", SqlDbType.DateTime).Value = DBNull.Value
            End If
            cmd.AddInParameter("@amount", SqlDbType.Money).Value = oReceipt.Amount
            cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = Cast.DefaultIfNull(oReceipt.MediaReference, Nothing)
            cmd.AddInParameter("@our_ref", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(oReceipt.OurReference, Nothing)
            cmd.AddInParameter("@their_ref", SqlDbType.VarChar, 30).Value = Cast.DefaultIfNull(oReceipt.TheirReference, Nothing)
            cmd.AddInParameter("@contact_name", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(oReceipt.ContactName, Nothing)
            cmd.AddInParameter("@address1", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oReceipt.Address1, Nothing)
            cmd.AddInParameter("@address2", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oReceipt.Address2, Nothing)
            cmd.AddInParameter("@address3", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oReceipt.Address3, Nothing)
            cmd.AddInParameter("@address4", SqlDbType.VarChar, 60).Value = Cast.DefaultIfNull(oReceipt.Address4, Nothing)
            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.PostalCode, Nothing)
            cmd.AddInParameter("@country_code", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.CountryCode, Nothing)
            cmd.AddInParameter("@payment_name", SqlDbType.VarChar, 255).Value = GetPaymentName(oReceipt.ChequeName)
            If oReceipt.ChequeDate <> Date.MinValue Then
                cmd.AddInParameter("@cheque_date", SqlDbType.DateTime).Value = oReceipt.ChequeDate
            Else
                cmd.AddInParameter("@cheque_date", SqlDbType.DateTime).Value = DBNull.Value
            End If
            cmd.AddInParameter("@cc_name", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCName, Nothing)
            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = Cast.DefaultIfNull(oReceipt.CCNumber, Nothing)

            If (Not String.IsNullOrEmpty(oReceipt.CCExpiryDate)) Then
                cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.CCExpiryDate, Nothing)
            Else
                cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = DBNull.Value
            End If
            If (Not String.IsNullOrEmpty(oReceipt.CCStartDate)) Then
                cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.CCStartDate, Nothing)
            Else
                cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = DBNull.Value
            End If
            cmd.AddInParameter("@cc_issue", SqlDbType.VarChar, 2).Value = Cast.DefaultIfNull(oReceipt.CCIssue, Nothing)
            cmd.AddInParameter("@cc_pin", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.CCPin, Nothing)
            cmd.AddInParameter("@cc_auth_code", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCAuthCode, Nothing)
            cmd.AddInParameter("@cc_manual_auth_code", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCManualAuthCode, Nothing)
            cmd.AddInParameter("@cc_transaction_code", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(oReceipt.CCTransactionCode, Nothing)
            cmd.AddInParameter("@cc_customer", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCCustomer, Nothing)
            cmd.AddInParameter("@username", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(_SiriusUser.Username, Nothing)
            cmd.AddInParameter("@batch_id", SqlDbType.Int).Value = SqlInt32.Null

            If oReceipt.CollectionDate <> Date.MinValue Then
                cmd.AddInParameter("@collection_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(oReceipt.CollectionDate, DBNull.Value) ' DateTime.MinValue)
            Else
                cmd.AddInParameter("@collection_date", SqlDbType.DateTime).Value = DBNull.Value
            End If
            cmd.AddInParameter("@comments", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oReceipt.Comments, Nothing)

            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.PartyBankKey, 0)
            cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oReceipt.CCTrackingNumber, Nothing)


            cmd.AddInParameter("@cc_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.ccCashListItemBankID, 0)
            cmd.AddInParameter("@type_of_card_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.CCTypeID, 0)
            cmd.AddInParameter("@cc_trans_slip_no", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oReceipt.CCTransactionSlipNumber, Nothing)
            If oReceipt.Bank IsNot Nothing Then
                cmd.AddInParameter("@bank_code", SqlDbType.VarChar, 30).Value = Cast.NullIfDefault(oReceipt.Bank.BankCode, Nothing)
                cmd.AddInParameter("@bank_location", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oReceipt.Bank.BankLocation, Nothing)
                cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oReceipt.Bank.BankBranch, Nothing)
                cmd.AddInParameter("@chequetype_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.Bank.ChequeTypeID, 0)
                cmd.AddInParameter("@Cheque_clearing_type_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.Bank.ChequeClearingTypeID, 0)
            Else
                cmd.AddInParameter("@bank_code", SqlDbType.VarChar, 60).Value = SqlString.Null
                cmd.AddInParameter("@bank_location", SqlDbType.VarChar, 100).Value = SqlString.Null
                cmd.AddInParameter("@bank_branch", SqlDbType.VarChar, 50).Value = SqlString.Null
                cmd.AddInParameter("@chequetype_id", SqlDbType.Int).Value = SqlInt16.Null
                cmd.AddInParameter("@Cheque_clearing_type_id", SqlDbType.Int).Value = SqlInt16.Null
            End If
            cmd.AddInParameter("@cc_token_id", SqlDbType.VarChar, 50).Value = Cast.NullIfDefault(oReceipt.CCTrackingNumber, Nothing)
            cmd.AddInParameter("@cc_insurance_file_cnt", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.CCInsuranceFileCnt, Nothing)
            con.ExecuteNonQuery(cmd)

            oReceipt.CashListItemId = Cast.ToInt32(cmd.Parameters.Item("@cashlistitem_id").Value, 0)
            oReceipt.AccountId = Cast.ToInt32(cmd.Parameters.Item("@account_id").Value, 0)

        End Using

    End Sub

    ''' <summary>
    ''' Posts a cashlist item for agent receipt
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oReceipt">An object of BaseReceiptType class</param>
    Public Sub PostAgentReceiptCashListItem(ByVal con As SiriusConnection, _
                                            ByVal oReceipt As BaseReceiptType)

        Dim oCashListPost As bACTCashListPost.Automated = Nothing

        Dim lReturn As Integer

        Try

            ' instantiate the business object
            oCashListPost = New bACTCashListPost.Automated

            'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
            Dim oDatabaseObject As Object = Nothing
            If Not con Is Nothing Then
                oDatabaseObject = Nothing
                oDatabaseObject = con.PMDAODatabase
            End If

            ' initialise the business object
            lReturn = CInt(oCashListPost.Initialise( _
                                   _SiriusUser.Username, _
                                   _SiriusUser.Password, _
                                   _SiriusUser.UserID, _
                                   _SiriusUser.SourceID, _
                                   _SiriusUser.LanguageID, _
                                   _SiriusUser.CurrencyID, _
                                   1, _
                                   SiriusUserDefaults.AppName, _
                                   False, oDatabaseObject))

            If (lReturn <> PMEReturnCode.PMTrue) Then

                ' if the account processing fails then throw a business rule error
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString, _
                                                    "bACTCashListPost.Automated.Initialise")
                oSAMErrorCollection.CheckForErrors()

            End If

            ' set the process modes
            lReturn = oCashListPost.SetProcessModes(0, 0, 0, 0, Date.Today)
            If (lReturn <> PMEReturnCode.PMTrue) Then
                ' if the account processing fails then throw a business rule error
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString, _
                                                    "bACTCashListPost.Automated.SetProcessModes")
                oSAMErrorCollection.CheckForErrors()
            End If

            ' Post the unallocated cash
            lReturn = oCashListPost.PostUnallocatedCash( _
                v_vCashListID:=oReceipt.CashlistId, _
                v_vCashListItemID:=oReceipt.CashListItemId)

            If (lReturn <> PMEReturnCode.PMTrue) Then
                ' if the account processing fails then throw a business rule error
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.AccountsProcessingFailed, _
                                                    SAMBusinessErrors.AccountsProcessingFailed.ToString, _
                                                    "bACTCashListPost.Automated.PostUnallocatedCash")
                oSAMErrorCollection.CheckForErrors()
            End If

            oReceipt.TransactionId = oCashListPost.CashTransId

        Finally
            If oCashListPost IsNot Nothing Then
                oCashListPost.Dispose()
                oCashListPost = Nothing
            End If
        End Try

    End Sub

    Public Function GetClaimPerilDetails(ByVal con As SiriusConnection, _
    ByVal claimPerilKey As Integer) As Integer
        Dim baseClaimPerilKey As Integer
        GetClaimPerilDetails(con, claimPerilKey, 0, "", baseClaimPerilKey)
        Return baseClaimPerilKey
    End Function

    Private Sub GetClaimPerilDetails(ByVal con As SiriusConnection, _
    ByVal lClaimPerilKey As Integer, _
    ByRef lGisScreenId As Integer, _
    ByRef sTransactionTypeCode As String, _
    ByRef baseClaimPerilKey As Integer)

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_Get_Claim_Peril_Details")
            cmd.AddInParameter("@Claim_Peril_Id", SqlDbType.Int).Value = lClaimPerilKey
            dt = con.ExecuteDataTable(cmd)
        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim dr As DataRow = dt.Rows(0)

            lGisScreenId = Cast.ToInt32(dr.Item("gis_screen_id"), 0)
            sTransactionTypeCode = Cast.ToStringTrim(dr.Item("transaction_type_code"), String.Empty)
            baseClaimPerilKey = Cast.ToInt32(dr.Item("base_claim_peril_id"), 0)

        End If

    End Sub

    Public Function FindInsuranceFileForClaims(ByVal oRequest As BaseFindInsuranceFileRequestType) As BaseFindInsuranceFileResponseType

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            Return FindInsuranceFileForClaims(con, oRequest)

        End Using

    End Function

    Public Function FindInsuranceFileForClaims(ByRef con As SiriusConnection, _
    ByVal oRequest As BaseFindInsuranceFileRequestType) As BaseFindInsuranceFileResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BaseFindInsuranceFileResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        If oRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.FindInsuranceFileForClaimsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.FindInsuranceFileForClaimsResponseType
        ElseIf oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.FindInsuranceFileForClaimsRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.FindInsuranceFileForClaimsResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        '*******************
        ' STRUCTURE VALIDATION
        '*******************
        oRequest.Validate(CType(oSAMErrorCollection, Object))

        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' DATA VALIDATION
        '*******************

        ' validate the branch code
        oRequest.SourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, oRequest.BranchCode, "BranchCode", oSAMErrorCollection)

        ' validate the policy number 
        oRequest.InsuranceFileCnt = GetAndValidateSpecifiedTableCode(con, "Insurance_File", "Insurance_File_cnt", "Insurance_Ref", oRequest.InsuranceFileRef, oSAMErrorCollection, "InsuranceFileRef")

        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' PROCESS
        '*******************
        ' get insurance file cnt 
        Dim iInsuranceFileKey As Integer
        iInsuranceFileKey = GetInsuranceFileForClaimDate(con, oRequest)

        ' return the insurance file cnt
        oResponse.InsuranceFileKey = iInsuranceFileKey

        Return oResponse

    End Function

    Private Function GetInsuranceFileForClaimDate(ByVal con As SiriusConnection, _
    ByVal oRequest As BaseFindInsuranceFileRequestType) As Integer

        Dim dt As DataTable = Nothing
        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_GetPolicy_U")

            cmd.AddInParameter("@PolicyNo", SqlDbType.VarChar, 30).Value = oRequest.InsuranceFileRef
            cmd.AddInParameter("@PartyShortName", SqlDbType.VarChar, 20).Value = Nothing
            cmd.AddInParameter("@PostCode", SqlDbType.VarChar, 20).Value = Nothing
            cmd.AddInParameter("@PolicyStartDate", SqlDbType.DateTime).Value = SqlDateTime.Null
            cmd.AddInParameter("@PolicyEndDate", SqlDbType.DateTime).Value = SqlDateTime.Null
            cmd.AddInParameter("@InsuranceFileCnt", SqlDbType.Int).Value = SqlInt32.Null
            cmd.AddInParameter("@GISValue", SqlDbType.VarChar, 100).Value = Nothing
            cmd.AddInParameter("@ClaimDate", SqlDbType.DateTime).Value = oRequest.SearchDate
            cmd.AddInParameter("@SourceID", SqlDbType.Int).Value = oRequest.SourceId
            cmd.AddInParameter("@AgentKey", SqlDbType.Int).Value = oRequest.AgentKey
            If oRequest.MaxRowsToFetchSpecified = True Then
                cmd.AddInParameter("@MaxRowsToFetch", SqlDbType.Int).Value = oRequest.MaxRowsToFetch
            End If
            dt = con.ExecuteDataTable(cmd)

        End Using

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Return Cast.ToInt32(dt.Rows(0).Item("insfilecnt"), 0)
        Else
            Return 0
        End If

    End Function

    Public Function PostDocument(ByVal oRequest As BasePostDocumentRequestType) As BasePostDocumentResponseType

        'Rk modifies as part of SAM SFI Interop conversion as required by SSP
        Using con As SiriusConnection = New SiriusConnectionPMDAO( _
                                             _SiriusUser.Username, _SiriusUser.SourceID, _
                                              _SiriusUser.LanguageID, _
                                                  SiriusUserDefaults.AppName)
            Dim oResponse As New BasePostDocumentResponseType
            Try
                con.BeginTransaction()
                oResponse = PostDocument(con, oRequest)
                con.CommitTransaction()
            Catch
                con.RollbackTransaction()
                Throw
            End Try

            Return oResponse

        End Using

    End Function

    Public Function PostDocument(ByRef con As SiriusConnection, _
    ByVal oRequest As BasePostDocumentRequestType) As BasePostDocumentResponseType

        Dim oBusiness As New CoreBusiness
        Dim oResponse As New BaseImplementationTypes.BasePostDocumentResponseType
        Dim nTypeOfPackage As enumTypeOfPackage

        If oRequest.GetType Is GetType(SAMForInsuranceImplementationTypes.PostDocumentRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsurancePackage
            oResponse = New SAMForInsuranceImplementationTypes.PostDocumentResponseType
        ElseIf oRequest.GetType Is GetType(SAMForInsuranceV2ImplementationTypes.PostDocumentRequestType) Then
            nTypeOfPackage = enumTypeOfPackage.SAMForInsuranceV2Package
            oResponse = New SAMForInsuranceV2ImplementationTypes.PostDocumentResponseType
        Else
            nTypeOfPackage = enumTypeOfPackage.UnknownPackage
        End If

        Dim oSAMErrorCollection As New SAMErrorCollection

        '*******************
        ' STRUCTURE VALIDATION
        '*******************
        If oRequest.Transactions IsNot Nothing Then
            For Each oTransaction As BaseTransactionType In oRequest.Transactions()
                If String.IsNullOrEmpty(oTransaction.AccountCode) And oTransaction.PartyKey <> 0 Then
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Party_ShortName")
                        cmd.AddInParameter("@Party_Cnt", SqlDbType.Int).Value = oTransaction.PartyKey
                        oTransaction.AccountCode = Cast.ToString(con.ExecuteScalar(cmd), String.Empty)
                    End Using
                End If
            Next
        End If
        oRequest.Validate(CType(oSAMErrorCollection, Object))

        ' throw any structure related errors
        oSAMErrorCollection.CheckForErrors()

        '*******************
        ' DATA VALIDATION
        '*******************

        ValidatePostDocumentRequestData(con, oBusiness, oSAMErrorCollection, oRequest)

        ' throw any data related errors
        oSAMErrorCollection.CheckForErrors()

        '***************************
        ' BUSINESS RULE VALIDATION
        '***************************

        Const kMinNoOfTransactionsAllowed As Integer = 2
        ' validate that there are at least two transactions in the array
        ' as this is the minimum required to complete this process
        If oRequest.Transactions Is Nothing OrElse oRequest.Transactions.Length < kMinNoOfTransactionsAllowed Then
            oSAMErrorCollection.AddBusinessRule( _
                SAMBusinessErrors.LessThanTwoTransactionsInTransactionArray, _
                SAMBusinessErrors.LessThanTwoTransactionsInTransactionArray.ToString)
            oSAMErrorCollection.CheckForErrors()
        End If

        Dim crAmount As Decimal = 0
        For Each oTransaction As BaseTransactionType In oRequest.Transactions
            crAmount += oTransaction.Amount
        Next
        If crAmount <> 0 Then
            oSAMErrorCollection.AddBusinessRule( _
                SAMBusinessErrors.TransactionAmountsDoNotBalance, _
                SAMBusinessErrors.TransactionAmountsDoNotBalance.ToString)
            oSAMErrorCollection.CheckForErrors()
        End If
        'Rahul DTU Work Start
        Dim sInsuranceRef As String
        If oRequest.InsuranceFileKey <> 0 Then

            oRequest.InsuranceFileKey = GetAndValidateSpecifiedTableCode(con, _
                "insurance_file", _
                "insurance_file_cnt", _
                "insurance_file_cnt", _
                oRequest.InsuranceFileKey.ToString, _
                oSAMErrorCollection, "InsuranceFileKey")

            oSAMErrorCollection.CheckForErrors()

            Dim ds As DataSet = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Insurance_Ref")

                cmd.AddInParameter("@insurance_File_cnt", SqlDbType.Int).Value = oRequest.InsuranceFileKey

                ds = con.ExecuteDataSet(cmd, "InsuranceFileDetails")

                If ds.Tables("InsuranceFileDetails").Rows.Count <> 0 Then
                    sInsuranceRef = ds.Tables(0).Rows(0).Item("Insurance_Ref").ToString
                End If

            End Using

        End If
        'Rahul DTU Work End

        '***************************
        ' PROCESS ADD AGENT RECEIPT
        '***************************

        ' get the source and currency details for the document
        GetSourceDetails(oRequest.BranchCode, oRequest.SourceId, oRequest.CurrencyId)

        Dim aTransactions(oRequest.Transactions.GetUpperBound(0), 15) As Object

        ' initialise array position
        Dim iTransaction As Integer = 0

        ' populate transaction array
        For Each oTranaction As BaseTransactionType In oRequest.Transactions
            oTranaction.InsuranceRef = sInsuranceRef
            PopulateTransactionArray(iTransaction, aTransactions, oRequest.CurrencyId, oTranaction)
            ' increment array element
            iTransaction += 1
        Next

        Try

            ' add document
            oResponse.DocumentRef = AddDocumentTransactions(con, oRequest, aTransactions)

            Return oResponse

        Catch ex As Exception

            Throw

        End Try

    End Function

    Private Function AddDocumentTransactions( _
    ByVal con As SiriusConnection, _
    ByVal oRequest As BasePostDocumentRequestType, _
    ByVal aTransactions As Object) As String

        Dim oDocumentPost As bACTDocumentPost.Form = Nothing

        Try
            Dim lReturn As Integer

            ' instantiate the business object
            oDocumentPost = New bACTDocumentPost.Form

            'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
            Dim oDatabaseObject As Object = Nothing
            If Not con Is Nothing Then
                oDatabaseObject = con.PMDAODatabase
            End If

            ' initialise the object
            lReturn = CInt(oDocumentPost.Initialise( _
                                   _SiriusUser.Username, _
                                   _SiriusUser.Password, _
                                   _SiriusUser.UserID, _
                                   _SiriusUser.SourceID, _
                                   _SiriusUser.LanguageID, _
                                   _SiriusUser.CurrencyID, _
                                   1, _
                                   SiriusUserDefaults.AppName, _
                                   False, oDatabaseObject))

            If (lReturn <> PMEReturnCode.PMTrue) Then
                ' if the account processing fails then throw a business rule error
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString, _
                                                    "bACTDocumentPost.Form.Initialise")
                oSAMErrorCollection.CheckForErrors()
            End If

            ' set the process modes
            lReturn = oDocumentPost.SetProcessModes(0, 0, 0, 0, Date.Today)

            If (lReturn <> PMEReturnCode.PMTrue) Then
                ' if the account processing fails then throw a business rule error
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString, _
                                                    "bACTDocumentPost.Form.SetProcessModes")
                oSAMErrorCollection.CheckForErrors()
            End If

            Dim lDocumentId As Object = 0
            Dim sDocumentRef As String = String.Empty
            Dim DocumentDate As Date = Date.Today

            If IsArray(oRequest.Transactions) Then
                If oRequest.Transactions(0) IsNot Nothing Then
                    If oRequest.Transactions(0).TransactionDateSpecified = True Then
                        DocumentDate = oRequest.Transactions(0).TransactionDate
                    End If
                End If
            End If

            If (oRequest.InsuranceFileKey = 0) Then
                ' run the accounts process
                lReturn = oDocumentPost.AddDocumentTransactions( _
                               r_vDocumentID:=lDocumentId, _
                               v_lDocumentTypeId:=oRequest.DocumentTypeId, _
                               v_sBranchID:=Cast.ToString(oRequest.SourceId.ToString, String.Empty), _
                               v_sComment:=oRequest.Comment, _
                               v_dtDocumentDate:=DocumentDate, _
                               v_vDocSourceID:=oRequest.SourceId, _
                               v_sDocumentRef:=sDocumentRef, _
                               v_vOperatorID:=_SiriusUser.UserID, _
                               v_vTransArray:=CType(CObj(aTransactions), Object(,)))
            Else
                ' run the accounts process passing in the insurance file key
                lReturn = oDocumentPost.AddDocumentTransactions( _
                               r_vDocumentID:=lDocumentId, _
                               v_lDocumentTypeId:=oRequest.DocumentTypeId, _
                               v_sBranchID:=Cast.ToString(oRequest.SourceId.ToString, String.Empty), _
                               v_sComment:=oRequest.Comment, _
                               v_dtDocumentDate:=DocumentDate, _
                               v_vDocSourceID:=oRequest.SourceId, _
                               v_sDocumentRef:=sDocumentRef, _
                               v_vOperatorID:=_SiriusUser.UserID, _
                               v_vTransArray:=CType(CObj(aTransactions), Object(,)), _
                               v_lInsuranceFileCnt:=oRequest.InsuranceFileKey)
            End If

            If (lReturn <> PMEReturnCode.PMTrue) Then
                Dim oSAMErrorCollection As New SAMErrorCollection
                ' if the account processing fails then throw a business rule error
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.AddDocumentTransactionsFailed, _
                                                    SAMBusinessErrors.AddDocumentTransactionsFailed.ToString, _
                                                    "bACTDocumentPost.Form.AddDocumentTransactions")
                oSAMErrorCollection.CheckForErrors()
                Return String.Empty
            Else
                ' return the document reference code
                Return sDocumentRef
            End If

        Finally

            If oDocumentPost IsNot Nothing Then
                oDocumentPost.Dispose()
                oDocumentPost = Nothing
            End If
        End Try

    End Function

    Private Function PopulateTransactionArray(ByVal iTransaction As Integer, _
    ByVal aTransaction(,) As Object, _
    ByVal lDocumentCurrencyId As Integer, _
    ByVal oTransaction As BaseTransactionType) As Object

        Const kExchangeRate As Integer = 1

        aTransaction(iTransaction, TransArray.Account_id) = oTransaction.AccountId
        aTransaction(iTransaction, TransArray.Account) = String.Empty
        aTransaction(iTransaction, TransArray.Currency_id) = lDocumentCurrencyId
        aTransaction(iTransaction, TransArray.Currency) = String.Empty
        aTransaction(iTransaction, TransArray.Amount) = oTransaction.Amount
        aTransaction(iTransaction, TransArray.Currency_Rate) = kExchangeRate
        aTransaction(iTransaction, TransArray.Base_Amount) = oTransaction.Amount
        aTransaction(iTransaction, TransArray.AltRef) = oTransaction.Reference
        aTransaction(iTransaction, TransArray.Comment) = oTransaction.Comment
        aTransaction(iTransaction, TransArray.UnderwritingYear_id) = oTransaction.UnderwritingYearId
        aTransaction(iTransaction, TransArray.UnderwritingYear) = Cast.ToString(oTransaction.UnderwritingYearCode, String.Empty)
        aTransaction(iTransaction, TransArray.Department_id) = 0
        aTransaction(iTransaction, TransArray.Department) = String.Empty
        aTransaction(iTransaction, TransArray.Insurance_Ref) = oTransaction.InsuranceRef
        aTransaction(iTransaction, TransArray.Purchase_Order) = String.Empty
        aTransaction(iTransaction, TransArray.Purchase_Invoice) = String.Empty

        Return aTransaction

    End Function

    Private Sub ValidatePostDocumentRequestData( _
       ByVal con As SiriusConnection, _
       ByVal oBusiness As CoreBusiness, _
       ByVal oSAMErrorCollection As SAMErrorCollection, _
       ByVal oRequest As BasePostDocumentRequestType)

        ' source 
        oRequest.SourceId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
            PMLookupTable.Source, oRequest.BranchCode, "BranchCode", oSAMErrorCollection)

        If oRequest.DocumentTypeCode = String.Empty Then
            ' get the relevant code for the specified document type
            oRequest.DocumentTypeCode = GetDocumentTypeCode(oRequest.DocumentType)
        End If

        ' document type
        oRequest.DocumentTypeId = GetAndValidateSpecifiedTableCode(con, _
            NonPMLookupTable.DocumentType, _
            NonPMLookupTableKeyFields.DocumentType, _
            "Code", _
             oRequest.DocumentTypeCode, _
            oSAMErrorCollection, "DocumentType")

        If oRequest.Transactions Is Nothing Then
            oSAMErrorCollection.AddInvalidData(SAMInvalidData.MandatoryInputMissing, _
                                                      SAMInvalidData.MandatoryInputMissing.ToString, _
                                                      "Transactions")
        Else
            For Each oTransaction As BaseTransactionType In oRequest.Transactions
                ValidatePostDocumentTransaction(con, oBusiness, oSAMErrorCollection, oTransaction)
            Next
        End If

    End Sub

    Private Function GetDocumentTypeCode(ByVal iDocumentType As DocumentTypeType) As String

        Dim sDocumentTypeCode As String = String.Empty

        Select Case iDocumentType
            Case BaseImplementationTypes.DocumentTypeType.JN
                sDocumentTypeCode = BaseImplementationTypes.DocumentTypeType.JN.ToString
        End Select

        Return sDocumentTypeCode

    End Function

    Private Sub ValidatePostDocumentTransaction( _
              ByVal con As SiriusConnection, _
              ByVal oBusiness As CoreBusiness, _
              ByVal oSAMErrorCollection As SAMErrorCollection, _
              ByVal oTransaction As BaseTransactionType)

        If oTransaction IsNot Nothing Then

            ' amount
            If oTransaction.Amount = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMInvalidData.TransactionAmountIsZero, _
                                                   SAMInvalidData.TransactionAmountIsZero.ToString, _
                                                    "Amount", "0")
            End If

            ' validate when an underwriting year is passed in
            If Not String.IsNullOrEmpty(oTransaction.UnderwritingYearCode) Then
                ' underwriting year code
                oTransaction.UnderwritingYearId = oBusiness.GetAndValidateListItemFromCode(Core.STSListType.PMLookup, _
                    PMLookupTable.UnderwritingYear, _
                    oTransaction.UnderwritingYearCode, _
                    "UnderwritingYearCode", _
                    oSAMErrorCollection)
            End If

            ' account code 
            oTransaction.AccountId = GetAndValidateSpecifiedTableCode(con, _
            NonPMLookupTable.Account, _
            NonPMLookupTableKeyFields.Account, _
            "Short_Code", _
            oTransaction.AccountCode, _
            oSAMErrorCollection, "AccountCode")

        End If

    End Sub

    ''' <summary>
    ''' Creates a cashlist item for Refund Cashlist
    ''' </summary>
    '''<param name="con">An object of SiriusConnection class</param>
    '''<param name="oReceipt">An object of BaseReceiptType class</param>
    Private Sub CreateRefundPaymentCashListItem(ByVal con As SiriusConnection, _
                                               ByVal oReceipt As BaseReceiptType)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_CashListItem_Payment")

            cmd.AddOutParameter("@cashlistitem_id", SqlDbType.Int)

            cmd.AddInParameter("@cashlist_id", SqlDbType.Int).Value = oReceipt.CashlistId

            cmd.AddInParameter("@account_code", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.AccountShortCode, Nothing)
            cmd.AddInParameter("@transaction_date", SqlDbType.DateTime).Value = oReceipt.TransactionDate
            cmd.AddInParameter("@amount", SqlDbType.Money).Value = oReceipt.Amount
            cmd.AddInParameter("@media_ref", SqlDbType.VarChar, 100).Value = Cast.DefaultIfNull(oReceipt.MediaReference, Nothing)
            cmd.AddInParameter("@our_ref", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(oReceipt.OurReference, Nothing)
            cmd.AddInParameter("@their_ref", SqlDbType.VarChar, 30).Value = Cast.DefaultIfNull(oReceipt.TheirReference, Nothing)
            cmd.AddInParameter("@contact_name", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(oReceipt.ContactName, Nothing)
            cmd.AddInParameter("@address1", SqlDbType.VarChar, 40).Value = Cast.DefaultIfNull(oReceipt.Address1, Nothing)
            cmd.AddInParameter("@address2", SqlDbType.VarChar, 40).Value = Cast.DefaultIfNull(oReceipt.Address2, Nothing)
            cmd.AddInParameter("@address3", SqlDbType.VarChar, 40).Value = Cast.DefaultIfNull(oReceipt.Address3, Nothing)
            cmd.AddInParameter("@address4", SqlDbType.VarChar, 40).Value = Cast.DefaultIfNull(oReceipt.Address4, Nothing)
            cmd.AddInParameter("@postal_code", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.PostalCode, Nothing)
            cmd.AddInParameter("@address_country_id", SqlDbType.Int).Value = Cast.DefaultIfNull(oReceipt.CountryId, Nothing)
            cmd.AddInParameter("@payment_name", SqlDbType.VarChar, 60).Value = GetPaymentName(oReceipt.ChequeName)
            cmd.AddInParameter("@cc_name", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCName, Nothing)
            cmd.AddInParameter("@cc_number", SqlDbType.VarChar, 30).Value = Cast.DefaultIfNull(oReceipt.CCNumber, Nothing)
            cmd.AddInParameter("@cc_expiry_date", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.CCExpiryDate, Nothing)
            cmd.AddInParameter("@cc_start_date", SqlDbType.VarChar, 10).Value = Cast.DefaultIfNull(oReceipt.CCStartDate, Nothing)
            cmd.AddInParameter("@cc_issue", SqlDbType.VarChar, 2).Value = Cast.DefaultIfNull(oReceipt.CCIssue, Nothing)
            cmd.AddInParameter("@cc_pin", SqlDbType.VarChar, 20).Value = Cast.DefaultIfNull(oReceipt.CCPin, Nothing)
            cmd.AddInParameter("@cc_auth_code", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCAuthCode, Nothing)
            cmd.AddInParameter("@cc_manual_auth_code", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCManualAuthCode, Nothing)
            cmd.AddInParameter("@cc_transaction_code", SqlDbType.VarChar, 255).Value = Cast.DefaultIfNull(oReceipt.CCTransactionCode, Nothing)
            cmd.AddInParameter("@cc_customer", SqlDbType.VarChar, 50).Value = Cast.DefaultIfNull(oReceipt.CCCustomer, Nothing)
            cmd.AddInParameter("@pmuser_id", SqlDbType.Int).Value = Cast.DefaultIfNull(_SiriusUser.UserID, Nothing)
            cmd.AddInParameter("@batch_id", SqlDbType.Int).Value = SqlInt32.Null

            cmd.AddInParameter("@party_bank_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.PartyBankKey, 0)

            cmd.AddInParameter("@payment_account_code", SqlDbType.VarChar, 60).Value = SqlString.Null
            cmd.AddInParameter("@letter", SqlDbType.TinyInt).Value = SqlString.Null
            cmd.AddInParameter("@payment_branch_code", SqlDbType.VarChar, 30).Value = SqlString.Null
            cmd.AddInParameter("@payment_expiry_date", SqlDbType.DateTime).Value = SqlDateTime.Null
            cmd.AddInParameter("@payment_reference1", SqlDbType.VarChar, 30).Value = SqlString.Null
            cmd.AddInParameter("@payment_reference2", SqlDbType.VarChar, 30).Value = SqlString.Null
            cmd.AddInParameter("@payment_type_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.PaymentTypeId, 0)
            cmd.AddInParameter("@payment_status_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.PaymentStatusId, 0)
            cmd.AddInParameter("@allocationstatus_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.AllocationStatusId, 0)
            cmd.AddInParameter("@mediatype_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.MediaTypeId, 0)
            cmd.AddInParameter("@cc_tracking_number", SqlDbType.VarChar, 100).Value = Cast.NullIfDefault(oReceipt.CCTrackingNumber, Nothing)
            cmd.AddInParameter("@sBusinessIdentifierCode", SqlDbType.VarChar, 50).Value = SqlString.Null
            cmd.AddInParameter("@sInternationalBankAccountNumber", SqlDbType.VarChar, 50).Value = SqlString.Null

            con.ExecuteNonQuery(cmd)

            oReceipt.CashListItemId = Cast.ToInt32(cmd.Parameters.Item("@cashlistitem_id").Value, 0)

        End Using

    End Sub
    Private Sub CreateRefundPaymentCashList(ByVal con As SiriusConnection, _
                                       ByVal oReceipt As BaseReceiptType)

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Add_CashList")

            cmd.AddInParameter("@bankaccount_name", SqlDbType.VarChar, 25).Value = Cast.NullIfDefault(oReceipt.BankAccountName, Nothing)
            cmd.AddInParameter("@bankaccount_code", SqlDbType.VarChar, 25).Value = SqlString.Null
            cmd.AddInParameter("@currency_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.CurrencyId, 0)
            cmd.AddInParameter("@cashlisttype_id", SqlDbType.Int).Value = Cast.NullIfDefault(1, 0)
            cmd.AddInParameter("@cashliststatus_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.PaymentStatusId, 0)
            cmd.AddInParameter("@username", SqlDbType.VarChar, 255).Value = Cast.NullIfDefault(_SiriusUser.Username, Nothing)
            cmd.AddInParameter("@list_date", SqlDbType.DateTime).Value = Cast.NullIfDefault(Date.Today, Nothing)
            cmd.AddInParameter("@cashlist_ref", SqlDbType.VarChar, 25).Value = Cast.NullIfDefault(oReceipt.CashListRef, Nothing)
            cmd.AddOutParameter("@cashlist_id", SqlDbType.Int)
            cmd.AddInParameter("@subbranch_id", SqlDbType.Int).Value = Cast.NullIfDefault(oReceipt.SubBranchID, 0)
            con.ExecuteNonQuery(cmd)

            oReceipt.CashlistId = Cast.ToInt32(cmd.Parameters.Item("@cashlist_id").Value, 0)

        End Using

    End Sub

End Class

