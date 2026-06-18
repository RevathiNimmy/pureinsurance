SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_commission_undel'
GO


CREATE PROCEDURE spu_sir_commission_undel  
    @party_type_id int,  
    @party_id int,  
    @product_id int,  
    @risk_type_id int,  
    @transaction_type_id int,  
    @commission_band_id int,  
    @commission_group_id int,  
	@Effective_Date datetime,
	@commission_level_id int,
	@UserId INT,
	@UniqueId VARCHAR(50),
	@ScreenHierarchy VARCHAR(500)
AS  
  
/********************************************************************************************************/  
/* Stored Procedure spu_sir_commission_undel                                        */  
/********************************************************************************************************/  
/* Revision             Description of Modification                                     Date        Who */  
/* --------             ---------------------------                                     ----        --- */  
/* 1.0                  SR14092000 - Created                                       */  
/* 2.0     Alix - Added effective date parameter           */  
/********************************************************************************************************/  
  
    Begin  
  
--      Delete from Commission_arrangement  
        Update Commission_arrangement  
            Set Is_deleted = 0,
			UserId = @UserId,
			UniqueId = @UniqueId,
			ScreenHierarchy = @ScreenHierarchy  
        Where Party_type = @PArty_type_id  
        And   Party_cnt = @party_id  
        And   Product_id = @product_id  
        And   Risk_type_id = @risk_type_id  
        And   Transaction_type_id = @transaction_type_id  
        And   Commission_band_id = @commission_band_id  
        And   Commission_grouping = @commission_group_id  
  And Effective_Date = @Effective_Date
  And commission_level_id =@commission_level_id   
    End  
GO