

EXECUTE DDLDropProcedure 'spu_ACT_Do_FindPayment'
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

  
CREATE PROCEDURE spu_ACT_Do_FindPayment    
    @payee_name varchar(60) = NULL,    
    @account_id int = NULL,    
    @payment_type_id int = NULL,    
    @payment_media_type_id int = NULL,    
    @media_reference varchar(100) = NULL,    
    @payment_status_id int = NULL,    
    @amount numeric(19,4) = NULL,    
    @batch_number int = NULL,    
    @batch_ref varchar(100) = NULL,    
    @user_id smallint = NULL,    
    @MaxRowsToFetch integer = 500,    
    @branch integer = NULL,    
    @clientcode varchar(50) = NULL,    
    @client_account_number varchar(60) = NULL,    
    @policy_claim_number varchar(60) = NULL,    
    @media_from varchar(60) = NULL,    
    @media_to varchar(60) = NULL,    
    @amount_from numeric(19,4) = NULL,    
    @amount_to numeric(19,4) = NULL,    
    @date_from datetime = NULL,    
    @date_to datetime = NULL,    
    @showonlyoutstanding int = 0,    
    @bankaccountid varchar(40) = NULL  
	  
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
 @sSql varchar(8000),    
 @where varchar(1000)    
    
SET NOCOUNT ON    
     
    
  
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
  cashlistitem_id bigint,    
  payment_name varchar(100),    
  account_id bigint,    
  Payment_description varchar(100),    
  Payment_code varchar(100),    
  Media_description varchar(100),    
  mediatype_id INT,    
  media_ref varchar(100),    
  amount numeric(20,2),
  Paymnent_Status_Description varchar(100),    
  Paymnent_Status_code varchar(100),    
  date_presented datetime,    
  batch_ref varchar(100),    
  username varchar(100),    
  cashlist_id varchar(100),    
  short_code varchar(100),    
  account_name varchar(255),    
  PolicyNumber varchar(50),    
  ClaimNumber varchar(50),    
  their_ref varchar(100),    
  our_ref varchar(255),    
  transaction_date datetime,    
  bank_account_no varchar(30),    
  document_ref varchar(50),    
  Payment_type_description varchar(100),    
  Sub_branch_code varchar(10),    
  Bank_code varchar(10),    
  CLI_reverse_reason_description varchar(100),    
  reversed_date datetime,    
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
                Allocated bit,    
                payment_account_code varchar(60),    
                payment_branch_code varchar(30),    
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
   else    
   SELECT @sSQL = @sSQL + @where + 'I.payment_name = ''' + @payee_name  + ''''    
  SELECT @where = ' AND '    
  END    
    -- End Change PN 61824    
    
  IF @account_id IS NOT NULL --Filter by @account_id if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'A.account_id = ' + cast(@account_id as varchar)    
   SELECT @where = ' AND '    
  END    
  IF @payment_type_id IS NOT NULL --Filter by @payment_type_id if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.cashlistitem_payment_type_id = ' + cast(@payment_type_id as varchar)    
   SELECT @where = ' AND '    
  END    
  IF @payment_media_type_id IS NOT NULL ----Filter by @payment_media_type_id if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.mediatype_id = ' + cast(@payment_media_type_id as varchar)    
   SELECT @where = ' AND '    
  END    
  IF @media_reference IS NOT NULL --Filter by @media_reference if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.media_ref = ' + @media_reference    
   SELECT @where = ' AND '    
  END    
  IF @payment_status_id IS NOT NULL --Filter by @payment_status_id if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.cashlistitem_payment_status_id = ' + cast(@payment_status_id as varchar)    
   SELECT @where = ' AND '    
  END    
  IF @amount IS NOT NULL --Filter by @amount if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.amount = ' + cast(@amount as varchar)    
   SELECT @where = ' AND '    
  END    
  IF @batch_number IS NOT NULL --Filter by @batch_number if passed    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.batch_id = ' + cast(@batch_number as varchar)    
   SELECT @where = ' AND '    
  END    
    
  -- Change PN 61824    
  IF @batch_ref IS NOT NULL --Filter by @batch_ref if passed    
  BEGIN    
   IF (SELECT CHARINDEX ('%',@batch_ref)) > 0    
    SELECT @sSQL = @sSQL + @where + 'B.batch_ref LIKE ''' + @batch_ref  + ''''    
     else    
    SELECT @sSQL = @sSQL + @where + 'B.batch_ref = ''' + @batch_ref  + ''''    
     SELECT @where = ' AND '    
  END    
  -- End Change PN 61824    
  IF @branch IS NOT NULL    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'C.company_id = ' + cast (@branch as varchar)    
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
   else    
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
    else    
     SELECT @sSQL = @sSQL + @where + 'I.media_ref = ''' + @media_from  + ''''    
    SELECT @where = ' AND '    
   END    
    
   IF ISNULL(@media_to,'') <> ''    
   BEGIN    
    IF (SELECT CHARINDEX ('%',@media_to)) > 0    
     SELECT @sSQL = @sSQL + @where + 'I.media_ref LIKE ''' + @media_to  + ''''    
    else    
     SELECT @sSQL = @sSQL + @where + 'I.media_ref = ''' + @media_to  + ''''    
    SELECT @where = ' AND '    
   END    
  END    
  -- End Change PN 61824    
    
  --Filter by @date_from,@date_to if passed    
  IF ISNULL(@date_from,0) <> 0    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.transaction_date >= ''' + cast(@date_from as varchar) + ''''    
   SELECT @where = ' AND '    
  END    
    
  IF ISNULL(@date_to,0) <> 0    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'I.transaction_date <= ''' + cast(@date_to as varchar) +''''    
   SELECT @where = ' AND '    
  END    
  
    
  --Filter by @amount_from,@amount_to if passed    
  IF ISNULL(@amount_from,0) <> 0    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'ABS(I.amount) >= ' + cast(@amount_from as varchar)    
   SELECT @where = ' AND '    
  END    
    
  IF ISNULL(@amount_to,0) <> 0    
  BEGIN    
   SELECT @sSQL = @sSQL + @where + 'ABS(I.amount) <= ' + cast(@amount_to as varchar)    
  END    
  
EXEC (@sSql)    
    
--Numeric Search    
IF  @NumericSearch = 1    
BEGIN    
        -- Delete the Media Type of type Non-numeric    
        IF ISNULL(@Media_From,0)<> Convert(bigint,0) AND  ISNULL(@Media_To,0)<> Convert(bigint,0)    
        BEGIN    
              Delete FROM #CashListDetails WHERE PATINDEX('%[A-Z]%',UPPER(media_ref))<> 0    
              Delete FROM #CashListDetails WHERE ISNULL(media_ref,0)=0    
              Delete from #CashListDetails WHERE Convert(bigint,media_ref)< @Media_From OR convert(bigint,media_ref)> @Media_To    
        END    
END    
    
EXEC DDLADDINDEX '#CashListDetails', 'transdetail_id'    
    
--Look for Un-Issued cheques    
IF  ISNULL(@amount_from,0) = 1    
BEGIN    
    
    DECLARE @RowCount INT ,    
            @AccountId INT,    
            @MAX bigint,    
            @MIN bigint,    
            @BankAccount_No varchar(50),    
            @MediaRef bigint    
    SET @RowCount= 0    
    
    DECLARE CUR_BANK CURSOR FAST_FORWARD FOR    
    
        SELECT A.bankaccount_id,    
            FirstAvailable = LastSeqNumber + 1  ,    
            LastAvailable = NextSeqNumber - 1    
        FROM (    
            SELECT Distinct C1.bankaccount_id,LastSeqNumber = (Select isnull(Max(CONVERT(NUMERIC,C2.media_ref)),0) as media_ref    
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
    order by 2    
    
  
    OPEN CUR_BANK    
    FETCH NEXT FROM CUR_BANK INTO @AccountId,@MediaRef,@max    
    
    WHILE @@FETCH_STATUS = 0    
    BEGIN    
    
  IF ISNULL(@media_from,Convert(bigint,0))<> Convert(bigint,0) OR ISNULL(@media_To,Convert(bigint,0))<> Convert(bigint,0)    
  BEGIN    
    
   IF ISNULL(@media_from,Convert(bigint,0))= Convert(bigint,0)    
    SET  @media_from = 1    
   IF ISNULL(@media_To,Convert(bigint,0))= Convert(bigint,0)    
    SET  @media_To = 9999999999    
  END    
     IF (ISNULL(@media_from,0)= 0 AND ISNULL(@media_To,0)= 0    
      AND ISNULL(@date_From,0) <> 0 AND ISNULL(@date_to,0) <> 0    
      OR (ISNULL(@date_From,0) <> 0 AND ISNULL(@date_to,0) = 0 )    
       OR (ISNULL(@date_From,0) = 0 AND ISNULL(@date_to,0) <> 0 )    
       OR @ResetMedia = 1)    
        BEGIN    
            -- Set the First and Last Account number    
            Set @media_From =  @MediaRef    
            Set @media_To = @MAX    
            Set @ResetMedia = 1    
        END    
    
        IF @media_To >= @MediaRef AND @media_From <= @MAX    
  BEGIN    
            IF @media_From > @MediaRef    
                Set @MediaRef = @media_From    
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
       Values    
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
  (SELECT  (Insurance_ref), td_id2 from transdetail td    
         INNER JOIN    
    (Select ad2.transdetail_id 'td_id1', ad1.transdetail_id 'td_id2' From allocationdetail ad1    
    Inner Join allocationdetail ad2 on ad2.allocation_id = ad1.allocation_id    
    INNER Join #CashListDetails tmpCl ON ad1.transdetail_id = tmpCl.transdetail_id    
     AND ad2.transdetail_id <> tmpCl.transdetail_id AND Allocated = 1) ad    
    ON ad.td_id1 = td.transdetail_id) ad_td    
 ON Transdetail_id = td_id2    
    
Update tmpCashListDetails SET tmpCashListDetails.Insurance_file_cnt = ifi.insurance_file_cnt    
 FROM #CashListDetails tmpCashListDetails    
 INNER JOIN insurance_file ifi ON RTRIM(insurance_ref) = RTRIM(Policynumber)    
 Where PolicyNumber IS Not NULL    
    
  
Update #CashListDetails SET Account_Name =    
    (Select shortname from Party WHERE Party_cnt = (Select MAX(insured_cnt)FROM Insurance_file IFL    
 Where IFL.Insurance_file_cnt = #CashListDetails.Insurance_file_cnt  ))    
    
--UPDATE TMP SET TMP.ClaimNumber=C.claim_number from #CashListDetails TMP LEFT JOIN    
--(SELECT DISTINCT Policy_Number,claim_number From Claim) C    
--ON LTRIM(RTRIM(TMP.PolicyNumber))=LTRIM(RTRIM(C.Policy_Number)) AND ISNULL(TMP.PolicyNumber,'')<>''    

Declare @transdetail_id int = 0
Declare @claimnumber varchar(50) = NULL
DECLARE db_cursor CURSOR FAST_FORWARD FOR  
select transdetail_id from #CashListDetails TMP 
OPEN db_cursor 
Fetch NEXT FROM db_cursor into @transdetail_id

while @@FETCH_STATUS = 0
BEGIN
	IF @transdetail_id IS NOT NULL 
	BEGIN
		set @claimnumber = (
						select top 1 C.claim_number from AllocationDetail AD2 
						JOIN TransDetail TD on AD2.transdetail_id = TD.transdetail_id
						JOIN Document D on TD.document_id = D.document_id
						JOIN Stats_Folder SF on D.document_ref = SF.document_ref
						JOIN Claim C on SF.loss_id = C.Claim_id
						where AD2.allocation_id = ( select top 1 AD1.allocation_id from AllocationDetail  AD1 where AD1.transdetail_id = @transdetail_id and is_reversed IS NULL)
						AND AD2.transdetail_id <> @transdetail_id
						)
		update #CashListDetails  set claimnumber = @claimnumber where transdetail_id = @transdetail_id 
	END 
	FETCH NEXT FROM db_cursor INTO @transdetail_id
end
CLOSE db_cursor
deallocate db_cursor


DECLARE @TmpMediaRef VARCHAR(100)    
    
SET @TmpMediaRef = REPLICATE('0',100)    
SET ROWCOUNT @MaxRowsToFetch     
    
Select cashlistitem_id,    
    payment_name,    
    account_id,    
    Payment_description,    
    Payment_code,    
    Media_description,    
    mediatype_id,    
    media_ref,    
    amount,    
    Paymnent_Status_Description,    
    Paymnent_Status_code,    
    date_presented,    
    batch_ref,    
    username,    
    cashlist_id,    
    short_code,    
    account_name,    
    ISNULL(PolicyNumber,'') + '   '+ISNULL(ClaimNumber,'') AS PolicyClaimNumber,    
    their_ref,    
    our_ref,    
    username,    
    transaction_date,    
    bank_account_no,    
    document_ref,    
    Payment_type_description,    
    Sub_branch_code,    
    Bank_code,    
    CLI_reverse_reason_description,    
    reversed_date,    
    cashlistitem_reverse_reason_id,    
    allow_reverse_allocations,    
    reverse_allocations_days,    
    transdetail_id,    
    Party_cnt,    
    insurance_file_cnt,    
    claim_id,    
    currency_id,    
    bank_Account_ID,    
    bank_reconciliation_date,    
    Allocated,    
    payment_account_code,    
    payment_branch_code ,    
 (Left(@TmpMediaRef,100-len(media_ref)) + media_ref) As TmpMediaRef     
    FROM #CashListDetails    
 WHERE (@policy_claim_number IS NULL OR PolicyNumber LIKE @policy_claim_number or ClaimNumber LIKE @policy_claim_number)    
 ORDER BY short_code,[ID],TmpMediaRef    
  
GO
