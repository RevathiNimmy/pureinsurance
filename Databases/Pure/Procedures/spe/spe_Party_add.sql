SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Party_add'
GO

/*
   sj 13/06/2002 - Add loyalty_number, alternative_identifier, marketing_segment_ind,
                   trading_name and sub_branch_id 
*/
CREATE PROCEDURE spe_Party_add
    @party_cnt int OUTPUT ,
	@party_shortname char(20) = '' Output ,
    @party_type_id smallint ,
    @is_also_agent tinyint ,
    @party_structure_id int ,
    @source_id smallint ,
    @party_id int ,
    @shortname char(20) ,
    @name varchar(255) ,
    @resolved_name varchar(255) ,
    @currency_id smallint ,
    @language_id smallint ,
    @collect_type_id smallint ,
    @accum_treatment_type_id int ,
    @stats_treatment_type_id int ,
    @party_category_id int ,
    @agent_cnt int ,
    @consultant_cnt int ,
    @created_by_id smallint ,
    @date_created datetime ,
    @last_modified datetime ,
    @modified_by_id smallint ,
    @payment_method_code varchar(70) ,
    @payment_term_code INT ,
    @credit_card_code varchar(70) ,
    @file_code varchar(50) ,
    @abc_count int ,
    @statements tinyint ,
    @reminder_type_id int ,
    @renewals tinyint ,
    @status varchar(6) ,
    @last_action_type varchar(20) ,
    @is_travel_agent tinyint ,
    @is_prospect tinyint ,
    @is_deleted tinyint ,
    @abi_code_on_406 varchar(4) ,
    @abi_code_on_81 varchar(3) ,
    @abi_codelist varchar(6) ,
    @area_id int ,
    @service_level_id int ,
    @invariant_key int ,
    @record_status varchar(5) ,
    @CCJs int ,
    @user_defined_data_id int ,
    @seasonal_gift_id int ,
    @correspondence_type_id int ,
    @renewal_stop_code_id int ,
    @swift_party_id int, 
    @loyalty_number char(20), 
    @alternative_identifier char(15),
    @marketing_segment_ind char(20),
    @trading_name varchar(255),
    @sub_branch_id int,
    @tob_letter datetime,  
    @UserId int,  
    @UniqueId VARCHAR(50),    
    @ScreenHierarchy VARCHAR(500)
AS
BEGIN

DECLARE @getbase bit
DECLARE @Party_Type_Code varchar(10)

--PWF 10/08/2002 - Check for source id
IF ISNULL(@source_id, 0) = 0 
    SELECT @source_id = 1

--PWF 10/08/2002 - Check for sub branch id
IF ISNULL(@sub_branch_id, 0) = 0
    EXEC spu_sub_branch_default @source_id, @sub_branch_id OUTPUT
    
SET @party_id=0



-- Make sure that Party Name is Unique

DECLARE @nLoop INT = 0
DECLARE  @tmpshortname VARCHAR(20)
DECLARE @OriginalShortName VARCHAR(20)

SET @tmpshortname =  RTRIM(@shortname)
SET @OriginalShortName = RTRIM(@shortname)
--Make sure that Party Name is Unique
Declare @tmpPartyCnt INT

SELECT @tmpPartyCnt = Party_cnt FROM Party WHERE shortname = @shortname

WHILE ISNULL(@tmpPartyCnt,0) > 0 --Party Shortcode Already Exists. Generate a New One
BEGIN
    SET @nLoop = @nLoop + 1
    SET @tmpPartyCnt = 0
    IF @nLoop <= 999
    BEGIN
        SET @tmpshortname = @OriginalShortName + convert(varchar,REPLICATE('0',3-LEN(@nLoop))) + Convert(VARCHAR,@nLoop)
    END
    SELECT @tmpPartyCnt = Party_cnt FROM Party WHERE shortname = @tmpshortname
END

SET @shortname =  @tmpshortname
--------------------------------------------------------------------------


BEGIN TRY

    INSERT INTO Party (
        party_type_id,   
        is_also_agent,
        party_structure_id,
        source_id,
        party_id,
        shortname,
        name,
        resolved_name,
        currency_id,
        language_id,
        collect_type_id,
        accum_treatment_type_id,
        stats_treatment_type_id,
        party_category_id,
        agent_cnt,
        consultant_cnt,
        created_by_id,
        date_created,
        last_modified,
        modified_by_id,
        payment_method_code,
        payment_term_code,
        credit_card_code,
        file_code,
        abc_count,
        statements,
        reminder_type_id,
        renewals,
        status,
        last_action_type,
        is_travel_agent,
        is_prospect,
        is_deleted,
        abi_code_on_406,
        abi_code_on_81,
        abi_codelist,
        area_id,
        service_level_id,
        invariant_key,
        record_status,
        CCJs,
        user_defined_data_id,
        seasonal_gift_id,
        correspondence_type_id,
        renewal_stop_code_id,
        swift_party_id,
        loyalty_number,
        alternative_identifier,
        marketing_segment_ind,
        trading_name,
        sub_branch_id,
        tob_letter,  
        UserId,  
        UniqueId,    
        ScreenHierarchy )
    VALUES (
        @party_type_id,
        @is_also_agent,
        @party_structure_id,
        @source_id,
        @party_id,
        @shortname,
        @name,
        @resolved_name,
        @currency_id,
        @language_id,
        @collect_type_id,
        @accum_treatment_type_id,
        @stats_treatment_type_id,
        @party_category_id,
        @agent_cnt,
        @consultant_cnt,
        @created_by_id,
        @date_created,
        @last_modified,
        @modified_by_id,
        @payment_method_code,
        @payment_term_code,
        @credit_card_code,
        @file_code,
        @abc_count,
        @statements,
        @reminder_type_id,
        @renewals,
        @status,
        @last_action_type,
        @is_travel_agent,
        @is_prospect,
        @is_deleted,
        @abi_code_on_406,
        @abi_code_on_81,
        @abi_codelist,
        @area_id,
        @service_level_id,
        @invariant_key,
        @record_status,
        @CCJs,
        @user_defined_data_id,
        @seasonal_gift_id,
        @correspondence_type_id,
        @renewal_stop_code_id,
        @swift_party_id,
        @loyalty_number,
        @alternative_identifier,
        @marketing_segment_ind,
        @trading_name,
        @sub_branch_id,
        @tob_letter,  
        @UserId,  
        @UniqueId,    
        @ScreenHierarchy)

    SELECT @party_cnt = SCOPE_IDENTITY()
END TRY
BEGIN CATCH   -- Added to avoid the Failure( specially from DTU2 which Transfer the data at a very high rate) 


SET @nLoop = @nLoop + 1
SET @tmpshortname = @OriginalShortName + convert(varchar,REPLICATE('0',3-LEN(@nLoop))) + Convert(VARCHAR,@nLoop)
SELECT @tmpPartyCnt = Party_cnt FROM Party WHERE shortname = @tmpshortname

WHILE ISNULL(@tmpPartyCnt,0) > 0 --Party Shortcode Already Exists. Generate a New One
BEGIN
    SET @nLoop = @nLoop + 1
    SET @tmpPartyCnt = 0
    IF @nLoop <= 999
    BEGIN
        SET @tmpshortname = @OriginalShortName + convert(varchar,REPLICATE('0',3-LEN(@nLoop))) + Convert(VARCHAR,@nLoop)
    END
    SELECT @tmpPartyCnt = Party_cnt FROM Party WHERE shortname = @tmpshortname
END

SET @shortname =  @tmpshortname


   INSERT INTO Party (
        party_type_id,   
        is_also_agent,
        party_structure_id,
        source_id,
        party_id,
        shortname,
        name,
        resolved_name,
        currency_id,
        language_id,
        collect_type_id,
        accum_treatment_type_id,
        stats_treatment_type_id,
        party_category_id,
        agent_cnt,
        consultant_cnt,
        created_by_id,
        date_created,
        last_modified,
        modified_by_id,
        payment_method_code,
        payment_term_code,
        credit_card_code,
        file_code,
        abc_count,
        statements,
        reminder_type_id,
        renewals,
        status,
        last_action_type,
        is_travel_agent,
        is_prospect,
        is_deleted,
        abi_code_on_406,
        abi_code_on_81,
        abi_codelist,
        area_id,
        service_level_id,
        invariant_key,
        record_status,
        CCJs,
        user_defined_data_id,
        seasonal_gift_id,
        correspondence_type_id,
        renewal_stop_code_id,
        swift_party_id,
        loyalty_number,
        alternative_identifier,
        marketing_segment_ind,
        trading_name,
        sub_branch_id,
        tob_letter,  
        UserId,  
        UniqueId,    
        ScreenHierarchy )
    VALUES (
        @party_type_id,
        @is_also_agent,
        @party_structure_id,
        @source_id,
        @party_id,
        @shortname,
        @name,
        @resolved_name,
        @currency_id,
        @language_id,
        @collect_type_id,
        @accum_treatment_type_id,
        @stats_treatment_type_id,
        @party_category_id,
        @agent_cnt,
        @consultant_cnt,
        @created_by_id,
        @date_created,
        @last_modified,
        @modified_by_id,
        @payment_method_code,
        @payment_term_code,
        @credit_card_code,
        @file_code,
        @abc_count,
        @statements,
        @reminder_type_id,
        @renewals,
        @status,
        @last_action_type,
        @is_travel_agent,
        @is_prospect,
        @is_deleted,
        @abi_code_on_406,
        @abi_code_on_81,
        @abi_codelist,
        @area_id,
        @service_level_id,
        @invariant_key,
        @record_status,
        @CCJs,
        @user_defined_data_id,
        @seasonal_gift_id,
        @correspondence_type_id,
        @renewal_stop_code_id,
        @swift_party_id,
        @loyalty_number,
        @alternative_identifier,
        @marketing_segment_ind,
        @trading_name,
        @sub_branch_id,
        @tob_letter,  
        @UserId,  
        @UniqueId,    
        @ScreenHierarchy)

    SELECT @party_cnt = SCOPE_IDENTITY()

END CATCH


--Kevin renshaw (CMG) - Moved functionality from Party trigger into this procedure PN3246

SELECT @Party_Type_Code = Code from party_type where party_type_id = @party_type_id
IF @Party_Type_Code = 'IN'
BEGIN
	EXECUTE spu_Party_GIS_Insurer_Synch
        @party_cnt, @shortname, @resolved_name, @abi_code_on_81, @is_deleted, NULL, NULL, @party_cnt, NULL
END

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
    UPDATE party SET currency_id = (SELECT base_currency_id FROM source 
                                    WHERE source_id = @Source_ID) 
        WHERE party_cnt = @party_cnt

END

-- Special M9 product option
-- This is temporary and will be replaced with future Party Numbering extensions
IF @party_type_code='PC'
BEGIN
	IF EXISTS (SELECT value FROM hidden_options WHERE option_number=77 AND ISNULL(value,0)=1)
	BEGIN
		DECLARE @sPartyCnt VARCHAR(20)

		SELECT @sPartyCnt=CAST(@party_cnt AS VARCHAR(20))

		UPDATE Party
		SET shortname='M9'+LEFT('00000000',8-Len(@sPartyCnt))+@sPartyCnt
		WHERE party_cnt=@party_cnt

	END
END

select @party_shortname = shortname from party where party_cnt = @party_cnt

GO
