
EXECUTE DDLDropProcedure 'spu_add_stats_folder_Deposit'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_add_stats_folder_Deposit
    @stats_folder_cnt int OUTPUT,
    @insurance_file_cnt int,
    @user_id int,  
    @user_name varchar(12),  
    --Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
    --Note:- it got changed as per table  
    @next_orion_doc_ref VARCHAR(25)  
    --End (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
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
    --JMK 03/05/2001  
    --@posting_period_year datetime,  
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
    @IsInstalments CHAR(1)  
  
DECLARE @deposit NUMERIC(19,4)  
--Oly do it if we have a PF  
IF EXISTS(SELECT @deposit FROM pfPremiumFinance WHERE Insurance_File_Cnt = @insurance_file_cnt )  
BEGIN  
    IF @deposit > 0  
    BEGIN  
    /* Set temporary default values */  
    SELECT  
        @debit_credit = 'D',  
        @document_prefix = 'JN',  
        @document_ref = 'Doc Ref',  
        @document_comment = 'Instalment NB Deposit',  
        @document_date = Getdate(),  
        @accounting_date = GetDate(),  
        @premium_total = 0,  
        @transaction_type_id = 1,  
        @transaction_type_code = 'NB',  
        @effective_date = GetDate(),  
        @loss_id = NULL,  
        @loss_code = NULL,  
        @loss_date = NULL  
  
    --SELECT @numbering_prefix = @document_prefix  
  
    -- Now get new doc ref from dedicated table.  
    -- Getting MAX from export folder sometimes cause duplicates on Orion.  
    -- Getting MAX from Document on Orion caused problems when FindTransaction referred back  
    -- to export folder for cover_start_date. Transactions that failed to post to Orion could have duplicate doc refs.  
    --SELECT  @retrieved_prefix = prefix,  
    --        @key_suffix_int = next_number  
    --FROM    Next_Orion_Doc_Ref  
    --WHERE   prefix = @numbering_prefix  
  
    --IF @Key_Suffix_Int is NULL  
    --BEGIN  
        -- This is a bit yakky but gives us the best chance of avoiding duplicates when  
        -- dealing with exisiting data.  
    --    SELECT  @max_orion_ref = MAX(document_ref)  
    --    FROM    Document  
    --    WHERE   document_ref like @document_prefix +'%'  
    --    AND     LEN ( LTRIM ( RTRIM ( document_ref ))) - LEN ( @document_prefix )  = 6  
  
    --    IF NOT( @max_orion_ref is null )  
    --        SELECT @Key_Suffix_Int = SUBSTRING ( @max_orion_ref ,  LEN ( @document_prefix ) + 1, 6 ) + 1  
    --    ELSE  
    --        SELECT @Key_Suffix_Int = 100001  
  
    --END  
  
    /* transaction_type details retrieved from Insurance_File_System */  
    SELECT @transaction_type_id = T.transaction_type_id,  
           @transaction_type_code = T.code  
    FROM   Insurance_File_System I,  
           Transaction_Type T  
    WHERE  I.insurance_file_cnt = @insurance_file_cnt  
    AND    T.transaction_type_id = I.last_trans_type_id  
  
    -- RWH (08/08/01) Set document comment dependant on transaction type.  
    SELECT @document_comment =  
        CASE @transaction_type_code  
            WHEN 'MTA' THEN 'Instalments MTA Deposit'  
            WHEN 'MTC' THEN 'Instalments MTC Deposit'  
            WHEN 'REN' THEN 'Instalements Renewal Deposit'  
        END  
  
    --Set document type dependant on transaction type.  
    SELECT @document_prefix = 'JN'  
  --Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
   -- SELECT  @document_ref = @document_prefix + CONVERT(CHAR,@next_orion_doc_ref)  
 SELECT  @document_ref = @document_prefix +  @next_orion_doc_ref  
--End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)  
  
    -- Set MTC as credit.  
    IF @transaction_type_code = 'MTC'  
        SELECT  @debit_credit = 'C'  
  
    -- Save next doc ref number.  
    --IF (@retrieved_prefix is null) OR (@retrieved_prefix = '')  
    --BEGIN  
  
    --    INSERT INTO Next_Orion_Doc_Ref  
    --    VALUES  (@numbering_prefix, @Key_Suffix_Int +1)  
  
    --END  
    --ELSE  
    --BEGIN  
    --    UPDATE  Next_Orion_Doc_Ref  
    --    SET     next_number = @Key_Suffix_Int +1  
    --    WHERE   prefix = @numbering_prefix  
    --END  
  
    -- Get details from insurance file  
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
           @sub_branch_id = branch_id -- IFIBCR  
    FROM   Insurance_File  
    WHERE  insurance_file_cnt = @insurance_file_cnt  
  
    -- Reset to sub_branch 1 where multi-tree accounts is not used  
    SELECT @multi_tree = value  
    FROM   hidden_options  
    WHERE  branch_id = 1 AND option_number = 16  
  
    IF (ISNULL(@multi_tree, '0') <> '1')  
        SELECT @sub_branch_id = 1  
  
    -- retrieve Period information from Orion  
    --declare @dtThisPeriodEnd datetime  
    SELECT @posting_period_number = current_period_id  
    FROM   ledger  
    WHERE  ledger_short_name = 'SA'  
    AND    sub_branch_id = @sub_branch_id  
  
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
  
    SELECT  @currency_code = code  
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
              created_by_username)  
    VALUES    (
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
              @user_name)  
  
    /* Return the Count of the Record Added */  
    SELECT @stats_folder_cnt = @@IDENTITY  
  
  
    END  
END  
  
END  

GO
