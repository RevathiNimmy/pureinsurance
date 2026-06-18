EXECUTE DDLDropProcedure 'spu_add_stats_folder_reverse'
GO
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_add_stats_folder_reverse
    @stats_folder_cnt int OUTPUT,
    @insurance_file_cnt int,
    @user_id int,
    @user_name varchar(255),
    @next_orion_doc_ref varchar(25)  
AS  
  
BEGIN  

DECLARE  
    @source_id int,  
    @debit_credit char(1),  
    @document_prefix char(3),  
    @document_ref varchar(25),  
    @document_comment varchar(60),  
    @document_date datetime,  
    @accounting_date datetime,  
    @posting_period_year int,  
    @posting_period_number smallint,  
    @premium_total numeric(19, 4),  
    @transaction_type_id int,  
    @transaction_type_code char(10),  
    @transaction_date datetime,  
    @insurance_ref varchar(30),  
    @effective_date datetime,  
    @cover_start_date datetime,  
    @expiry_date datetime,  
    @insurance_holder_cnt int,  
    @insurance_holder_shortname varchar(20),  
    @insurance_holder_name varchar(60),  
    @product_id int,  
    @product_code char(10),  
    @business_type_id smallint,  
    @business_type_code char(10),  
    @account_handler_cnt int,  
    @account_handler_shortname char(20),  
    @branch_id smallint,  
    @branch_code char(10),  
    @currency_code char(10),  
    @agent_cnt int,  
    @agent_shortname varchar(20),  
    @loss_id int,  
    @loss_code varchar(30),  
    @loss_date datetime,  
    @created_by_user_id smallint,  
    @created_by_username varchar(12),  
    --@key_suffix_int int,  
    @currency_id smallint,  
    --@retrieved_prefix varchar(10),  
    --@max_orion_ref varchar(20),  
    --@numbering_prefix char(3),  
    @sub_branch_id int,  
    @multi_tree varchar(20),  
    @IsInstalments CHAR(1),  
    @underwriting_year_id int,  
    @post_by_effective_date TINYINT --(RC) PLICO 9-10  
  
/* Set temporary default values */  
SELECT  
    @debit_credit = 'D',  
    @document_prefix = 'SND',  
    @document_ref = 'Doc Ref',  
    @document_comment = 'New Business Premium',  
    @document_date = Getdate(),  
    @accounting_date = GetDate(),  
-- JMK 03/05/2001  
--    @posting_period_year = GetDate(),  
--    @posting_period_number = 5,  
    @premium_total = 0,  
    @transaction_type_id = 1,  
    @transaction_type_code = 'NB',  
    @effective_date = GetDate(),  
    @loss_id = NULL,  
    @loss_code = NULL,  
    @loss_date = NULL  
  
/* TF180598 - transaction_type details retrieved from Insurance_File_System */  
SELECT TOP 1 @transaction_type_id = T.transaction_type_id,  
       @transaction_type_code = T.code  
FROM   stats_folder sf,  
       Transaction_Type T  
WHERE  sf.insurance_file_cnt = @insurance_file_cnt  
AND    T.transaction_type_id = sf.transaction_type_id  ORDER BY 1 DESC
  
-- RWH (08/08/01) Set document comment dependant on transaction type.  
SELECT @document_comment =  
    CASE @transaction_type_code  
        WHEN 'MTA' THEN 'MTA Premium'  
        WHEN 'MTC' THEN 'Mid-term Cancellation'  
        WHEN 'MTR' THEN 'Policy Reinstatement'  
        WHEN 'REN' THEN 'Renewal Premium'  
        WHEN 'DRI' THEN 'Deferred Reinsurance'  
        WHEN 'PT' THEN 'Portfolio Transfer'  
END  
  
SELECT @IsInstalments = 'S'  
  
-- Determine the correct credit / debit status  
SELECT  @debit_credit = CASE WHEN this_premium <= 0 THEN 'D' ELSE 'C' END  
FROM    insurance_file  
WHERE   insurance_file_cnt = @insurance_file_cnt  
  
SELECT @document_prefix =  
    CASE @transaction_type_code  
        WHEN 'MTA' THEN @IsInstalments + 'E' + @debit_credit -- i.e. SED  
        WHEN 'MTC' THEN @IsInstalments + 'E' + @debit_credit -- i.e. SEC  
        WHEN 'MTR' THEN @IsInstalments + 'I' + @debit_credit -- i.e. SID  
        WHEN 'REN' THEN @IsInstalments + 'R' + @debit_credit -- i.e. SRD  
        WHEN 'DRI' THEN 'SDD'  
        WHEN 'PT' THEN 'SPD'  
        ELSE  @IsInstalments + 'N' + @debit_credit -- i.e. SND  
    END  
  
SELECT  @document_ref = @document_prefix +  @next_orion_doc_ref  

SELECT @source_id = source_id,  
       @insurance_ref = insurance_ref,  
       @cover_start_date = cover_start_date,  
       @expiry_date = expiry_date,  
       @insurance_holder_cnt = insured_cnt,  
       @product_id = product_id,  
       @business_type_id = business_type_id,  
       @account_handler_cnt = account_handler_cnt,  
       @branch_id = branch_id,  
       @currency_id = currency_id,  
       @agent_cnt = lead_agent_cnt,  
       @sub_branch_id = branch_id, -- IFIBCR  
     @underwriting_year_id = underwriting_year_id  
FROM   Insurance_File  
WHERE  insurance_file_cnt = @insurance_file_cnt  
  

SELECT @multi_tree = value  
FROM   hidden_options  
WHERE  branch_id = 1 AND option_number = 16  
  
IF (ISNULL(@multi_tree, '0') <> '1')  
    SELECT @sub_branch_id = 1  

SELECT @posting_period_number=posting_period_id FROM Insurance_File WHERE insurance_file_cnt = @insurance_file_cnt  
  
IF @posting_period_number IS NULL BEGIN  
  
  --(RC) PLICO 9-10 START  
  SELECT  @post_by_effective_date = value  
  FROM    system_options  
  WHERE   branch_id = 1  
  AND     option_number = 5038  
  
  IF ISNULL(@post_by_effective_date, 0) = 1 BEGIN  
  
    IF @cover_start_date > getdate() BEGIN  
      SELECT @effective_date = @cover_start_date  
    END  
    IF @cover_start_date <= getdate() BEGIN  
      SELECT @effective_date = getdate()  
    END  
  
 -- Get posting period number  
 CREATE TABLE #tempTable (period_id int, year_name varchar(20), period_name varchar(15))  
 INSERT INTO #tempTable EXEC spu_ACT_Do_GetPeriodForDate @source_id, @effective_date, @sub_branch_id  
 SELECT @posting_period_number = period_id FROM #tempTable  
 DROP TABLE #tempTable  
 SELECT @posting_period_number  
  
  END  
  ELSE  
  BEGIN  
    SELECT @posting_period_number = current_period_id  
    FROM   ledger  
    WHERE  ledger_short_name = 'SA'  
    AND    sub_branch_id = @sub_branch_id  
  END  
  
END  

  
SELECT @posting_period_year = datepart(year, min(period_end_date))  
FROM   period  
WHERE  year_name = (SELECT year_name FROM period WHERE Period_id = @posting_period_number)  
AND    sub_branch_id = @sub_branch_id  

SELECT  @insurance_holder_shortname = shortname,  
    @insurance_holder_name = name  
FROM    Party  
WHERE   party_cnt = @insurance_holder_cnt  
  
SELECT  @account_handler_shortname = shortname  
FROM    Party  
WHERE   party_cnt = @account_handler_cnt  
  
SELECT  @product_code = code  
FROM    Product  
WHERE   product_id = @product_id  
  
SELECT  @business_type_code = code  
FROM    Business_Type  
WHERE   business_type_id = @business_type_id  
  
SELECT  @branch_code = code  
FROM    Branch  
WHERE   branch_id = @branch_id  
  
SELECT  @currency_code = iso_code  
FROM    Currency  
WHERE   currency_id = @currency_id  
  
SELECT  @agent_shortname = shortname  
FROM    Party  
WHERE   party_cnt = @agent_cnt  
  
/* Set transaction date */  
SELECT @transaction_date = GetDate()  
  
/* Insert the Stats Folder */  
INSERT INTO Stats_Folder (  
              source_id,  
              debit_credit,  
              document_ref,  
              document_comment,  
              document_date,  
              accounting_date,  
              posting_period_year,  
              posting_period_number,  
              premium_total,  
              transaction_type_id,  
              transaction_type_code,  
              transaction_date,  
              insurance_file_cnt,  
              insurance_ref,  
              effective_date,  
              cover_start_date,  
              expiry_date,  
              insurance_holder_cnt,  
              insurance_holder_shortname,  
              insurance_holder_name,  
              product_id,  
              product_code,  
              business_type_id,  
              business_type_code,  
              account_handler_cnt,  
              account_handler_shortname,  
              branch_id,  
              branch_code,  
              currency_code,  
         agent_cnt,  
              agent_shortname,  
              loss_id,  
              loss_code,  
              loss_date,  
              created_by_user_id,  
              created_by_username,  
        underwriting_year_id)  
VALUES       (  
              @source_id,  
              @debit_credit,  
              @document_ref,  
              @document_comment,  
              @document_date,  
              @accounting_date,  
              @posting_period_year,  
              @posting_period_number,  
              @premium_total,  
              @transaction_type_id,  
              @transaction_type_code,  
              @transaction_date,  
              @insurance_file_cnt,  
              @insurance_ref,  
              @effective_date,  
              @cover_start_date,  
              @expiry_date,  
              @insurance_holder_cnt,  
              @insurance_holder_shortname,  
              @insurance_holder_name,  
              @product_id,  
              @product_code,  
              @business_type_id,  
              @business_type_code,  
              @account_handler_cnt,  
              @account_handler_shortname,  
              @branch_id,  
              @branch_code,  
              @currency_code,  
              @agent_cnt,  
              @agent_shortname,  
              @loss_id,  
              @loss_code,  
              @loss_date,  
              @user_id,  
              @user_name,  
        @underwriting_year_id)  
  
/* Return the Count of the Record Added */  
SELECT @stats_folder_cnt = @@IDENTITY  
  
  
END  
GO

