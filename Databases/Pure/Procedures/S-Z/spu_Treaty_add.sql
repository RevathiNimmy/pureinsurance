SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Treaty_add'
GO


CREATE PROCEDURE spu_Treaty_add
        @treaty_id int output,    
        @code char(10),    
        @description varchar(255),    
        @is_deleted tinyint,    
        @effective_date datetime,    
        @expiry_date datetime,    
        @agreement_code varchar(255),    
        @reinsurance_type_id int,    
        @replaces_treaty_id int,  
        @replaced_by_effective_date datetime,    
        @replaced_By_treaty_id int, 
		@treaty_limit DECIMAL(18,2) ,
		@currency_id INT,
		@reinstatements INT,		
        @UserId int,  
        @UniqueId varchar(50),  
        @ScreenHierarchy varchar(500)  
    AS    
    
        -- Get the caption id    
        Declare @caption_id int    
        Execute spu_pm_caption_id_return 1, @description, @caption_id output    
        
        Insert  Treaty (    
                code,    
                description,    
                caption_id,    
                is_deleted,    
                effective_date,    
                expiry_date,    
                agreement_code,    
                reinsurance_type_id,    
                replaces_treaty_id,  
                replaced_by_effective_date,  
                replaced_by_treaty_id,
				treaty_limit,
				currency_id,
				reinstatements,				
                UserId,  
                UniqueId,  
                ScreenHierarchy)    
        Values (@code,    
                @description,    
                @caption_id,    
                @is_deleted,    
                @effective_date,    
                @expiry_date,    
                @agreement_code,    
                @reinsurance_type_id,    
                @replaces_treaty_id,  
				@replaced_by_effective_date,  
                @replaced_By_treaty_id,
				@treaty_limit,
				@currency_id,
				@reinstatements,				
                @UserId,  
                @UniqueId,  
                @ScreenHierarchy)    
        
        -- Return new treaty id    
        Select  @treaty_id = SCOPE_IDENTITY()   
      

GO

