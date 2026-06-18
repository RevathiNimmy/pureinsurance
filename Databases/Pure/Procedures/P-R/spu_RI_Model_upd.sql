SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Model_upd'
GO


CREATE PROCEDURE spu_RI_Model_upd
    @ri_model_id int,
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
    @pmuser_id smallint,
    @audit_ri_model_id int output,
    @UserId int,  
    @UniqueId varchar(50),  
    @ScreenHierarchy varchar(500),
	@treaty_premium_type tinyint = 0
AS

    --copy the original ri_model record into Audit_ri_model table before updating (QBENZ005)
    Insert into Audit_ri_model 
	Select RI_model_id ,
		code ,
		Description,
		caption_id,
		effective_date,
		is_deleted ,
		getdate() ,
		@pmuser_id,
		expiry_date ,
		ri_model_type ,
		FAC_premium_type,
		Claim_Allocation_type ,
		Currency_id,
		xol_clm_ri_model_id,
		xol_clm_limit,
		xol_cat_ri_model_id ,
		xol_cat_limit ,
		xol_cat_reinstatements,
		treaty_premium_type
	From RI_Model 
	Where RI_model_id= @ri_model_id

Set @audit_ri_model_id = @@identity

    Declare @caption_id int
    Execute spu_pm_caption_id_return 1, @description, @caption_id output

    Update  RI_Model
    Set     code = @code,
            description = @description,
            caption_id = @caption_id,
            is_deleted = @is_deleted,
            effective_date = @effective_date,
            expiry_date = @expiry_date,
            ri_model_type = @ri_model_type,
            fac_premium_type = @fac_premium_type,
            claim_allocation_type = @claim_allocation_type,
            currency_id = @currency_id,
            xol_clm_ri_model_id = @xol_clm_ri_model_id, 
            xol_clm_limit = @xol_clm_limit,
            xol_cat_ri_model_id = @xol_cat_ri_model_id, 
            xol_cat_limit = @xol_cat_limit,
            xol_cat_reinstatements = @xol_cat_reinstatements,            
            UserId = @UserId,
            UniqueId = @UniqueId,
            ScreenHierarchy = @screenHierarchy,
			treaty_premium_type = @treaty_premium_type
    Where   ri_model_id = @ri_model_id

GO

