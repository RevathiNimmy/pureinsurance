SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_ACT_Do_FindPayment'
GO
Create PROCEDURE spu_SAM_ACT_Do_FindPayment  
    @payee_name VARCHAR(60) = NULL,  
    @account_id INT = NULL,  
    @payment_type_id INT = NULL,  
    @payment_media_type_id INT = NULL,  
    @media_reference VARCHAR(60) = NULL,  
    @payment_status_id INT = NULL,  
    @amount NUMERIC(19,4) = NULL,  
    @batch_number INT = NULL,  
    @batch_ref VARCHAR(100) = NULL,  
    @user_id SMALLINT = NULL,  
    @MaxRowsToFetch INTEGER = 500,  
    @branch INTEGER = NULL,  
    @clientcode VARCHAR(50) = NULL,  
    @client_account_number VARCHAR(60) = NULL,  
    @policy_claim_number VARCHAR(60) = NULL,  
    @media_from VARCHAR(60) = NULL,  
    @media_to VARCHAR(60) = NULL,  
    @amount_from NUMERIC(19,4) = NULL,  
    @amount_to NUMERIC(19,4) = NULL,  
    @date_from DATETIME = NULL,  
    @date_to DATETIME = NULL,  
    @showonlyoutstanding INT = 0,  
    @bankaccountid VARCHAR(40) = NULL,
	@AgentKey INT=0 
AS  
  
UPDATE CHEQUE  
 SET media_ref = NULL  
WHERE media_ref = ''  
  
IF @media_from='' or @media_from = '0'  
    SET @media_from =NULL  
  
IF @media_To='' or @media_To ='0'  
    SET @media_To =NULL  
  
DECLARE @paymenttype_id INT,  
        @CODE Varchar(15),  
        @NumericSearch BIT,  
        @payment_status_id_UnissuedCheques INT,  
        @ResetMedia INT,  
 @sSql VARCHAR(8000),  
 @where VARCHAR(1000)  
  
SET NOCOUNT ON  
SET ROWCOUNT @MaxRowsToFetch  
  
 SELECT @CODE = mediatype_validation.code FROM  mediatype_validation  
 JOIN mediatype ON mediatype.mediatype_validation_id = mediatype_validation.mediatype_validation_id  
 WHERE mediatype.mediatype_id=Isnull(@payment_media_type_id,0)  
  
 IF @CODE='CHEQUE'  
  SET @NumericSearch = 1  
 ELSE  
  SET @NumericSearch = 0  
  
 IF  @NumericSearch = 0  
BEGIN  
 IF ISNUMERIC(@media_from)=1 AND ISNUMERIC(@media_to)=1  
  SET @NumericSearch = 1  
 ELSE  
  IF  ISNUMERIC(@media_from)=1 OR LTRIM(RTRIM(@media_to))=''  
   SET @NumericSearch = 1  
  ELSE  
   IF  LTRIM(RTRIM(@media_from))='' OR ISNUMERIC(@media_to)=1  
    SET @NumericSearch = 1  
   ELSE  
    SET @NumericSearch = 0  
END  
  
    Select @payment_status_id_UnissuedCheques =  cashlistitem_payment_status_id From cashlistitem_payment_status Where Code='UnISS'  
  
  CREATE TABLE #CashListDetails  
 (  
  cashlistitem_id BIGINT,  
  payment_name VARCHAR(100),  
  account_id BIGINT,  
  Payment_description VARCHAR(100),  
  Payment_code VARCHAR(100),  
  Media_description VARCHAR(100),  
  mediatype_id INT,  
  media_ref VARCHAR(100),  
  amount FLOAT,  
  Paymnent_Status_Description VARCHAR(100),  
  Paymnent_Status_code VARCHAR(100),  
  date_presented DATETIME,  
  batch_ref VARCHAR(100),  
  username VARCHAR(100),  
  cashlist_id VARCHAR(100),  
  short_code VARCHAR(100),  
  account_name VARCHAR(255),  
  PolicyNumber VARCHAR(50),  
  ClaimNumber VARCHAR(50),  
  their_ref VARCHAR(100),  
  our_ref VARCHAR(100),  
  transaction_date DATETIME,  
  bank_account_no VARCHAR(30),  
  document_ref VARCHAR(50),  
  Payment_type_description VARCHAR(100),  
  Sub_branch_code VARCHAR(10),  
  Bank_code VARCHAR(10),  
  CLI_reverse_reason_description VARCHAR(100),  
  reversed_date DATETIME,  
  cashlistitem_reverse_reason_id INT,  
  allow_reverse_allocations INT,  
  reverse_allocations_days INT,  
  transdetail_id INT,  
  Party_cnt INT,  
  insurance_file_cnt INT,  
  claim_id INT,  
  currency_id INT,  
  bank_Account_ID INT,  
  bank_reconciliation_date DATETIME,  
                Allocated BIT,  
                payment_account_code VARCHAR(60),  
                payment_branch_code VARCHAR(30),  
  ID INT IDENTITY  
 )  
  
 SELECT @sSql = 'INSERT INTO #CashListDetails  
      SELECT  
             I.cashlistitem_id,  
             I.payment_name,  
             A.account_id,  
             PT.description,  
             PT.code,  
             M.description,  
             M.mediatype_id,  
             I.media_ref,  
             I.amount,  
             PS.description,  
             PS.code,  
             I.date_presented,  
             B.batch_ref,  
             U.username,  
             I.cashlist_id,  
             P.shortname as short_code,  
             P.resolved_name as account_name,  
             '''','''',  
  
             I.their_ref,  
             I.our_ref,  
             I.transaction_date,  
             BA.bank_account_no,  
             D.document_ref,  
             CLIPT.description,  
             SB.code,  
             BK.code,  
             CLRR.description,  
             I.reversed_date,  
             I.cashlistitem_reverse_reason_id,  
             UA.allow_reverse_allocations,  
             UA.reverse_allocations_days,  
             TD.transdetail_id,  
             P.Party_cnt,  
             D.insurance_file_cnt,  
             CLM.claim_id,  
             C.currency_id,'  + CHAR(13) + CHAR(10)  
  IF  @NumericSearch = 0  
             SELECT @sSql = @sSql + ' NULL as bank_account_id, ' + CHAR(13) + CHAR(10)  
  Else  
      SELECT @sSql = @sSql + ' BA.Account_ID AS bank_account_id, ' + CHAR(13) + CHAR(10)  
  
      SELECT @sSql = @sSql + '(  
     SELECT MAX(TD4.bank_reconciliation_date) FROM transdetail TD4  
     LEFT JOIN document D4  
      ON TD4.document_id=D4.document_id  
     LEFT JOIN CashListItem I4  
      ON I4.TransDetail_ID = TD4.Transdetail_id  
     WHERE  
      TD4.document_id=D.document_id  
      AND ISNULL(TD4.transdetail_id,'''')<>ISNULL(I4.transdetail_id,'''')  
     ) AS bank_reconciliation_date,  
              Case When (Select Count(allocationdetail_id) From Allocationdetail Where Transdetail_Id = TD.Transdetail_id)>0  
                  Then 1  
              Else 0  
              end,  
              I.payment_account_code,  
              I.payment_branch_code  
  
      FROM CashListItem I  
    INNER JOIN CashList C  
        ON    I.cashlist_id = C.cashlist_id AND C.cashlisttype_id IN (1, 3)  
    LEFT JOIN TransDetail TD  
        ON I.TransDetail_ID = TD.Transdetail_id  
    INNER JOIN cashlistitem_payment_type PT  
        ON  I.cashlistitem_payment_type_id = PT.cashlistitem_payment_type_id  
    INNER JOIN Document D  
        ON D.Document_id = TD.Document_id  
    INNER JOIN Account A  
        ON I.account_id = A.account_id  
    INNER JOIN CashListItem_Payment_Status PS  
        ON I.cashlistitem_payment_status_id = PS.cashlistitem_payment_status_id  
    INNER JOIN MediaType M  
        ON I.mediaType_id = M.mediatype_id  
    INNER JOIN CashListItem_Payment_Type CLIPT  
        ON I.CashListItem_Payment_Type_id = CLIPT.CashListItem_Payment_Type_id  
    INNER JOIN BankAccount BA  
        ON C.BankAccount_id = BA.BankAccount_Id  
    LEFT Join Party P  
        ON A.Account_key= P.Party_cnt  
    INNER JOIN PMUSER U  
        ON  I.pmuser_id = U.user_id  
    LEFT JOIN Batch B  
        ON B.Batch_id = I.Batch_Id  
    LEFT OUTER JOIN Sub_Branch SB  
        ON C.sub_branch_id = SB.sub_branch_id and SB.is_deleted = 0  
    LEFT OUTER JOIN Bank BK  
    ON BA.bank_id = BK.bank_id  
    LEFT OUTER JOIN cashlistitem_reverse_reason CLRR  
    ON I.cashlistitem_reverse_reason_id = CLRR.cashlistitem_reverse_reason_id  
    LEFT OUTER JOIN User_Authorities UA  
        ON I.pmuser_id =  UA.user_id  
    LEFT OUTER JOIN CLAIM CLM  
        ON D.insurance_file_cnt = CLM.policy_id ' + CHAR(13) + CHAR(10)  
  
  select @where = 'WHERE '  
  
  -- Change PN 61824  
  IF @payee_name IS NOT NULL --Filter by @payee_name if passed  
  BEGIN  
   IF (SELECT CHARINDEX ('%',@payee_name)) > 0  
   SELECT @sSQL = @sSQL + @where + 'I.payment_name LIKE ''' + @payee_name  + ''''  
   ELSE  
   SELECT @sSQL = @sSQL + @where + 'I.payment_name = ''' + @payee_name  + ''''  
  SELECT @where = ' AND '  
  END  
    -- End Change PN 61824  
  
IF @account_id IS NOT NULL --Filter by @account_id if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'A.account_id = ' + cast(@account_id AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  IF @payment_type_id IS NOT NULL --Filter by @payment_type_id if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.cashlistitem_payment_type_id = ' + cast(@payment_type_id AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  IF @payment_media_type_id IS NOT NULL ----Filter by @payment_media_type_id if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.mediatype_id = ' + cast(@payment_media_type_id AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  IF @media_reference IS NOT NULL --Filter by @media_reference if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.media_ref = ' + @media_reference  
   SELECT @where = ' AND '  
  END  
  IF @payment_status_id IS NOT NULL --Filter by @payment_status_id if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.cashlistitem_payment_status_id = ' + cast(@payment_status_id AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  IF @amount IS NOT NULL --Filter by @amount if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.amount = ' + cast(@amount AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  IF @batch_number IS NOT NULL --Filter by @batch_number if passed  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.batch_id = ' + cast(@batch_number AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  
  -- Change PN 61824  
  IF @batch_ref IS NOT NULL --Filter by @batch_ref if passed  
  BEGIN  
   IF (SELECT CHARINDEX ('%',@batch_ref)) > 0  
    SELECT @sSQL = @sSQL + @where + 'B.batch_ref LIKE ''' + @batch_ref  + ''''  
     ELSE  
    SELECT @sSQL = @sSQL + @where + 'B.batch_ref = ''' + @batch_ref  + ''''  
     SELECT @where = ' AND '  
  END  
  -- End Change PN 61824  
  IF @branch IS NOT NULL  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'C.company_id = ' + cast (@branch AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  IF @clientcode IS NOT NULL  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'A.short_code = ''' + @clientcode + ''''  
   SELECT @where = ' AND '  
  END  
  
  IF @client_account_number IS NOT NULL  
  BEGIN  
   IF (SELECT CHARINDEX ('%',@client_account_number)) > 0  
    SELECT @sSQL = @sSQL + @where + 'I.payment_account_code LIKE ''' + @client_account_number  + ''''  
   ELSE  
    SELECT @sSQL = @sSQL + @where + 'I.payment_account_code = ''' + @client_account_number  + ''''  
   SELECT @where = ' AND '  
  END  
   ---End Change 61824  
  IF isnull(@showonlyoutstanding, 0) <> 0  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'TD.outstanding_amount <> 0'  
   SELECT @where = ' AND '  
  END  
  IF @bankaccountid IS NOT NULL  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'BA.bankaccount_id = ' + @bankaccountid  
   SELECT @where = ' AND '  
  END  
  
   --Change PN 61824  
        IF  @NumericSearch = 0  
  BEGIN  
   IF ISNULL(@media_from,'') <> ''  
   BEGIN  
    IF (SELECT CHARINDEX ('%',@media_from)) > 0  
     SELECT @sSQL = @sSQL + @where + 'I.media_ref LIKE ''' + @media_from  + ''''  
    ELSE  
     SELECT @sSQL = @sSQL + @where + 'I.media_ref = ''' + @media_from  + ''''  
    SELECT @where = ' AND '  
   END  
  
   IF ISNULL(@media_to,'') <> ''  
   BEGIN  
    IF (SELECT CHARINDEX ('%',@media_to)) > 0  
     SELECT @sSQL = @sSQL + @where + 'I.media_ref LIKE ''' + @media_to  + ''''  
    ELSE  
     SELECT @sSQL = @sSQL + @where + 'I.media_ref = ''' + @media_to  + ''''  
    SELECT @where = ' AND '  
   END  
  END  
  -- End Change PN 61824  
  
  --Filter by @date_from,@date_to if passed  
  IF ISNULL(@date_from,0) <> 0  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.transaction_date >= ''' + cast(@date_from AS VARCHAR) + ''''  
   SELECT @where = ' AND '  
  END  
  
  IF ISNULL(@date_to,0) <> 0  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'I.transaction_date <= ''' + cast(@date_to AS VARCHAR) +''''  
   SELECT @where = ' AND '  
  END  
  
  --Filter by @amount_from,@amount_to if passed  
  IF ISNULL(@amount_from,0) <> 0  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'ABS(I.amount) >= ' + cast(@amount_from AS VARCHAR)  
   SELECT @where = ' AND '  
  END  
  
  IF ISNULL(@amount_to,0) <> 0  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + 'ABS(I.amount) <= ' + cast(@amount_to AS VARCHAR)  
  END  
  IF ISNULL(@AgentKey,0) <> 0  
  BEGIN  
   SELECT @sSQL = @sSQL + @where + '(P.agent_cnt = ' + cast(@AgentKey AS VARCHAR) + ' OR P.party_cnt = ' + cast(@AgentKey AS VARCHAR)  + ')'
  END
EXEC (@sSql)  
  
--Numeric Search  
IF  @NumericSearch = 1  
BEGIN  
        -- Delete the Media Type of type Non-numeric  
        IF ISNULL(@Media_From,0)<> Convert(BIGINT,0) AND  ISNULL(@Media_To,0)<> Convert(BIGINT,0)  
        BEGIN  
              Delete FROM #CashListDetails WHERE PATINDEX('%[A-Z]%',UPPER(media_ref))<> 0  
              Delete FROM #CashListDetails WHERE ISNULL(media_ref,0)=0  
              Delete from #CashListDetails WHERE Convert(BIGINT,media_ref)< @Media_From OR convert(BIGINT,media_ref)> @Media_To  
        END  
END  
  
EXEC DDLADDINDEX '#CashListDetails', 'transdetail_id'  
  
--Look for Un-Issued cheques  
IF  ISNULL(@amount_from,0) = 1  
BEGIN  
  
    DECLARE @RowCount INT ,  
            @AccountId INT,  
            @MAX BIGINT,  
            @MIN BIGINT,  
            @BankAccount_No VARCHAR(50),  
            @MediaRef BIGINT  
    SET @RowCount= 0  
  
    DECLARE CUR_BANK CURSOR FOR  
  
        SELECT A.bankaccount_id,  
            FirstAvailable = LastSeqNumber + 1  ,  
            LastAvailable = NextSeqNumber - 1  
        FROM (  
            SELECT DISTINCT C1.bankaccount_id,LastSeqNumber = (SELECT isnull(Max(CONVERT(NUMERIC,C2.media_ref)),0) AS media_ref  
            FROM CHEQUE C2  
  
            WHERE CONVERT(NUMERIC,C2.media_ref) < CONVERT(NUMERIC,C1.media_ref) AND C2.bankaccount_id =C1.bankaccount_id) ,  
  
            NextSeqNumber = C1.media_ref  FROM cheque C1  
            LEFT JOIN BankAccount BA ON BA.Account_id = C1.BankAccount_ID  
            WHERE (CONVERT(NUMERIC,C1.media_ref)) > 0 AND (@bankaccountid IS NULL OR BA.bankaccount_id = @bankaccountid)  
            AND   (@branch IS NULL OR BA.sub_branch_id = @branch)  
            AND  
            (  
               (ISNULL(@date_from,0)=0 AND ISNULL(@date_to,0)=0)  
            OR  (   ISNULL(@date_from,0) <> 0 AND ISNULL(@date_to,0) <> 0  
            AND (Printed_date >=@date_from AND  Printed_date < DateAdd(d,1,@date_to  ))  
            OR  ( ISNULL(@date_from,0) <> 0 AND ISNULL(@date_to,0) = 0  
            AND (Printed_date >=@date_from))  
            OR  ( ISNULL(@date_from,0) = 0 AND ISNULL(@date_to,0) <> 0  
            AND (Printed_date < DateAdd(d,1,@date_to)))  
            ))  
      ) as A  
        WHERE NextSeqNumber - LastSeqNumber > 1 AND LastSeqNumber > 0  
    ORDER BY FirstAvailable  
  
    OPEN CUR_BANK  
    FETCH NEXT FROM CUR_BANK INTO @AccountId,@MediaRef,@max  
  
    WHILE @@FETCH_STATUS = 0  
    BEGIN  
  
  IF ISNULL(@media_from,Convert(BIGINT,0))<> Convert(BIGINT,0) OR ISNULL(@media_To,Convert(BIGINT,0))<> Convert(BIGINT,0)  
  BEGIN  
  
   IF ISNULL(@media_from,Convert(BIGINT,0))= Convert(BIGINT,0)  
    SET  @media_from = 1  
   IF ISNULL(@media_To,Convert(BIGINT,0))= Convert(BIGINT,0)  
    SET  @media_To = 9999999999  
  END  
     IF (ISNULL(@media_from,0)= 0 AND ISNULL(@media_To,0)= 0  
      AND ISNULL(@date_From,0) <> 0 AND ISNULL(@date_to,0) <> 0  
      OR (ISNULL(@date_From,0) <> 0 AND ISNULL(@date_to,0) = 0 )  
       OR (ISNULL(@date_From,0) = 0 AND ISNULL(@date_to,0) <> 0 )  
       OR @ResetMedia = 1)  
        BEGIN  
            -- Set the First and Last Account number  
            SET @media_From =  @MediaRef  
            SET @media_To = @MAX  
            SET @ResetMedia = 1  
        END  
  
        IF @media_To >= @MediaRef AND @media_From <= @MAX  
  BEGIN  
            IF @media_From > @MediaRef  
                SET @MediaRef = @media_From  
   WHILE  @RowCount < 500 AND @MediaRef <= @MAX AND @MediaRef <= @media_To  and (@payment_status_id_UnissuedCheques = @payment_status_id  OR ISNULL(@payment_status_id,0) = 0)  
   BEGIN  
   IF NOT EXISTS ( SELECT 1 FROM #CashListDetails WHERE Media_ref= @MediaRef AND bank_Account_id= @AccountId )  
    BEGIN  
     SET @RowCount = @RowCount +1  
  
      INSERT INTO #CashListDetails  
          (  
                                Media_Ref,  
                                Paymnent_Status_Description,  
                                bank_account_no,  
                                Media_description  
       )  
       VALUES  
      (  
       @MediaRef,  
       'UnIssued',  
       @BankAccount_No,  
                            @Code  
      )  
    END  
  
    SET @mediaRef= @MediaRef +1  
   END  
  END  
    FETCH NEXT FROM CUR_BANK INTO @AccountId,@MediaRef,@max  
    END  
    CLOSE CUR_BANK  
    DEALLOCATE CUR_BANK  
  
    END  
  
Update #CashListDetails SET PolicyNumber = Insurance_ref  
 FROM #CashListDetails CLD  
  INNER JOIN  
  (SELECT  (Insurance_ref), td_id2 FROM transdetail td  
         INNER JOIN  
    (SELECT ad2.transdetail_id 'td_id1', ad1.transdetail_id 'td_id2' FROM allocationdetail ad1  
    Inner Join allocationdetail ad2 ON ad2.allocation_id = ad1.allocation_id  
    INNER Join #CashListDetails tmpCl ON ad1.transdetail_id = tmpCl.transdetail_id  
     AND ad2.transdetail_id <> tmpCl.transdetail_id AND Allocated = 1) ad  
    ON ad.td_id1 = td.transdetail_id) ad_td  
 ON Transdetail_id = td_id2  
  
UPDATE tmpCashListDetails SET tmpCashListDetails.Insurance_file_cnt = ifi.insurance_file_cnt  
 FROM #CashListDetails tmpCashListDetails  
 INNER JOIN insurance_file ifi ON RTRIM(insurance_ref) = RTRIM(Policynumber)  
 WHERE PolicyNumber IS Not NULL  
  
UPDATE #CashListDetails SET Account_Name =  
    (SELECT shortname FROM Party WHERE Party_cnt = (SELECT MAX(insured_cnt)FROM Insurance_file IFL  
 WHERE IFL.Insurance_file_cnt = #CashListDetails.Insurance_file_cnt  ))  
  
UPDATE TMP SET TMP.ClaimNumber=C.claim_number FROM #CashListDetails TMP LEFT JOIN  
(SELECT DISTINCT Policy_Number,claim_number FROM Claim) C  
ON LTRIM(RTRIM(TMP.PolicyNumber))=LTRIM(RTRIM(C.Policy_Number)) AND ISNULL(TMP.PolicyNumber,'')<>''  
  
SELECT  
    short_code 'ClientCode' ,  
 account_name 'PolicyHolder' ,  
 ISNULL(PolicyNumber,'') + '   '+ISNULL(ClaimNumber,'') 'PolicyClaimNumber',  
 amount 'Amount',  
 transaction_date 'PaymentDate',  
 Media_description 'MediaReference',  
 Paymnent_Status_Description 'PaymentStatus' ,  
 bank_reconciliation_date 'BankReconciliationDate',  
 CLI_reverse_reason_description 'CancellationReason',  
 reversed_date 'CancellationDate',  
 account_id 'AccountNumber',  
 Bank_code 'BankSortCode',  
 Sub_branch_code 'BranchCode',  
 batch_ref 'BatchReference',  
 their_ref 'TheirReference',  
 our_ref 'OurReference',  
 bank_account_no 'BankAccount' ,  
 document_ref 'DocumentReference',  
 Payment_type_description 'PaymentType',  
 media_ref 'MediaType',  
 username 'User',  
 payment_name 'PayeeName' ,  
 cashlistitem_reverse_reason_id 'ReverseReasonKey' ,  
 allow_reverse_allocations 'AllowReverseAllocation' ,  
 reverse_allocations_days 'ReverseAllocationDays' ,  
 cashlistitem_id 'CashListItemKey',  
 transdetail_id 'TransDetailKey' ,  
 Party_cnt 'PartyKey',  
 insurance_file_cnt 'InsuranceFileKey' ,  
 claim_id 'ClaimKey',  
 currency_id 'CurrencyKey' ,  
 mediatype_id 'MediaTypeId',  
 Paymnent_Status_code 'PaymentStatuscode',  
 Payment_code 'PaymentCode',  
 bank_Account_ID 'BankAccountID',  
 Allocated,  
    payment_account_code 'PaymentAccountCode',  
    payment_branch_code 'PaymentBranchCode'  
  
    FROM #CashListDetails  
 WHERE (@policy_claim_number IS NULL OR PolicyNumber LIKE @policy_claim_number or ClaimNumber LIKE @policy_claim_number)  
 ORDER BY short_code,[ID]  
 
 GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO