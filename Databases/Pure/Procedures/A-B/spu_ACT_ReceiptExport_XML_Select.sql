SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_ACT_ReceiptExport_XML_Select'
GO
create PROCEDURE spu_ACT_ReceiptExport_XML_Select   
 @batch_id INT,  
 @bank_account_name VARCHAR(60) = '',  
 @media_type_code VARCHAR(50) = '',  
 @new_batch  SMALLINT = 0  
AS  
  
    If (Select export_date From batch Where batch_id = @batch_id) Is Null Begin  
        Update  batch  
        Set     export_date = GetDate()  
        Where   batch_id = @batch_id  
    End Else Begin
        Update  batch  
        Set     reexport_date = GetDate()  
        Where   batch_id = @batch_id  
    End  
  
DECLARE @parameters varchar (300)  
DECLARE @due_date datetime  
DECLARE @cashListItemId INT  
DECLARE @BankAccount_id INT  
DECLARE @group_id int
  
SET @parameters = 'batch_id=' + Cast(@batch_id as varchar(10)) + ' bank_account_name=' + @bank_account_name + ' media_type_code=' + @media_type_code  
  
IF @bank_account_name <>''  
    Select @BankAccount_id = BankAccount_id From bankAccount  
        WHERE Bank_Account_Name = RTRIM(@bank_account_name) OR Description  = RTRIM(@bank_account_name)  
  
IF @new_batch = 1  
BEGIN  
 DECLARE Receipt_Export CURSOR FAST_FORWARD FOR  
 SELECT CLI.cashlistitem_id  
  
    FROM  CashListItem CLI  
  
    left JOIN cashList CL  
       ON CLI.cashList_id = CL.cashList_id  
  
    JOIN Currency C  
       ON CL.currency_id = C.Currency_id  
  
    JOIN PMUser U  
       ON CLI.pmuser_id = u.user_id  
  
    LEFT JOIN CashListItem_Receipt_Type CLIRT  
       ON CLI.cashlistitem_receipt_type_id = CLIRT.cashlistitem_receipt_type_id  
  
    LEFT JOIN MediaType MT  
       ON CLI.MediaType_ID = MT.MediaType_ID  
  
    LEFT JOIN MediaType_Issuer MTI  
       ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id  
    LEFT JOIN BankAccount BA  --CashListItem_Bank CLIB  
         ON CL.bankaccount_id = BA.bankaccount_id  
    LEFT JOIN Account A  
       ON CLI.account_id = A.account_id  
  
    WHERE  
  CLI.batch_id IS NULL  
 AND  
     (CLI.allocationstatus_id = 3 OR CLI.allocationstatus_id = 2) --Only allocated/Posted enteries will be exported  
 AND  
     CL.Cashlisttype_id=2  
 AND 
	 CLI.cashlistitem_Receipt_status_id<> 5  
 AND 
     (  
      (@bank_account_name <>'' AND BA.BankAccount_id = @BankAccount_id )  
     OR  
      (@bank_account_name ='')  
     )  
    AND  
     (  
      (@media_type_code <>'' AND MT.code = @media_type_code)  
     OR  
      (@media_type_code ='')  
     )  
  
 OPEN Receipt_Export  
  
 FETCH NEXT FROM Receipt_Export  
 INTO  
  @cashListItemId  
  
 WHILE @@FETCH_STATUS = 0  
 BEGIN  
  
  UPDATE  CashListItem  
  SET  batch_id = @batch_id  
  WHERE cashlistitem_id = @cashListItemId  

  --reset the group_id
  SET @group_id = 0

  IF EXISTS(
  SELECT PFI.pfinstalments_id
  FROM PFInstalments PFI INNER JOIN CashListItem_Instalments CLI_I
  ON PFI.pfinstalments_id = CLI_I.pfinstalments_id
  WHERE CLI_I.cashlistitem_id = @cashListItemId)

  -- set the value of @group_id if installment exists for the cashListItemId
  SELECT @group_id = MIN(PFI.pfinstalments_id)
  FROM PFInstalments PFI INNER JOIN CashListItem_Instalments CLI_I
  ON PFI.pfinstalments_id = CLI_I.pfinstalments_id
  WHERE CLI_I.cashlistitem_id = @cashListItemId

  -- Update the batch_id of installment if any for this cashlistitem_id
  UPDATE PFInstalments
  SET batch_id = @batch_id,
      group_id = @group_id
  WHERE pfinstalments_id IN (SELECT pfinstalments_id
                             FROM CashListItem_Instalments
                             WHERE cashlistitem_id = @cashListItemId)
  
   FETCH NEXT FROM Receipt_Export    INTO   @cashListItemId  
 END  
  
 CLOSE Receipt_Export  
 DEALLOCATE Receipt_Export  
  
END  
  
SELECT  1                As Tag,  
 Null             As Parent,  
 'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216' As [EXPORT_HEADER!1!xmlns],  
 'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],  
 'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216  
 Receipt_Export.xsd'     As [EXPORT_HEADER!1!xsi:schemaLocation],  
 'Receipt_Export' As [EXPORT_HEADER!1!interface_name],  
 @parameters As [EXPORT_HEADER!1!parameters_used],  
 @batch_id             As [EXPORT_HEADER!1!batch_id],  
 max(b.batch_ref)             As [EXPORT_HEADER!1!batch_reference],  
 GetDate()        As [EXPORT_HEADER!1!date_exported],  
 @bank_account_name             As [EXPORT_HEADER!1!bank_account_name],  
 @media_type_code            As [EXPORT_HEADER!1!media_type_code],  
 Null As [Bank_Account!2!account_name],  
 Null As [Bank_Account!2!account_description],  
 Null As [Bank_Account!2!account_code],  
 Null As [Bank_Account!2!account_number],  
 Null As [Bank_Account!2!sepa_account_bic], 
 Null As [Bank_Account!2!sepa_account_iban], 
 Null As [Bank_Account!2!financial_institution_code],  
 Null As [Bank_Account!2!direct_debit_supplier_name],  
 Null As [Bank_Account!2!direct_debit_supplier_id],  
 Null As [Bank_Account!2!processing_days],  
 Null As [Bank_Account!2!remitter],  
 Null As [Transaction!3!cashlistitem_id],  
 Null As [Transaction!3!amount],  
 Null As [Transaction!3!currency_code],  
 Null As [Transaction!3!transaction_date],  
 Null As [Transaction!3!receipt_type_code],    
 Null As [Transaction!3!media_type_code],    
 Null As [Transaction!3!media_reference],  
 Null As [Transaction!3!our_reference],  
 Null As [Transaction!3!their_reference],  
 Null As [Transaction!3!cheque_name],  
 Null As [Transaction!3!cheque_bank_code],  
 Null As [Transaction!3!cheque_date],  
 Null As [Transaction!3!account_name],  
 Null As [Transaction!3!account_sort_code],  
 Null As [Transaction!3!account_number],  
 Null As [Transaction!3!cc_name],  
 Null As [Transaction!3!cc_number],  
 Null As [Transaction!3!cc_expiry_date],  
 Null As [Transaction!3!cc_start_date],  
 Null As [Transaction!3!cc_issue],  
 Null As [Transaction!3!cc_pin],  
 Null As [Transaction!3!cc_auth_code],  
 Null As [Transaction!3!cc_manual_auth_code],  
 Null As [Transaction!3!cc_transaction_code],  
 Null As [Transaction!3!cc_customer],  
 Null As [Transaction!3!cc_tracking_number], 
 Null As [Transaction!3!contact_name],  
 Null As [Transaction!3!address1],  
 Null As [Transaction!3!address2],  
 Null As [Transaction!3!address3],  
 Null As [Transaction!3!address4],  
 Null As [Transaction!3!postal_code],  
 Null As [Transaction!3!country_code],  
 Null As [Transaction!3!further_details],  
 Null As [Transaction!3!user_name],  
 Null As [Transaction!3!transdetail_id],
 Null As [Transaction_Policy!4!policy_id],
 Null As [Transaction_Policy!4!insurance_ref],  
 Null AS [Transaction_Policy!4!client_name]
 FROM  
 batch b  
 INNER JOIN  
 CashListItem CLI  
 ON  
 b.batch_id = CLI.batch_id  
 WHERE  
 (ISNULL(b.batch_id,0) <> 0 and b.batch_id = @batch_id)  
-- group by b.batch_id  
  
 UNION ALL  
  
 SELECT 2                 As [Tag],  
   1                 As [Parent],  
  'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216' As [EXPORT_HEADER!1!xmlns],  
 'http://www.w3.org/2001/XMLSchema-instance' As [EXPORT_HEADER!1!xmlns:xsi],  
 'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216  
 Receipt_Export.xsd' As [EXPORT_HEADER!1!xsi:schemaLocation],  
 'Receipt_Export' As [EXPORT_HEADER!1!interface_name],  
 @parameters As [EXPORT_HEADER!1!parameters_used],  
 @batch_id As [EXPORT_HEADER!1!batch_id],  
 b.batch_ref As [EXPORT_HEADER!1!batch_reference],  
 GetDate() As [EXPORT_HEADER!1!date_exported],  
 @bank_account_name As [EXPORT_HEADER!1!bank_account_name],  
 @media_type_code As [EXPORT_HEADER!1!media_type_code],  
 BA.bank_account_name As [Bank_Account!2!account_name],  
 BA.description As [Bank_Account!2!account_description],  
 BA.code As [Bank_Account!2!account_code],  
 BA.bank_account_no As [Bank_Account!2!account_number],  
 ISNULL(BA.business_identifier_code,'') As [Bank_Account!2!sepa_account_bic],
 ISNULL(BA.international_bank_account_number,'') As [Bank_Account!2!sepa_account_iban],
 BA.financial_institution_code As [Bank_Account!2!financial_institution_code],  
 BA.direct_debit_supplier_name As [Bank_Account!2!direct_debit_supplier_name],  
 BA.direct_debit_supplier_id As [Bank_Account!2!direct_debit_supplier_id],  
 BA.processing_days As [Bank_Account!2!processing_days],  
 BA.remitter As [Bank_Account!2!remitter],  
 Null As [Transaction!3!cashlistitem_id],  
 Null As [Transaction!3!amount],  
 Null As [Transaction!3!currency_code],  
 Null As [Transaction!3!transaction_date],  
 Null As [Transaction!3!receipt_type_code],  
 Null As [Transaction!3!media_type_code],  
 Null As [Transaction!3!media_reference],  
 Null As [Transaction!3!our_reference],  
 Null As [Transaction!3!their_reference],  
 Null As [Transaction!3!cheque_name],  
 Null As [Transaction!3!cheque_bank_code],  
 Null As [Transaction!3!cheque_date],  
 Null As [Transaction!3!account_name],  
 Null As [Transaction!3!account_sort_code],  
 Null As [Transaction!3!account_number],  
 Null As [Transaction!3!cc_name],  
 Null As [Transaction!3!cc_number],  
 Null As [Transaction!3!cc_expiry_date],  
 Null As [Transaction!3!cc_start_date],  
 Null As [Transaction!3!cc_issue],  
 Null As [Transaction!3!cc_pin],  
 Null As [Transaction!3!cc_auth_code],  
 Null As [Transaction!3!cc_manual_auth_code],  
 Null As [Transaction!3!cc_transaction_code],  
 Null As [Transaction!3!cc_customer],  
 Null As [Transaction!3!cc_tracking_number],   
 Null As [Transaction!3!contact_name],  
 Null As [Transaction!3!address1],  
 Null As [Transaction!3!address2],  
 Null As [Transaction!3!address3],  
 Null As [Transaction!3!address4],  
 Null As [Transaction!3!postal_code],  
 Null As [Transaction!3!country_code],  
 Null As [Transaction!3!further_details],  
 Null As [Transaction!3!user_name],  
 Null As [Transaction!3!transdetail_id],  
 Null As [Transaction_Policy!4!policy_id],
 Null As [Transaction_Policy!4!insurance_ref],  
 Null AS [Transaction_Policy!4!client_name]
 FROM  
 Batch b  
 INNER JOIN  
 CashListItem CLI  
 ON  
 CLI.batch_id = b.batch_id  
 LEFT JOIN  
 MediaType MT  
 ON  
 CLI.MediaType_ID = MT.MediaType_ID  
 INNER JOIN  
 CashList CL  
 ON  
 CLI.cashlist_id = CL.cashlist_id  
 LEFT JOIN  
 BankAccount BA  
 ON  
 CL.bankaccount_id = BA.bankaccount_id  
   WHERE  
  CLI.batch_id = @batch_id  
 AND  
     (CLI.allocationstatus_id = 3 OR CLI.allocationstatus_id = 2) --Only allocated/Posted enteries will be exported  
 AND  
     CL.Cashlisttype_id=2  
 AND  
 	 CLI.cashlistitem_Receipt_status_id<> 5  
 AND
     (  
      (@bank_account_name <>'' AND BA.BankAccount_id = @BankAccount_id )  
     OR  
      (@bank_account_name ='')  
     )  
  
 AND  
     (  
      (@media_type_code <>'' AND MT.code = @media_type_code)  
     OR  
      (@media_type_code ='')  
     )  
  
 UNION ALL  
  
 SELECT 3                 As [Tag],  
   2                 As [Parent],  
   'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216' As [EXPORT_HEADER!1!xmlns],  
 'http://www.w3.org/2001/XMLSchema-instance' As [EXPORT_HEADER!1!xmlns:xsi],  
 'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216  
 Receipt_Export.xsd' As [EXPORT_HEADER!1!xsi:schemaLocation],  
 'Receipt_Export' As [EXPORT_HEADER!1!interface_name],  
 @parameters As [EXPORT_HEADER!1!parameters_used],  
 @batch_id As [EXPORT_HEADER!1!batch_id],  
 b.batch_ref As [EXPORT_HEADER!1!batch_reference],  
 GetDate() As [EXPORT_HEADER!1!date_exported],  
 @bank_account_name As [EXPORT_HEADER!1!bank_account_name],  
 @media_type_code As [EXPORT_HEADER!1!media_type_code],  
 BA.bank_account_name As [Bank_Account!2!account_name],  
 BA.description As [Bank_Account!2!account_description],  
 BA.code As [Bank_Account!2!account_code],  
 BA.bank_account_no As [Bank_Account!2!account_number],  
 ISNULL(BA.business_identifier_code,'') As [Bank_Account!2!sepa_account_bic],
 ISNULL(BA.international_bank_account_number,'') As [Bank_Account!2!sepa_account_iban],
 BA.financial_institution_code As [Bank_Account!2!financial_institution_code],  
 BA.direct_debit_supplier_name As [Bank_Account!2!direct_debit_supplier_name],  
 BA.direct_debit_supplier_id As [Bank_Account!2!direct_debit_supplier_id],  
 BA.processing_days As [Bank_Account!2!processing_days],  
 BA.remitter As [Bank_Account!2!remitter],  
 CLI.cashlistitem_id As [Transaction!3!cashlistitem_id],  
 CLI.amount As [Transaction!3!amount],  
 C.ISO_code As [Transaction!3!currency_code],  
 CLI.transaction_date As [Transaction!3!transaction_date],  
 CLIRT.code As [Transaction!3!receipt_type_code],  
 MT.code As [Transaction!3!media_type_code],  
 CLI.media_ref As [Transaction!3!media_reference],  
 CLI.our_ref As [Transaction!3!our_reference],  
 CLI.their_ref As [Transaction!3!their_reference],  
 CLI.payment_name As [Transaction!3!cheque_name],  
 CLIB.code As [Transaction!3!cheque_bank_code],  
 CLI.cheque_date As [Transaction!3!cheque_date],  
 ACC.payment_name As [Transaction!3!account_name],  
 ACC.payment_branch_code As [Transaction!3!account_sort_code],  
 ACC.payment_account_code As [Transaction!3!account_number],  
 CLI.cc_name As [Transaction!3!cc_name],  
 CLI.cc_number As [Transaction!3!cc_number],  
 CLI.cc_expiry_date As [Transaction!3!cc_expiry_date],  
 CLI.cc_start_date As [Transaction!3!cc_start_date],  
 CLI.cc_issue As [Transaction!3!cc_issue],  
 CLI.cc_pin As [Transaction!3!cc_pin],  
 CLI.cc_auth_code As [Transaction!3!cc_auth_code],  
 CLI.cc_manual_auth_code As [Transaction!3!cc_manual_auth_code],  
 CLI.cc_transaction_code As [Transaction!3!cc_transaction_code],  
 CLI.cc_customer As [Transaction!3!cc_customer],  
 CLI.cc_tracking_number As [Transaction!3!cc_tracking_number],
 CLI.contact_name As [Transaction!3!contact_name],  
 CLI.address1 As [Transaction!3!address1],  
 CLI.address2 As [Transaction!3!address2],  
 CLI.address3 As [Transaction!3!address3],  
 CLI.address4 As [Transaction!3!address4],  
 CLI.postal_code As [Transaction!3!postal_code],  
 CNTY.code As [Transaction!3!country_code],  
 CLI.receipt_details As [Transaction!3!further_details],  
 U.username As [Transaction!3!user_name],  
 CLI.transdetail_id As [Transaction!3!transdetail_id],  
 Null As [Transaction_Policy!4!policy_id],
 Null As [Transaction_Policy!4!insurance_ref],  
 Null AS [Transaction_Policy!4!client_name] 
 FROM  
 Batch b  
 INNER JOIN  
 CashListItem CLI  
 ON  
 CLI.batch_id = b.batch_id  
 INNER JOIN  
 CashList CL  
 ON  
 CLI.cashlist_id = CL.cashlist_id  
 LEFT JOIN  
 BankAccount BA  
 ON  
 CL.bankaccount_id = BA.bankaccount_id  
 INNER JOIN  
 Currency C  
 ON  
 CL.currency_id = C.Currency_id  
 LEFT JOIN  
 CashListItem_Receipt_Type CLIRT  
 ON  
 CLI.cashlistitem_receipt_type_id = CLIRT.cashlistitem_receipt_type_id  
 LEFT JOIN  
 MediaType MT  
 ON  
 CLI.MediaType_ID = MT.MediaType_ID  
 INNER JOIN  
 PMUser U  
 ON  
 CLI.pmuser_id = u.user_id  
 LEFT JOIN  
 Country CNTY  
 ON  
 CNTY.country_id = CLI.address_country  
 LEFT JOIN  
 CashListItem_Bank CLIB  
 ON  
 CLI.cashlistitem_bank_id = CLIB.cashlistitem_bank_id  
 LEFT JOIN  
 Account ACC  
 ON  
 CLI.account_id = ACC.account_id 

   WHERE  
  CLI.batch_id = @batch_id  
 AND  
     (CLI.allocationstatus_id = 3 OR CLI.allocationstatus_id = 2) --Only allocated/Posted enteries will be exported  
 AND  
     CL.Cashlisttype_id=2  
 AND  
 	 CLI.cashlistitem_Receipt_status_id<> 5  
 AND 
     (  
      (@bank_account_name <>'' AND BA.BankAccount_id = @BankAccount_id )  
     OR  
      (@bank_account_name ='')  
     )  
    AND  
     (  
      (@media_type_code <>'' AND MT.code = @media_type_code)  
     OR  
      (@media_type_code ='')  
     )  
UNION ALL    
	SELECT DISTINCT 4	As [Tag],  
   		3               As [Parent],  
   		'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216' As [EXPORT_HEADER!1!xmlns],  
 		'http://www.w3.org/2001/XMLSchema-instance' As [EXPORT_HEADER!1!xmlns:xsi],  
 		'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216  
 		Receipt_Export.xsd' As [EXPORT_HEADER!1!xsi:schemaLocation],  
 		'Receipt_Export' As [EXPORT_HEADER!1!interface_name],  
 		@parameters As [EXPORT_HEADER!1!parameters_used],  
 		@batch_id As [EXPORT_HEADER!1!batch_id],  
 		b.batch_ref As [EXPORT_HEADER!1!batch_reference],  
 		GetDate() As [EXPORT_HEADER!1!date_exported],  
 		@bank_account_name As [EXPORT_HEADER!1!bank_account_name],  
 		@media_type_code As [EXPORT_HEADER!1!media_type_code],  
 		BA.bank_account_name As [Bank_Account!2!account_name],  
 		BA.description As [Bank_Account!2!account_description],
 		BA.code As [Bank_Account!2!account_code],  
 		BA.bank_account_no As [Bank_Account!2!account_number],  
		ISNULL(BA.business_identifier_code,'') As [Bank_Account!2!sepa_account_bic],
		ISNULL(BA.international_bank_account_number,'') As [Bank_Account!2!sepa_account_iban],
 		BA.financial_institution_code As [Bank_Account!2!financial_institution_code],  
 		BA.direct_debit_supplier_name As [Bank_Account!2!direct_debit_supplier_name],  
 		BA.direct_debit_supplier_id As [Bank_Account!2!direct_debit_supplier_id],  
 		BA.processing_days As [Bank_Account!2!processing_days],  
 		BA.remitter As [Bank_Account!2!remitter],  
 		CLI.cashlistitem_id As [Transaction!3!cashlistitem_id],  
 		CLI.amount As [Transaction!3!amount],  
 		C.ISO_code As [Transaction!3!currency_code],  
 		CLI.transaction_date As [Transaction!3!transaction_date],  
 		CLIRT.code As [Transaction!3!receipt_type_code],  
 		MT.code As [Transaction!3!media_type_code],  
 		CLI.media_ref As [Transaction!3!media_reference],  
 		CLI.our_ref As [Transaction!3!our_reference],  
 		CLI.their_ref As [Transaction!3!their_reference],  
 		CLI.payment_name As [Transaction!3!cheque_name],  
 		CLIB.code As [Transaction!3!cheque_bank_code],  
 		CLI.cheque_date As [Transaction!3!cheque_date],  
 		ACC.payment_name As [Transaction!3!account_name],  
 		ACC.payment_branch_code As [Transaction!3!account_sort_code],  
 		ACC.payment_account_code As [Transaction!3!account_number],  
 		CLI.cc_name As [Transaction!3!cc_name],  
 		CLI.cc_number As [Transaction!3!cc_number],  
 		CLI.cc_expiry_date As [Transaction!3!cc_expiry_date],  
 		CLI.cc_start_date As [Transaction!3!cc_start_date],  
 		CLI.cc_issue As [Transaction!3!cc_issue],  
 		CLI.cc_pin As [Transaction!3!cc_pin],  
 		CLI.cc_auth_code As [Transaction!3!cc_auth_code],  
 		CLI.cc_manual_auth_code As [Transaction!3!cc_manual_auth_code],  
 		CLI.cc_transaction_code As [Transaction!3!cc_transaction_code],  
 		CLI.cc_customer As [Transaction!3!cc_customer],  
 		CLI.cc_tracking_number As [Transaction!3!cc_tracking_number],
 		CLI.contact_name As [Transaction!3!contact_name],  
 		CLI.address1 As [Transaction!3!address1],  
 		CLI.address2 As [Transaction!3!address2],  
 		CLI.address3 As [Transaction!3!address3],  
 		CLI.address4 As [Transaction!3!address4],  
 		CLI.postal_code As [Transaction!3!postal_code],  
 		CNTY.code As [Transaction!3!country_code],  
 		CLI.receipt_details As [Transaction!3!further_details],  
 		U.username As [Transaction!3!user_name],  
		CLI.transdetail_id As [Transaction!3!transdetail_id],  	
 		INF.insurance_file_cnt As [Transaction_Policy!4!policy_id],
   INF.insurance_ref As [Transaction_Policy!4!insurance_ref],
   PTY.ShortName AS [Transaction_Policy!4!client_name]  
 	FROM Batch b 
		INNER JOIN CashListItem CLI ON CLI.batch_id = b.batch_id  
 		INNER JOIN CashList CL ON CLI.cashlist_id = CL.cashlist_id  
		LEFT JOIN BankAccount BA ON CL.bankaccount_id = BA.bankaccount_id  
		INNER JOIN Currency C ON CL.currency_id = C.Currency_id  
		LEFT JOIN CashListItem_Receipt_Type CLIRT ON CLI.cashlistitem_receipt_type_id = CLIRT.cashlistitem_receipt_type_id  
  		LEFT JOIN MediaType MT ON CLI.MediaType_ID = MT.MediaType_ID  
		INNER JOIN PMUser U ON CLI.pmuser_id = u.user_id  
		LEFT JOIN Country CNTY ON CNTY.country_id = CLI.address_country  
		LEFT JOIN CashListItem_Bank CLIB ON CLI.cashlistitem_bank_id = CLIB.cashlistitem_bank_id  
		LEFT JOIN Account ACC ON CLI.account_id = ACC.account_id 
		INNER JOIN AllocationDetail AD ON AD.CashListItem_Id = CLI.CashListItem_Id
		INNER JOIN TransDetail TD ON TD.TransDetail_id =AD.TransDetail_id
		INNER JOIN Document D ON D.Document_id=TD.Document_id
		INNER JOIN Insurance_File INF ON D.Insurance_File_Cnt = INF.Insurance_File_Cnt 
  INNER JOIN Party PTY ON PTY.Party_Cnt=INF.Insured_Cnt 
	WHERE  
  		CLI.batch_id = @batch_id  
	AND  
		(CLI.allocationstatus_id = 3 OR CLI.allocationstatus_id = 2) --Only allocated/Posted enteries will be exported  
	AND  
		CL.Cashlisttype_id=2  
	AND  
		    CLI.cashlistitem_Receipt_status_id<> 5   
	AND
		(  
			(@bank_account_name <>'' AND BA.BankAccount_id = @BankAccount_id )  
			OR  
			(@bank_account_name ='')  
		)  
	    AND  
		(  
	    	(@media_type_code <>'' AND MT.code = @media_type_code)  
	    	OR  
	      	(@media_type_code ='')  
	    )

UNION ALL
SELECT DISTINCT 4	As [Tag],  
   		3               As [Parent],  
   		'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216' As [EXPORT_HEADER!1!xmlns],  
 		'http://www.w3.org/2001/XMLSchema-instance' As [EXPORT_HEADER!1!xmlns:xsi],  
 		'http://www.siriusfs.com/SFI/Export/Receipt_Export/20081216  
 		Receipt_Export.xsd' As [EXPORT_HEADER!1!xsi:schemaLocation],  
 		'Receipt_Export' As [EXPORT_HEADER!1!interface_name],  
 		@parameters As [EXPORT_HEADER!1!parameters_used],  
 		@batch_id As [EXPORT_HEADER!1!batch_id],  
 		b.batch_ref As [EXPORT_HEADER!1!batch_reference],  
 		GetDate() As [EXPORT_HEADER!1!date_exported],  
 		@bank_account_name As [EXPORT_HEADER!1!bank_account_name],  
 		@media_type_code As [EXPORT_HEADER!1!media_type_code],  
 		BA.bank_account_name As [Bank_Account!2!account_name],  
 		BA.description As [Bank_Account!2!account_description],  
 		BA.code As [Bank_Account!2!account_code],  
 		BA.bank_account_no As [Bank_Account!2!account_number],  
		ISNULL(BA.business_identifier_code,'') As [Bank_Account!2!sepa_account_bic],
		ISNULL(BA.international_bank_account_number,'') As [Bank_Account!2!sepa_account_iban],
 		BA.financial_institution_code As [Bank_Account!2!financial_institution_code],  
 		BA.direct_debit_supplier_name As [Bank_Account!2!direct_debit_supplier_name],  
 		BA.direct_debit_supplier_id As [Bank_Account!2!direct_debit_supplier_id],  
 		BA.processing_days As [Bank_Account!2!processing_days],  
 		BA.remitter As [Bank_Account!2!remitter],  
 		CLI.cashlistitem_id As [Transaction!3!cashlistitem_id],
 		CLI.amount As [Transaction!3!amount],  
 		C.ISO_code As [Transaction!3!currency_code],  
 		CLI.transaction_date As [Transaction!3!transaction_date],  
 		CLIRT.code As [Transaction!3!receipt_type_code],  
 		MT.code As [Transaction!3!media_type_code],  
 		CLI.media_ref As [Transaction!3!media_reference],  
 		CLI.our_ref As [Transaction!3!our_reference],  
 		CLI.their_ref As [Transaction!3!their_reference],  
 		CLI.payment_name As [Transaction!3!cheque_name],  
 		CLIB.code As [Transaction!3!cheque_bank_code],  
 		CLI.cheque_date As [Transaction!3!cheque_date],  
 		ACC.payment_name As [Transaction!3!account_name],  
 		ACC.payment_branch_code As [Transaction!3!account_sort_code],  
 		ACC.payment_account_code As [Transaction!3!account_number],  
 		CLI.cc_name As [Transaction!3!cc_name],  
 		CLI.cc_number As [Transaction!3!cc_number],  
 		CLI.cc_expiry_date As [Transaction!3!cc_expiry_date],  
 		CLI.cc_start_date As [Transaction!3!cc_start_date],  
 		CLI.cc_issue As [Transaction!3!cc_issue],  
 		CLI.cc_pin As [Transaction!3!cc_pin],  
 		CLI.cc_auth_code As [Transaction!3!cc_auth_code],  
 		CLI.cc_manual_auth_code As [Transaction!3!cc_manual_auth_code],  
 		CLI.cc_transaction_code As [Transaction!3!cc_transaction_code],  
 		CLI.cc_customer As [Transaction!3!cc_customer],  
 		CLI.cc_tracking_number As [Transaction!3!cc_tracking_number],
 		CLI.contact_name As [Transaction!3!contact_name],  
 		CLI.address1 As [Transaction!3!address1],  
 		CLI.address2 As [Transaction!3!address2],  
 		CLI.address3 As [Transaction!3!address3],  
 		CLI.address4 As [Transaction!3!address4],  
 		CLI.postal_code As [Transaction!3!postal_code],  
 		CNTY.code As [Transaction!3!country_code],  
 		CLI.receipt_details As [Transaction!3!further_details],  
 		U.username As [Transaction!3!user_name],  
		CLI.transdetail_id As [Transaction!3!transdetail_id],  	
 		INF.insurance_file_cnt As [Transaction_Policy!4!policy_id],
		INF.insurance_ref As [Transaction_Policy!4!insurance_ref],
		PTY.ShortName AS [Transaction_Policy!4!client_name] 
 	FROM Batch b 
		INNER JOIN CashListItem CLI ON CLI.batch_id = b.batch_id  
 		INNER JOIN CashList CL ON CLI.cashlist_id = CL.cashlist_id  
		LEFT JOIN BankAccount BA ON CL.bankaccount_id = BA.bankaccount_id  
		INNER JOIN Currency C ON CL.currency_id = C.Currency_id  
		LEFT JOIN CashListItem_Receipt_Type CLIRT ON CLI.cashlistitem_receipt_type_id = CLIRT.cashlistitem_receipt_type_id  
  		LEFT JOIN MediaType MT ON CLI.MediaType_ID = MT.MediaType_ID  
		INNER JOIN PMUser U ON CLI.pmuser_id = u.user_id  
		LEFT JOIN Country CNTY ON CNTY.country_id = CLI.address_country  
		LEFT JOIN CashListItem_Bank CLIB ON CLI.cashlistitem_bank_id = CLIB.cashlistitem_bank_id  
		LEFT JOIN Account ACC ON CLI.account_id = ACC.account_id 
		INNER JOIN CashListItem_Instalments CLIIN ON CLIIN.CashListItem_Id = CLI.CashListItem_Id
		INNER JOIN PFInstalments PFI ON PFI.PFInstalments_ID = CLIIN.PFInstalments_ID
		INNER JOIN PFPremiumFinance PFP ON PFP.PFPrem_Finance_cnt = PFI.PFPrem_Finance_cnt AND PFP.PFPrem_Finance_Version = PFI.PFPrem_Finance_Version
		INNER JOIN Insurance_File INF ON PFP.Insurance_File_cnt = INF.Insurance_File_Cnt 
		INNER JOIN Party PTY ON PTY.Party_Cnt=INF.Insured_Cnt
	WHERE  
  		CLI.batch_id = @batch_id  
	AND  
		(CLI.allocationstatus_id = 3 OR CLI.allocationstatus_id = 2)
	AND  
		CL.Cashlisttype_id=2  
	AND  
			CLI.cashlistitem_Receipt_status_id<> 5    
	AND 
		(  
			(@bank_account_name <>'' AND BA.BankAccount_id = @BankAccount_id )  
			OR  
			(@bank_account_name ='')  
		)  
	    AND  
		(  
	    	(@media_type_code <>'' AND MT.code = @media_type_code)  
	    	OR  
	      	(@media_type_code ='')  
	    )
ORDER BY  
	[EXPORT_HEADER!1!batch_id],  
        [Bank_Account!2!account_code],  
   	[Transaction!3!cashlistitem_id],  
	[Transaction_Policy!4!policy_id]
 FOR XML EXPLICIT  
  
 
SET QUOTED_IDENTIFIER ON  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
