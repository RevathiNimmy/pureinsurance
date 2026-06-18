SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_add_stats_folder'
GO

CREATE PROCEDURE spu_clm_add_stats_folder    
    @stats_folder_cnt INT OUTPUT,    
    @transaction_type_code CHAR(10),   
    @user_id INT,    
    @claim_id INT
        
    
   AS    
          
    DECLARE    
        @source_id smallint,    
        @document_ref varchar(25),    
        @document_date datetime,    
        @accounting_date datetime,    
        @premium_total numeric(19, 4),    
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
        @sub_branch_id smallint,    
        @sub_branch_code char(10),    
        @currency_code char(10),    
        @agent_cnt int,    
        @agent_shortname varchar(20),    
        @loss_id int,    
        @loss_code varchar(30),    
        @loss_date datetime,    
        @underwriting_year_id int,    
        @currency_id smallint,
        @insurance_file_cnt INT,  --policy_id  
		@debit_credit VARCHAR(1),  --  
		@document_comment VARCHAR(60),-- "Payment for claim number " & v_sClaimNumber   
		@transaction_type_id INT,    
		@user_name VARCHAR(12),        
		@documenttype_id INT,  
        @ClaimNumber VARCHAR(30)
 
 BEGIN
 
-- select the claim number
SELECT @ClaimNumber =Claim_NUMBER, @insurance_file_cnt=policy_id FROM Claim WHERE Claim_id= @CLAIM_ID

 -- SELECT THE TRANSACTION TYPE ID
 SELECT @transaction_type_id= (SELECT transaction_type_id FROM Transaction_Type WHERE code=@transaction_type_code)
 
 SELECT @user_name =(SELECT username  FROM PMUser WHERE user_id=@user_id )
 --SELECT DOCUMENT TYPE ID
 SELECT @DocumentType_id =(SELECT
							CASE @transaction_type_code
							WHEN 'C_CP' THEN 28
							WHEN 'C_CR' THEN 41
							WHEN 'C_CO' THEN 35
							WHEN 'C_SA' THEN 29
							WHEN 'C_RV' THEN 29
							END)
					
 --SELECT DOCUMENT COMMENT
SELECT @document_comment=(SELECT
							CASE @transaction_type_code
							WHEN 'C_CP' THEN 'Payment for claim number ' +@ClaimNumber
							WHEN 'C_CR' THEN 'Reserves for claim number ' +@ClaimNumber
							WHEN 'C_CO' THEN 'Open Claim' +@ClaimNumber
							WHEN 'C_CA' THEN 'Maintain Claim' +@ClaimNumber
							WHEN 'C_SA' THEN 'Salvage for Claim Number :=' + @ClaimNumber
							WHEN 'C_RV' THEN 'TP Recovery for Claim Number :=' + @ClaimNumber
							END)
 
 -- SELECT DEBIT CREDIT FLAG
  SELECT @debit_credit=(SELECT
							CASE @transaction_type_code
							WHEN 'C_CP' THEN 'C'
							WHEN 'C_CR' THEN 'D'
							WHEN 'C_CO' THEN 'D'
							WHEN 'C_CA' THEN 'D'
							WHEN 'C_SA' THEN 'C'
							WHEN 'C_RV' THEN 'C'
							END)
							

    -- Set default values    
    SELECT  @accounting_date = GetDate(),    
            @document_ref = 'Doc Ref',    
            @document_date = Getdate(),    
            @effective_date = GetDate(),    
            @loss_id = @claim_id,    
            @premium_total = 0,    
            @transaction_date = GetDate()    
    
    -- Get loss code and date    
    SELECT  @loss_code = Claim_Number,    
            @loss_date = Loss_from_date,    
            @underwriting_year_id = underwriting_year_id    
    FROM    claim    
    WHERE   claim_id = @claim_id    
    
    -- Get details from insurance file    
    SELECT  @source_id = ifi.source_id,    
            @insurance_ref = ifi.insurance_ref,    
            @cover_start_date = ifi.cover_start_date,    
            @expiry_date = ifi.expiry_date,    
            @insurance_holder_cnt = ifo.insurance_holder_cnt,    
            @product_id = ifi.product_id,    
            @business_type_id = ifi.business_type_id,    
            @account_handler_cnt = ifi.account_handler_cnt,    
            @sub_branch_id = ifi.branch_id,    
            @agent_cnt = ifi.lead_agent_cnt    
    FROM    Insurance_File ifi    
    JOIN    Insurance_Folder ifo    
            ON ifi.insurance_folder_cnt = ifo.insurance_folder_cnt    
    WHERE   ifi.insurance_file_cnt = @insurance_file_cnt    
    
    -- Get appropriate currency    
    IF @documenttype_id = 28    
        --Get Payment Currency    
        SELECT  @currency_id = currency_id    
        FROM    claim_payment    
        WHERE   claim_id = @claim_id    
        AND     claim_payment_id = base_claim_payment_id    
    
    ELSE IF @documenttype_id = 29    
        --Get Receipt Currency    
        SELECT  @currency_id = currency_id    
        FROM    claim_receipt    
        WHERE   claim_id = @claim_id    
        AND     claim_receipt_id = base_claim_receipt_id  
    
    ELSE -- @documenttype_id NOT IN (28, 29)    
        --Get Reserve currency    
        SELECT  @currency_id = currency_id    
        FROM    claim    
        WHERE   claim_id = @claim_id    
    
    -- Set appropriate comment    
    SELECT  @document_comment = CASE @transaction_type_code    
        WHEN 'C_CO' THEN 'Reserve for claim ref. ' + @loss_code    
        WHEN 'C_CR' THEN 'Reserve for claim ref. ' + @loss_code    
        WHEN 'C_CP' THEN 'Payment for claim ref. ' + @loss_code    
        WHEN 'C_SA' THEN 'Salvage for claim ref. ' + @loss_code    
        WHEN 'C_RV' THEN 'TP Recovery for claim ref. ' + @loss_code    
        ELSE @document_comment    
        END    
    
    -- get client name    
    SELECT  @insurance_holder_shortname = shortname,    
            @insurance_holder_name = name    
    FROM    Party    
    WHERE   party_cnt = @insurance_holder_cnt    
    
    -- get account handler name    
    SELECT  @account_handler_shortname = shortname    
    FROM    Party    
    WHERE   party_cnt = @account_handler_cnt    
    
    -- get product code    
    SELECT  @product_code = code    
    FROM    Product    
    WHERE   product_id = @product_id    
    
    -- get business type code    
    SELECT  @business_type_code = code    
    FROM    Business_Type    
    WHERE   business_type_id = @business_type_id    
    
    -- get branch code    
    SELECT  @sub_branch_code = code    
    FROM    sub_branch    
    WHERE   sub_branch_id = @sub_branch_id    
    
    -- get currency code    
    SELECT  @currency_code = code    
    FROM    Currency    
    WHERE   currency_id = @currency_id    
    
    -- get agent name    
    SELECT  @agent_shortname = shortname    
    FROM    Party    
    WHERE   party_cnt = @agent_cnt    
    
    -- Insert the Stats Folder    
    INSERT INTO Stats_Folder 
	(     
            source_id,    
            debit_credit,    
            document_ref,    
            document_comment,    
            document_date,    
            accounting_date,    
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
            underwriting_year_id, 
	    posting_period_year, 
	    posting_period_number)    
    VALUES
	(
            @source_id,    
            @debit_credit,    
            @document_ref,    
            @document_comment,    
            @document_date,    
            @accounting_date,    
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
            @sub_branch_id,    
            @sub_branch_code,    
            @currency_code,    
            @agent_cnt,    
            @agent_shortname,    
            @loss_id,    
            @loss_code,    
            @loss_date,    
            @user_id,    
            @user_name,    
            @underwriting_year_id, 
	    0,
	    0
    )
    SELECT @stats_folder_cnt = @@IDENTITY  
  
    
 END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
