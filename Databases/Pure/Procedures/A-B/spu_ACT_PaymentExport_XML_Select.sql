SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_PaymentExport_XML_Select'
GO

CREATE PROCEDURE spu_ACT_PaymentExport_XML_Select
 @batch_id INT,
 @bank_account_name VARCHAR(60) = '',
 @media_type_code VARCHAR(50) = '',
 @lead_days  VARCHAR(10) = '',
 @new_batch  SMALLINT = 0,
 @cashlistitem_payment_type VARCHAR(20) = NULL

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
DECLARE @total_amount_all_instalments numeric (19,4)
DECLARE @total_transactions INT
DECLARE @BatchStatus_Batch_Exported INT
-- initialisation
SET @total_amount_all_instalments = 0
SET @total_transactions = 0

SET @parameters = 'batch_id=' + Cast(@batch_id as varchar(10)) + ' bank_account_name=' + @bank_account_name + ' media_type_code=' + @media_type_code + ' lead_days=' + Cast(@lead_days as varchar(10))
SET @due_date = DATEADD(dd,ISNULL(0 - CAST(@lead_days as int),0), GETDATE())
  -- get required id constants
SELECT @BatchStatus_Batch_Exported = batchstatus_id FROM BatchStatus Where code = 'BE'
IF @bank_account_name <>''
    Select @BankAccount_id = BankAccount_id From bankAccount
        WHERE Bank_Account_Name = @bank_account_name OR Description  = @bank_account_name

Create Table #iFile
 (CashListItem_Id INT,
 Insurance_Ref varchar(30),
 ShortName varchar(30),
 cnt int default 0)

IF @new_batch = 1
BEGIN

DECLARE @sqlstatement NVARCHAR(4000)

  SELECT @sqlstatement ='UPDATE  CashListItem
  SET  batch_id = + ' + CONVERT(NVARCHAR,@batch_id) +
  ' WHERE cashlistitem_id IN
(
 SELECT Distinct CLI.cashlistitem_id

    FROM  CashListItem CLI

    JOIN cashList CL
       ON CLI.cashList_id = CL.cashList_id

    JOIN Currency C
       ON CL.currency_id = C.Currency_id

    JOIN PMUser U
       ON CLI.pmuser_id = u.user_id

    LEFT JOIN CashListItem_Payment_Type CLIPT
       ON CLI.cashlistitem_payment_type_id = CLIPT.cashlistitem_payment_type_id

    LEFT JOIN MediaType MT
       ON CLI.MediaType_ID = MT.MediaType_ID

    LEFT JOIN MediaType_Issuer MTI
       ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id

    LEFT JOIN BankAccount BA  --CashListItem_Bank CLIB
         ON CL.bankaccount_id = BA.bankaccount_id
    LEFT JOIN Account A
       ON CLI.account_id = A.account_id

    WHERE CLI.batch_id IS NULL AND
   (CLI.allocationstatus_id = 3 OR CLI.allocationstatus_id = 2) AND
   CL.Cashlisttype_id<>2 AND  CLI.cashlistitem_payment_status_id <> 3'

    IF @bank_account_name <>'' BEGIN
		SELECT @sqlstatement = @sqlstatement + ' AND BA.BankAccount_id = ' + CONVERT(NVARCHAR,@BankAccount_id)
    END

    IF @media_type_code <>'' BEGIN
		SELECT @sqlstatement = @sqlstatement +  ' AND MT.code = ''' + @media_type_code  + ''''
    END

    IF @lead_days <> 0 BEGIN
		SELECT @sqlstatement = @sqlstatement +  'AND CLI.transaction_date >= ''' + CONVERT(NVARCHAR,@due_date) + ''''
    END

    IF @cashlistitem_payment_type IS NOT NULL BEGIN
	 SELECT @sqlstatement = @sqlstatement + ' AND CLI.cashlistitem_payment_type_id in (
	 SELECT cashlistitem_payment_type_id
	 from cashlistitem_payment_type
	 where code =  ''' + @cashlistitem_payment_type +''')'
	END
 SELECT @sqlstatement = @sqlstatement +  ')'

 exec sp_executesql @sqlstatement

  SELECT @total_amount_all_instalments = SUM(amount) , @total_transactions = COUNT(*) from CashListItem where batch_id = @batch_id
  UPDATE Batch
  SET batchstatus_id = @BatchStatus_Batch_Exported,
  total_amount = @total_amount_all_instalments,
  total_transactions = @total_transactions
  WHERE batch_id = @batch_id

END

Insert INTO #iFile
 Select
        A.CashListItem_Id,
        MAX(I.Insurance_Ref),
  MAX(P.ShortName), COUNT( A.CashListItem_Id)
          FROM
              Insurance_File I
              INNER JOIN Document D
                  ON D.Insurance_File_Cnt=I.Insurance_File_Cnt
              INNER JOIN (Select Distinct CashListItem_Id, Document_Ref From AllocationDetail WHERE CashListItem_Id IS NOT NULL) A
                  ON A.Document_Ref = D.Document_Ref --A.TransDetail_Id=T.TransDetail_Id
              INNER JOIN Party P
                  ON P.Party_Cnt=I.Insured_Cnt
              INNER JOIN CashListItem cli on cli.cashlistitem_id=A.cashlistitem_id
              WHERE cli.batch_id=@batch_id
                 Group By  A.CashListItem_Id
                 Order By CashListItem_Id

 Update #iFile
 Set Insurance_Ref = '<Multiple Policies>', ShortName = ''
  Where cnt > 1

 Select @total_amount_all_instalments = SUM(amount) , @total_transactions = COUNT(*) from CashListItem where batch_id = @batch_id
 UPDATE Batch
  SET total_amount = @total_amount_all_instalments,
  total_transactions = @total_transactions
  WHERE batch_id = @batch_id

SELECT  1                As Tag,
 Null             As Parent,
 'http://www.siriusfs.com/SFI/Export/Payment_Export/20060420' As [EXPORT_HEADER!1!xmlns],
 'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],
 'http://www.siriusfs.com/SFI/Export/Payment_Export/20060420 Payment_Export.xsd'     As [EXPORT_HEADER!1!xsi:schemaLocation],
 GetDate()        As [EXPORT_HEADER!1!date_exported],
 'Payment_Export' As [EXPORT_HEADER!1!interface_name],
 @batch_id             As [EXPORT_HEADER!1!batch_id],
 b.batch_ref             As [EXPORT_HEADER!1!batch_reference],
 @total_transactions                    As [EXPORT_HEADER!1!total_transactions],
 @total_amount_all_instalments        As [EXPORT_HEADER!1!total_amount],
 @bank_account_name             As [EXPORT_HEADER!1!bank_account_name],
 @media_type_code            As [EXPORT_HEADER!1!media_type_code],
 @lead_days             As [EXPORT_HEADER!1!lead_days],
 --NULL          As [EXPORT_HEADER!1!merchant_id],
 Null          As [PAYMENT!2!cashlistitem_id],
 Null          As [PAYMENT!2!payment_type_code],
 Null          As [PAYMENT!2!media_type_code],
 Null          As [PAYMENT!2!mediatype_code_issuer],
 Null          As [PAYMENT!2!bankaccount_code],
 Null		   As [PAYMENT!2!bankaccount_number], --(Derick de Klerk) Adding User Bank Account Number
 Null		   As [PAYMENT!2!bankaccount_bic], --(Derick de Klerk) Adding User Bank Account BIC
 Null          As [PAYMENT!2!account_code],
 NULL          As [PAYMENT!2!cheque_number],
 Null          As [PAYMENT!2!our_reference],
 Null          As [PAYMENT!2!their_reference],
 Null          As [PAYMENT!2!transaction_date],
 Null          As [PAYMENT!2!cheque_date],
 Null          As [PAYMENT!2!currency_code],
 Null          As [PAYMENT!2!amount],
 Null          As [PAYMENT!2!contact_name],
 Null          As [PAYMENT!2!address1],
 Null          As [PAYMENT!2!address2],
 Null          As [PAYMENT!2!address3],
 Null          As [PAYMENT!2!address4],
 Null          As [PAYMENT!2!postal_code],
 Null          As [PAYMENT!2!payment_name],
 Null          As [PAYMENT!2!payment_account_code],
 Null          As [PAYMENT!2!sepa_payment_bic],
 Null          As [PAYMENT!2!sepa_payment_iban],
 Null          As [PAYMENT!2!payment_branch_code],
 Null		   As [PAYMENT!2!payment_account_type],  --(Derick de Klerk) Adding Client Bank Account Type
 Null		   As [PAYMENT!2!payment_bank_name],  --(Derick de Klerk) Adding Client Bank Name
 Null		   As [PAYMENT!2!payment_bank_country],  --(Derick de Klerk) Adding Client Bank Country
 Null          As [PAYMENT!2!payment_expiry_date],
 Null          As [PAYMENT!2!payment_reference1],
 Null          As [PAYMENT!2!payment_reference2],
 NULL          As [PAYMENT!2!cc_name],
 NULL          As [PAYMENT!2!cc_number],
 Null          As [PAYMENT!2!cc_expiry_date],
 Null          As [PAYMENT!2!cc_start_date],
 Null          As [PAYMENT!2!cc_issue],
 Null          As [PAYMENT!2!cc_pin],
 Null          As [PAYMENT!2!cc_auth_code],
 Null          As [PAYMENT!2!cc_manual_auth_code],
 Null          As [PAYMENT!2!cc_transaction_code],
 Null          As [PAYMENT!2!cc_customer],
 Null          As [PAYMENT!2!user_name],
 Null          As [CLAIM!3!loss_year],
 Null          As [CLAIM!3!claim_number],
 Null          As [PAYMENT!2!tax_number],
 Null          As [CLAIM!3!policy_number],
 Null          As [PAYMENT!2!payee_number],
 Null          As [PAYMENT!2!transdetail_id],
 Null          As [PAYMENT!2!merchant_id],
 --Start (Prakash C Varghese) - (Tech Spec  - CCR043 - Payment Export v0.01.doc) - (4.1.1)
 Null          As [PAYMENT!2!policy_number],
 Null          As [PAYMENT!2!policy_holder]
 --End (Prakash C Varghese) - (Tech Spec  - CCR043 - Payment Export v0.01.doc) - (4.1.1)
 FROM    batch b --WHERE b.batch_id = @batch_id

 WHERE   (ISNULL(b.batch_id,0) <> 0 and b.batch_id = @batch_id)

    UNION ALL

 SELECT 2                 As [Tag],
   1                 As [Parent],
 'http://www.siriusfs.com/SFI/Export/Payment_Export/20060420' As [EXPORT_HEADER!1!xmlns],
 'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],
 'http://www.siriusfs.com/SFI/Export/Payment_Export/20060420 Payment_Export.xsd'     As [EXPORT_HEADER!1!xsi:schemaLocation],
 GetDate()       As [EXPORT_HEADER!1!date_exported],
 'Payment_Export'     As [EXPORT_HEADER!1!interface_name],
 @batch_id             As [EXPORT_HEADER!1!batch_id],
 b.batch_ref             As [EXPORT_HEADER!1!batch_reference],
 @total_transactions                    As [EXPORT_HEADER!1!total_transactions],
 @total_amount_all_instalments        As [EXPORT_HEADER!1!total_amount],
 @bank_account_name             As [EXPORT_HEADER!1!bank_account_name],
 @media_type_code            As [EXPORT_HEADER!1!media_type_code],
 @lead_days             As [EXPORT_HEADER!1!lead_days],
 --AC.merchant_id  as [EXPORT_HEADER!1!merchant_id],
   CLI.cashlistitem_id     As [PAYMENT!2!cashlistitem_id],
   ISNULL(CLIPT.Code,'')    As [PAYMENT!2!payment_type_code],
   MT.Code        As [PAYMENT!2!media_type_code],
   MTI.Code        As [PAYMENT!2!media_type_code_issuer],
   ISNULL(BA.Code,'')  As [PAYMENT!2!bankaccount_code],
   ISNULL(BA.bank_account_no,'')  As [PAYMENT!2!bankaccount_number], --(Derick de Klerk) Adding User Bank Account Number
   ISNULL(BA.business_identifier_code,'')  As [PAYMENT!2!bankaccount_bic], --(Derick de Klerk) Adding User Bank Account BIC
   A.Short_Code      As [PAYMENT!2!account_code],
   CLI.Media_ref      As [PAYMENT!2!cheque_number],
   CLI.our_ref       As [PAYMENT!2!our_reference],
   CLI.their_ref      As [PAYMENT!2!their_reference],
   CLI.transaction_date    As [PAYMENT!2!transaction_date],
   CASE WHEN CLI.cheque_date = '1899-12-30T00:00:00' THEN CLI.transaction_date
 ELSE CLI.cheque_date END
     As [PAYMENT!2!cheque_date],  --currency_code missing
   C.ISO_code       As [PAYMENT!2!currency_code],
   CLI.amount       As [PAYMENT!2!amount],
   CLI.contact_name     As [PAYMENT!2!contact_name],
   CLI.address1      As [PAYMENT!2!address1],
   CLI.address2      As [PAYMENT!2!address2],
   CLI.address3      As [PAYMENT!2!address3],
   CLI.address4      As [PAYMENT!2!address4],
   CLI.postal_code      As [PAYMENT!2!postal_code],
   CLI.payment_name     As [PAYMENT!2!payment_name],
   CLI.payment_account_code   As [PAYMENT!2!payment_account_code],
   ISNULL(CLI.business_identifier_code,'')          As [PAYMENT!2!sepa_payment_bic],
   ISNULL(CLI.international_bank_account_number,'')          As [PAYMENT!2!sepa_payment_iban],
   CLI.payment_branch_code    As [PAYMENT!2!payment_branch_code],
   PB.account_type    As [PAYMENT!2!payment_account_type],  --(Derick de Klerk) Adding Client Bank Account Type
   CLIB.[description]    As [PAYMENT!2!payment_bank_name],  --(Derick de Klerk) Adding Client Bank Name
   Cty.[code]		   As [PAYMENT!2!payment_bank_country],  --(Derick de Klerk) Adding Client Bank Country
   CLI.payment_expiry_date    As [PAYMENT!2!payment_expiry_date],
   CLI.payment_reference1    As [PAYMENT!2!payment_reference1],
   CLI.payment_reference2    As [PAYMENT!2!payment_reference2], --WORKING FINE
   CLI.cc_name       As [PAYMENT!2!cc_name],
   CLI.cc_number      As [PAYMENT!2!cc_number],
   CLI.cc_expiry_date     As [PAYMENT!2!cc_expiry_date],
   CLI.cc_start_date     As [PAYMENT!2!cc_start_date],
   CLI.cc_issue      As [PAYMENT!2!cc_issue],
   CLI.cc_pin       As [PAYMENT!2!cc_pin],
   CLI.cc_auth_code     As [PAYMENT!2!cc_auth_code],
   CLI.cc_manual_auth_code    As [PAYMENT!2!cc_manual_auth_code],
   CLI.cc_transaction_code    As [PAYMENT!2!cc_transaction_code],
   CLI.cc_customer      As [PAYMENT!2!cc_customer],
   U.username       As [PAYMENT!2!user_name],
   null As [CLAIM!3!loss_year],
   null As [CLAIM!3!claim_number],
   Party.Tax_number As [PAYMENT!2!tax_number],
   null As [CLAIM!3!policy_number],
   A.short_code As [PAYMENT!2!payee_number],
   CLI.transdetail_id As [PAYMENT!2!transdetail_id],
   AC.merchant_id  as [PAYMENT!2!merchant_id],
   --Start (Prakash C Varghese) - (Tech Spec  - CCR043 - Payment Export v0.01.doc) - (4.1.1)
   #iFile.Insurance_Ref As [PAYMENT!2!policy_number],
   #iFile.ShortName As [PAYMENT!2!policy_holder]
   --End (Prakash C Varghese) - (Tech Spec  - CCR043 - Payment Export v0.01.doc) - (4.1.1)
   FROM  CashListItem CLI

   --FROM  batch b
   JOIN  Batch b
      ON CLI.batch_id = b.batch_id

   JOIN cashList CL
      ON CLI.cashList_id = CL.cashList_id

   JOIN Currency C
      ON CL.currency_id = C.Currency_id

   JOIN PMUser U
      ON CLI.pmuser_id = u.user_id

	LEFT JOIN Party_Bank PB --(Derick de Klerk) Joining Party_Bank for Bank Account Type
		ON PB.party_bank_id = CLI.party_bank_id
	LEFT JOIN Country Cty		--(Derick de Klerk) Joining Country to retrieve Bank Country
		ON Cty.country_id = PB.bank_country

   LEFT JOIN CashListItem_Bank CLIB	 --(Derick de Klerk) Joining CashListItem_Bank for Bank Name
		ON PB.bank_name_id = CLIB.cashlistitem_bank_id

   LEFT JOIN CashListItem_Payment_Type CLIPT
      ON CLI.cashlistitem_payment_type_id = CLIPT.cashlistitem_payment_type_id

   LEFT JOIN MediaType MT
      ON CLI.MediaType_ID = MT.MediaType_ID

   LEFT JOIN MediaType_Issuer MTI
      ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id

   LEFT JOIN Account A
      ON CLI.account_id = A.account_id

   LEFT JOIN bankaccount BA
      ON CL.bankaccount_id = BA.bankaccount_id

   JOIN account AC
      ON CLI.account_id = AC.account_id

   LEFT JOIN Party ON
      A.Account_Key = Party.Party_Cnt

   LEFT JOIN #iFile
       ON #iFile.CashListItem_Id=CLI.CashListItem_Id

   WHERE CLI.batch_id = @batch_id AND
         CL.Cashlisttype_id <> 2 AND
   (
    (@bank_account_name <>'' AND BA.BankAccount_id = @BankAccount_id)
   OR
    (@bank_account_name ='')
   )
   AND
   (
    (@media_type_code <>'' AND MT.code = @media_type_code)
   OR
    (@media_type_code ='')
   )
   AND
   (
    (@lead_days <> 0 AND CLI.transaction_date >= @due_date)
   OR
    (@lead_days =0)
   )

   --AND CLIPT.code <> 'CLP'

   UNION ALL

 SELECT distinct 3                 As [Tag],
   2                 As [Parent],
 'http://www.siriusfs.com/SFI/Export/Payment_Export/20060420' As [EXPORT_HEADER!1!xmlns],
 'http://www.w3.org/2001/XMLSchema-instance'      As [EXPORT_HEADER!1!xmlns:xsi],
 'http://www.siriusfs.com/SFI/Export/Payment_Export/20060420 Payment Export.xsd'     As [EXPORT_HEADER!1!xsi:schemaLocation],
 GetDate()       As [EXPORT_HEADER!1!date_exported],
 'Payment_Export'     As [EXPORT_HEADER!1!interface_name],
 @batch_id             As [EXPORT_HEADER!1!batch_id],
 b.batch_ref             As [EXPORT_HEADER!1!batch_reference],
 @total_transactions                    As [EXPORT_HEADER!1!total_transactions],
 @total_amount_all_instalments        As [EXPORT_HEADER!1!total_amount],
 @bank_account_name             As [EXPORT_HEADER!1!bank_account_name],
 @media_type_code            As [EXPORT_HEADER!1!media_type_code],
 @lead_days             As [EXPORT_HEADER!1!lead_days],
   CLI.cashlistitem_id     As [PAYMENT!2!cashlistitem_id],
   ISNULL(CLIPT.Code,'')    As [PAYMENT!2!payment_type_code],
   MT.Code        As [PAYMENT!2!media_type_code],
   MTI.Code        As [PAYMENT!2!media_type_code_issuer],
   ISNULL(BA.Code,'')  As [PAYMENT!2!bankaccount_code],
   ISNULL(BA.bank_account_no,'')  As [PAYMENT!2!bankaccount_number], --(Derick de Klerk) Adding User Bank Account Number
   ISNULL(BA.business_identifier_code,'')  As [PAYMENT!2!bankaccount_bic], --(Derick de Klerk) Adding User Bank Account BIC
   A.Short_Code      As [PAYMENT!2!account_code],
   CLI.Media_ref      As [PAYMENT!2!cheque_number],
   CLI.our_ref       As [PAYMENT!2!our_reference],
   CLI.their_ref      As [PAYMENT!2!their_reference],
   CLI.transaction_date    As [PAYMENT!2!transaction_date],
   CLI.cheque_date      As [PAYMENT!2!cheque_date],  --currency_code missing
   C.ISO_code       As [PAYMENT!2!currency_code],
   CLI.amount       As [PAYMENT!2!amount],
   CLI.contact_name     As [PAYMENT!2!contact_name],
   CLI.address1      As [PAYMENT!2!address1],
   CLI.address2      As [PAYMENT!2!address2],
   CLI.address3      As [PAYMENT!2!address3],
   CLI.address4      As [PAYMENT!2!address4],
   CLI.postal_code      As [PAYMENT!2!postal_code],
   CLI.payment_name     As [PAYMENT!2!payment_name],
   CLI.payment_account_code   As [PAYMENT!2!payment_account_code],
   ISNULL(CLI.business_identifier_code,'')          As [PAYMENT!2!sepa_payment_bic],
   ISNULL(CLI.international_bank_account_number,'')          As [PAYMENT!2!sepa_payment_iban],
   CLI.payment_branch_code    As [PAYMENT!2!payment_branch_code],
   PB.account_type    As [PAYMENT!2!payment_account_type],  --(Derick de Klerk) Adding Client Bank Account Type
   CLIB.[description]    As [PAYMENT!2!payment_bank_name],  --(Derick de Klerk) Adding Client Bank Name
   Cty.[code]		   As [PAYMENT!2!payment_bank_country],  --(Derick de Klerk) Adding Client Bank Country
   CLI.payment_expiry_date    As [PAYMENT!2!payment_expiry_date],
   CLI.payment_reference1    As [PAYMENT!2!payment_reference1],
   CLI.payment_reference2    As [PAYMENT!2!payment_reference2], --WORKING FINE
   CLI.cc_name       As [PAYMENT!2!cc_name],
   CLI.cc_number      As [PAYMENT!2!cc_number],
   CLI.cc_expiry_date     As [PAYMENT!2!cc_expiry_date],
   CLI.cc_start_date     As [PAYMENT!2!cc_start_date],
   CLI.cc_issue      As [PAYMENT!2!cc_issue],
   CLI.cc_pin       As [PAYMENT!2!cc_pin],
   CLI.cc_auth_code     As [PAYMENT!2!cc_auth_code],
   CLI.cc_manual_auth_code    As [PAYMENT!2!cc_manual_auth_code],
   CLI.cc_transaction_code    As [PAYMENT!2!cc_transaction_code],
   CLI.cc_customer      As [PAYMENT!2!cc_customer],
   U.username       As [PAYMENT!2!user_name],
   underwriting_year.code As [CLAIM!3!loss_year],
   claim.claim_number As [CLAIM!3!claim_number],
   Party.Tax_number As [PAYMENT!2!tax_number],
   claim.policy_number As [CLAIM!3!policy_number],
   A.short_code As [PAYMENT!2!payee_number],
   CLI.transdetail_id As [PAYMENT!2!transdetail_id],
   AC.merchant_id as  [PAYMENT!2!merchant_id],
   Null          As [PAYMENT!2!policy_number],
   Null          As [PAYMENT!2!policy_holder]
   FROM  CashListItem CLI

   LEFT JOIN Transdetail ON
    CLI.transdetail_id = transdetail.transdetail_id

   LEFT JOIN AllocationDetail ON
    allocationdetail.transdetail_id = transdetail.transdetail_id

   LEFT JOIN AllocationDetail allocationdetailmatched ON
        allocationdetail.allocation_id = allocationdetailmatched.allocation_id
   AND allocationdetail.transdetail_id <> allocationdetailmatched.transdetail_id
    AND ISNULL(allocationdetailmatched.is_primary, 0) = 1 -- Pull only primary transaction of other side otherwise will have duplicate records

   LEFT JOIN TransDetail transdetailmatched ON
  transdetailmatched.transdetail_id = allocationdetailmatched.transdetail_id

   LEFT JOIN Document ON
     transdetailmatched.document_id = document.document_id

   LEFT JOIN Claim_Payment ON
      claim_payment.document_id = document.document_id

    INNER JOIN (SELECT  policy_number,  claim_id,  version_Id, Underwriting_Year_Id,  claim_number FROM claim
          ) Claim ON    claim.claim_id = claim_payment.claim_id

       LEFT JOIN Underwriting_Year ON
        claim.underwriting_year_id = underwriting_year.underwriting_year_id

   --FROM  batch b
   JOIN  Batch b
      ON CLI.batch_id = b.batch_id

   JOIN cashList CL
      ON CLI.cashList_id = CL.cashList_id

   JOIN Currency C
      ON CL.currency_id = C.Currency_id

   JOIN PMUser U
      ON CLI.pmuser_id = u.user_id

   	LEFT JOIN Party_Bank PB --(Derick de Klerk) Joining Party_Bank for Bank Account Type
		ON PB.party_bank_id = CLI.party_bank_id
	LEFT JOIN Country Cty		--(Derick de Klerk) Joining Country to retrieve Bank Country
		ON Cty.country_id = PB.bank_country

   LEFT JOIN CashListItem_Bank CLIB	 --(Derick de Klerk) Joining CashListItem_Bank for Bank Name
		ON PB.bank_name_id = CLIB.cashlistitem_bank_id

   LEFT JOIN CashListItem_Payment_Type CLIPT
      ON CLI.cashlistitem_payment_type_id = CLIPT.cashlistitem_payment_type_id

   LEFT JOIN MediaType MT
      ON CLI.MediaType_ID = MT.MediaType_ID

   LEFT JOIN MediaType_Issuer MTI
      ON CLI.mediatype_issuer_id = MTI.mediatype_issuer_id

   LEFT JOIN BankAccount BA
      ON CL.bankaccount_id = BA.bankaccount_id

   JOIN account AC
      ON CLI.account_id = AC.account_id

   LEFT JOIN Account A
      ON CLI.account_id = A.account_id

   LEFT JOIN Party ON
     A.Account_Key = Party.Party_Cnt

   WHERE CLI.batch_id = @batch_id AND
         CL.Cashlisttype_id <> 2 AND
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
   AND
   (
    (@lead_days <> 0 AND CLI.transaction_date >= @due_date)
   OR
    (@lead_days =0)
   )
Order by
     [EXPORT_HEADER!1!batch_id],
     [PAYMENT!2!cashlistitem_id],
     [CLAIM!3!claim_number]
   --AND CLIPT.code = 'CLP'

  FOR XML EXPLICIT

  Drop Table #iFile

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

GO


