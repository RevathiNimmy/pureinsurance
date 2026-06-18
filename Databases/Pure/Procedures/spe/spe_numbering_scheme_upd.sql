SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_numbering_scheme_upd'
GO

CREATE PROCEDURE spe_numbering_scheme_upd
    @numbering_scheme_id int,  
    @caption_id int,  
    @code char(10),  
    @description varchar(255),  
    @is_deleted tinyint,  
    @effective_date datetime,  
    @numbering_scheme_type_id int,  
    @numbering_scheme tinyint,  
    @is_generated tinyint,  
    @mask_code varchar(20),  
    @fixed_code varchar(20),  
    @next_number int,  
    @highest_number int,  
    @step int,  
    @is_reuse_abandoned tinyint,  
    @party_type_id smallint =NULL,  
 --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
    @is_read_only tinyint,
    @is_reset_daily smallint, 
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
	--Start - Renuka - (WPR87 Paralleling)
	@is_reset_number TINYINT,
	@UserId int = NULL,
	@UniqueId varchar(50) = NULL,
	@ScreenHierarchy varchar(500) = NULL 
	--End - Renuka - (WPR87 Paralleling)
  
AS  
BEGIN  
	--Start - Renuka - (WPR87 Paralleling)
	--Get the current is reset number before updating the table
	DECLARE @existing_is_reset_number AS INT
	DECLARE @period_max_number AS INT
	DECLARE @period_next_number AS INT
	DECLARE @records_exist AS BIT
	SET @period_max_number=0
	SET @period_next_number=0
	SELECT 
		@existing_is_reset_number=ISNULL(is_reset_number,0)
	FROM
		Numbering_Scheme
	WHERE
		Numbering_Scheme_id=@numbering_scheme_id

	--If @is_reset_number=1 and mask code contains 'U', update next_number value in period_next_number table accordingly
	--In any case, if the given @next_number value is lesser than already existing value, 
	--then next_number column will not be updated.
	IF @is_reset_number=1 AND CHARINDEX('U',UPPER(@mask_code),0)<>0
	BEGIN
		IF @existing_is_reset_number=0
		BEGIN
			--Check period_next_number table has value for this numbering scheme	
			IF EXISTS(	
						SELECT 
							1
						FROM
							Period_Next_Number
						WHERE
							Numbering_Scheme_id=@numbering_scheme_id)
							
				SET @records_exist=1
			ELSE
			BEGIN
				--If there is no record in period next number table for this numbering scheme, insert records
				INSERT INTO 
					period_next_number
				SELECT DISTINCT
					@numbering_scheme_id,
					year_name,
					@next_number
				FROM
					Period
			END
		END
		IF @existing_is_reset_number=1 OR @records_exist=1
		BEGIN
			--Check records for given year
			SELECT 
				@period_next_number=ISNULL(Next_Number,0)
			FROM
				Period_Next_Number
			WHERE
				Numbering_Scheme_id=@numbering_scheme_id
				AND Year_Name=YEAR(@effective_date)
	
			--If no record then insert record
			If @period_next_number=0
			BEGIN
				INSERT INTO 
					period_next_number
				SELECT DISTINCT
					@numbering_scheme_id,
					year_name,
					@next_number
				FROM
					Period
				WHERE
					Year_Name=YEAR(@effective_date)
			END
			ELSE
			BEGIN
				--If record present, check existing next_number value<given @next_number value. If yes update the next_number
				--value in period_next_number table with given @next_number value. Else, dont update since changing the 
				--value of next number to a lesser value can create duplicate number during number generation.
				IF @period_next_number<@next_number
				BEGIN
					UPDATE
						Period_Next_Number
					SET
						Next_Number=@next_number
					WHERE
						Numbering_Scheme_id=@numbering_scheme_id
					AND Year_Name=YEAR(@effective_Date)
				END
			END	
		END
	END
	
	--If @is_reset_number=0 and @existing_is_reset_number=1 update the next_number in numbering_scheme table accordingly
	--In any case if the given @next_number value is lesser than already existing value, 
	--then next_number column in Numbering scheme table will be updated with higher value among the two.
	If @is_reset_number=0 AND @existing_is_reset_number=1
	BEGIN
		--Get the highest next number value available for this numbering scheme in period_next_number table	
		SELECT 
			@period_max_number=ISNULL(MAX(Next_Number),0)
		FROM
			Period_Next_Number
		WHERE
			Numbering_Scheme_id=@numbering_scheme_id

		--If @next_number<@period_max_number value, set @next_number value to the higher value
		IF @next_number<@period_max_number
			SET @next_number=@period_max_number
	END
	--End - Renuka - (WPR87 Paralleling)
 IF @party_type_id =0  
  SET @party_type_id=NULL  
  
UPDATE numbering_scheme  
    SET  
    caption_id=@caption_id,  
    code=@code,  
    description=@description,  
    is_deleted=@is_deleted,  
    effective_date=@effective_date,  
    numbering_scheme_type_id=@numbering_scheme_type_id,  
    numbering_scheme=@numbering_scheme,  
    is_generated=@is_generated,  
    mask_code=@mask_code,  
    fixed_code=@fixed_code,  
    next_number=@next_number,  
    highest_number=@highest_number,  
    step=@step,  
    is_reuse_abandoned=@is_reuse_abandoned,  
    party_type_id=@party_type_id,  
 --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.3)
    is_read_only=@is_read_only,  
    is_reset_daily=@is_reset_daily,
   --(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.3)
	--Start - Renuka - (WPR87 Paralleling)
	is_reset_number=@is_reset_number,
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
	--End - Renuka - (WPR87 Paralleling)
WHERE numbering_scheme_id = @numbering_scheme_id  
--Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.2.2)

INSERT INTO numbering_scheme_history( 
    scheme_valid_from,
    numbering_scheme_id,		 
    caption_id ,  
    code ,  
    description ,  
    is_deleted ,  
    effective_date ,  
    numbering_scheme_type_id ,  
    numbering_scheme ,  
    is_generated ,  
    mask_code ,  
    fixed_code ,  
    next_number ,  
    highest_number ,  
    step ,  
    is_reuse_abandoned,  
    party_type_id,  
 --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.3)
    is_read_only,
    is_reset_daily,
 --(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.3)
	--Start - Renuka - (WPR87 Paralleling)
	is_reset_number)
	--End - Renuka - (WPR87 Paralleling)
VALUES (
    Getdate(),
    @numbering_scheme_id,	  
    @caption_id,  
    @code,  
    @description,  
    @is_deleted,  
    @effective_date,  
    @numbering_scheme_type_id,  
    @numbering_scheme,  
    @is_generated,  
    @mask_code,  
    @fixed_code,  
    @next_number,  
    @highest_number,  
    @step,  
    @is_reuse_abandoned,  
    @party_type_id,  
 --(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.3)
    @is_read_only,
    @is_reset_daily,
 --(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.3)
	--Start - Renuka - (WPR87 Paralleling)
	@is_reset_number)
	--End - Renuka - (WPR87 Paralleling)
--End (Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.2.2)  
END  
