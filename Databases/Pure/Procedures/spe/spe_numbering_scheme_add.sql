SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_numbering_scheme_add'
GO

CREATE PROCEDURE spe_numbering_scheme_add
    @numbering_scheme_id int OUTPUT ,  
    @caption_id int ,  
    @code char(10) ,  
    @description varchar(255) ,  
    @is_deleted tinyint ,  
    @effective_date datetime ,  
    @numbering_scheme_type_id int ,  
    @numbering_scheme tinyint ,  
    @is_generated tinyint ,  
    @mask_code varchar(20) ,  
    @fixed_code varchar(20) ,  
    @next_number int ,  
    @highest_number int ,  
    @step int ,  
    @is_reuse_abandoned tinyint,  
    @party_type_id smallint= NULL,  
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
  
  
IF @party_type_id = 0  
  Set @party_type_id = NULL  
  
INSERT INTO numbering_scheme (  
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
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
    is_read_only,
    is_reset_daily,
--(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
	--Start - Renuka - (WPR87 Paralleling)
	is_reset_number,
	UserId,
	UniqueId,
	ScreenHierarchy)
	--End - Renuka - (WPR87 Paralleling)
VALUES (  
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
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)  
    @is_read_only,
    @is_reset_daily,
  --(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
	--Start - Renuka - (WPR87 Paralleling)
	@is_reset_number,
	@UserId,
	@UniqueId,
	@ScreenHierarchy)
	--End - Renuka - (WPR87 Paralleling)
SELECT @numbering_scheme_id=SCOPE_IDENTITY()  
--Start(Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.2.1)
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
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
    is_read_only,
    is_reset_daily,
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
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
--(Start)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)
    @is_read_only,
    @is_reset_daily,
  --(End)-(Arul Stephen)-(Tech Spec - LOA002 - Unique EFT Number.doc)-(6.1.2.1)  
	--Start - Renuka - (WPR87 Paralleling)
	@is_reset_number)
	--End - Renuka - (WPR87 Paralleling)
--End (Saurabh Agrawal) Tech Spec VAL P14 Policy Numbering (6.2.1)
--Start - Renuka - (WPR87 Paralleling)
--If @is_reset_number is set and numbering scheme has 'U' then insert data into period_next_number table for 
--all the years exists with this numbering scheme id and next number as a number defined in this numbering scheme
IF @is_reset_number=1 AND CHARINDEX('U',UPPER(@mask_code),0)<>0
BEGIN
	INSERT INTO 
		period_next_number
	SELECT DISTINCT
		@numbering_scheme_id,
		year_name,
		@next_number
	FROM
		Period
END
--End - Renuka - (WPR87 Paralleling)
END
