SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Model_add'
GO


CREATE PROCEDURE spu_RI_Model_add
    @ri_model_id int output,
    @code char(10),
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @expiry_date datetime,
    @ri_model_type tinyint,
    @fac_premium_type tinyint,
    @claim_allocation_type tinyint,
    @currency_id smallint,
    @xol_clm_ri_model_id int, 
    @xol_clm_limit money,
    @xol_cat_ri_model_id int, 
    @xol_cat_limit money,
    @xol_cat_reinstatements smallint,
    @UserId int,  
    @UniqueId varchar (50),  
    @ScreenHierarchy varchar(500),
	@treaty_premium_type tinyint = 0
AS

    -- Get the caption id
    Declare @caption_id int
    Execute spu_pm_caption_id_return 1, @description, @caption_id output

    Insert  RI_Model (
            code,
            description,
            caption_id,
            is_deleted,
            effective_date,
            expiry_date,
            ri_model_type,
            fac_premium_type,
            claim_allocation_type,
            currency_id,
            xol_clm_ri_model_id, 
            xol_clm_limit,
            xol_cat_ri_model_id, 
            xol_cat_limit,
            xol_cat_reinstatements,           
            UserId,  
            UniqueId,  
            ScreenHierarchy,
			treaty_premium_type)
    Values (@code,
            @description,
            @caption_id,
            @is_deleted,
            @effective_date,
            @expiry_date,
            @ri_model_type,
            @fac_premium_type,
            @claim_allocation_type,
            @currency_id,
            @xol_clm_ri_model_id, 
            @xol_clm_limit,
            @xol_cat_ri_model_id, 
            @xol_cat_limit,
            @xol_cat_reinstatements,
            @UserId,  
            @UniqueId,  
            @ScreenHierarchy,
			@treaty_premium_type)

    -- Return new ri model id
    Select  @ri_model_id = SCOPE_IDENTITY()

GO

