SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Party_upd'
GO

CREATE PROCEDURE spe_Party_upd
    @party_cnt INT,
    @party_type_id SMALLINT,
    @is_also_agent TINYINT,
    @party_structure_id INT,
    @source_id SMALLINT,
    @party_id INT,
    @shortname CHAR(20),
    @name VARCHAR(255),
    @resolved_name VARCHAR(255),
    @currency_id SMALLINT,
    @language_id SMALLINT,
    @collect_type_id SMALLINT,
    @accum_treatment_type_id INT,
    @stats_treatment_type_id INT,
    @party_category_id INT,
    @agent_cnt INT,
    @consultant_cnt INT,
    @created_by_id SMALLINT,
    @date_created DATETIME,
    @last_modified DATETIME,
    @modified_by_id SMALLINT,
    @payment_method_code VARCHAR(70),
    @payment_term_code INT,
    @credit_card_code VARCHAR(70),
    @file_code VARCHAR(50),
    @abc_count INT,
    @statements TINYINT,
    @reminder_type_id INT,
    @renewals TINYINT,
    @status VARCHAR(6),
    @last_action_type VARCHAR(20),
    @is_travel_agent TINYINT,
    @is_prospect TINYINT,
    @is_deleted TINYINT,
    @abi_code_on_406 VARCHAR(4),
    @abi_code_on_81 VARCHAR(3),
    @abi_codelist VARCHAR(6),
    @area_id INT,
    @service_level_id INT,
    @invariant_key INT,
    @record_status VARCHAR(5),
    @CCJs INT,
    @user_defined_data_id INT,
    @seasonal_gift_id INT,
    @correspondence_type_id INT,
    @renewal_stop_code_id INT,
    @swift_party_id INT,
    @loyalty_number CHAR(20),
    @alternative_identifier CHAR(15),
    @marketing_segment_ind CHAR(20),
    @trading_name VARCHAR(255),
    @sub_branch_id INT,
    @tob_letter DATETIME,
	@UserId int=NULL,
	@UniqueId varchar(50)=NULL,
	@ScreenHierarchy varchar(500)=NULL
AS

DECLARE @getbase BIT
DECLARE @sTable VARCHAR(70)
DECLARE @party_type_code VARCHAR(10)
DECLARE @source_id_old INT 

IF EXISTS 
    (   
        SELECT 
            NULL 
        FROM claim
        WHERE client_short_name = 
            (
                SELECT 
                    shortname 
                FROM party 
                WHERE party_cnt = @party_cnt
            )
    )
BEGIN
    UPDATE claim
    SET claim.client_short_name = @Shortname
    WHERE claim.client_short_name = 
        (
            SELECT 
                shortname 
            FROM party 
            WHERE party_cnt = @party_cnt
        )
END

--SET @party_id=0

IF ISNULL(@sub_branch_id, 0) = 0
BEGIN
    EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT
END
    
UPDATE Party
SET party_type_id = @party_type_id,
    is_also_agent = @is_also_agent,
    party_structure_id = @party_structure_id,
    source_id = @source_id,
    party_id = @party_id,
    shortname = @shortname,
    name = @name,
    resolved_name = @resolved_name,
    currency_id = @currency_id,
    language_id = @language_id,
    collect_type_id = @collect_type_id,
    accum_treatment_type_id = @accum_treatment_type_id,
    stats_treatment_type_id = @stats_treatment_type_id,
    party_category_id = @party_category_id,
    agent_cnt = @agent_cnt,
    consultant_cnt = @consultant_cnt,
    created_by_id = @created_by_id,
    date_created = @date_created,
    last_modified = getdate(),
    modified_by_id = @modified_by_id,
    payment_method_code = @payment_method_code,
    payment_term_code = @payment_term_code,
    credit_card_code = @credit_card_code,
    file_code = @file_code,
    abc_count = @abc_count,
    statements = @statements,
    reminder_type_id = @reminder_type_id,
    renewals = @renewals,
    status = @status,
    last_action_type = @last_action_type,
    is_travel_agent = @is_travel_agent,
    is_prospect = @is_prospect,
    is_deleted = @is_deleted,
    abi_code_on_406 = @abi_code_on_406,
    abi_code_on_81 = @abi_code_on_81,
    abi_codelist = @abi_codelist,
    area_id = @area_id,
    service_level_id = @service_level_id,
    invariant_key = @invariant_key,
    record_status = @record_status,
    CCJs = @CCJs,
    user_defined_data_id = @user_defined_data_id,
    seasonal_gift_id = @seasonal_gift_id,
    correspondence_type_id = @correspondence_type_id,
    renewal_stop_code_id = @renewal_stop_code_id,
    swift_party_id = @swift_party_id,
    loyalty_number = @loyalty_number ,
    alternative_identifier = @alternative_identifier,
    marketing_segment_ind = @marketing_segment_ind ,
    trading_name = @trading_name,
    sub_branch_id = @sub_branch_id,
    tob_letter = @tob_letter,
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
WHERE party_cnt = @party_cnt

SELECT @Party_Type_Code = Code 
FROM party_type 
WHERE party_type_id = @party_type_id

SELECT @getbase = 
    CASE @party_type_code
        WHEN 'CO' THEN 1
        WHEN 'AH' THEN 1
        WHEN 'BR' THEN 1
        WHEN 'DI' THEN 1
        WHEN 'CM' THEN 1
        WHEN 'NC' THEN 1
        /*WHEN 'FP' THEN 1*/
        WHEN 'HC' THEN 1
        WHEN 'AGG' THEN 1
        /*You can set the currency on the other party form.*/
        /*WHEN 'OTSOL' THEN 1
        WHEN 'OTLOSS' THEN 1
        WHEN 'OTTHIRD' THEN 1
        WHEN 'OTDRIVER' THEN 1
        WHEN 'OTWITNESS' THEN 1
        WHEN 'OTREPAIRER' THEN 1
        WHEN 'OTSUPPLIER' THEN 1*/
        ELSE 0
    END         

IF @getbase = 1
BEGIN
    UPDATE party 
    SET currency_id = 
        (
            SELECT 
                base_currency_id 
            FROM source 
            WHERE source_id = @source_id
        ) 
    WHERE party_cnt = @party_cnt
END

/*Updating Insured Name in Insurance File*/
UPDATE  Insurance_File 
SET insured_name = LEFT(@resolved_name, 100)
WHERE insured_cnt = @party_cnt
--CLIENT MANAGER EVENT LOG  
DECLARE @event_Cnt INT  
DECLARE @event_Date DATETIME  
DECLARE @event_type_id int  
SET @event_Date = GETDATE()  
DECLARE @party_code VARCHAR(3)  
SET @party_code = (SELECT LTRIM(RTRIM(code)) FROM Party_Type WHERE party_type_id = @party_type_id)  
  
--Event logging required only for Personal/Corporate/Group Client  
  
IF @party_code = 'PC'OR @party_code = 'CC' OR @party_code = 'GC'  
BEGIN  
  
IF  EXISTS (SELECT event_type_id FROM event_type WHERE code = 'CLIENTUPD'  )  
BEGIN  
 SELECT @event_type_id= event_type_id FROM event_type WHERE code = 'CLIENTUPD'    
  
 EXEC spe_event_log_add @event_Cnt OUTPUT, @party_cnt ,NULL, NULL, NULL , NULL, NULL, NULL, NULL , NULL ,NULL ,  
  @event_type_id , @UserId, @event_Date,  
 'Client was updated', 0,NULL , NULL , NULL , NULL, NULL ,NULL, NULL , NULL , NULL,NULL,NULL,NULL,NULL,NULL  
END  
END 
GO


