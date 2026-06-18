SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Treaty_upd'
GO


CREATE PROCEDURE spu_Treaty_upd
    @treaty_id int,  
    @code char(10),  
    @description varchar(255),  
    @is_deleted tinyint,  
    @effective_date datetime,  
    @expiry_date datetime,  
    @agreement_code varchar(255),  
    @reinsurance_type_id int, 
	@treaty_limit DECIMAL(18,2) ,
    @currency_id INT,
    @reinstatements INT,	
    @replaces_treaty_id int,
    @replaced_by_effective_date datetime,  
    @replaced_by_treaty_id int,
	@UserId int,  
	@UniqueId varchar(50),  
	@ScreenHierarchy varchar(500)  
    	  
AS  
  
    Declare @caption_id int  
    Execute spu_pm_caption_id_return 1, @description, @caption_id output  
  
    Update  Treaty  
    Set     code = @code,  
            description = @description,  
            caption_id = @caption_id,  
            is_deleted = @is_deleted,  
            effective_date = @effective_date,  
            expiry_date = @expiry_date,  
            agreement_code = @agreement_code,  
            reinsurance_type_id = @reinsurance_type_id,  
            replaces_treaty_id = @replaces_treaty_id,
			replaced_by_effective_date = @replaced_by_effective_date, 
            replaced_by_treaty_id = @replaced_by_treaty_id,
			treaty_limit = @treaty_limit,
			currency_id = @currency_id,
			reinstatements = @reinstatements,
			UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy = @ScreenHierarchy

    Where   treaty_id = @treaty_id  


GO