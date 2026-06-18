SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_commission_del'
GO


CREATE PROCEDURE spu_sir_commission_del  
    @party_type_id int,  
    @party_id int,  
    @product_id int,  
    @risk_type_id int,  
    @transaction_type_id int,  
    @commission_band_id int,  
    @commission_group_id int,  
 @Effective_Date datetime ,
 @commission_level_id int = 0,
 @UserId INT,
 @UniqueId VARCHAR(50),
 @ScreenHierarchy VARCHAR(500)
AS  
  
/********************************************************************************************************/  
/* Stored Procedure spu_sir_commission_del                                          */  
/********************************************************************************************************/  
/* Revision             Description of Modification                                     Date        Who */  
/* --------             ---------------------------                                     ----        --- */  
/* 1.0                  SR11092000 - Created                                       */  
/* 1.1                  PB/CMG12072002 - Commission Grouping added                 */  
/* 2.0     Alix - Added effective date parameter           */  
/* 3.0     Nexus change - to prevent user from changing SSP commission 08/02/08    RDT  */  
/********************************************************************************************************/  
  
    Begin  
  
--      Delete from Commission_arrangement  
    Update Commission_arrangement  
        Set Is_deleted = 1,
		UserId = @UserId,
		UniqueId = @UniqueId,
		ScreenHierarchy = @ScreenHierarchy
 From Commission_arrangement ca  
 left outer join Party_agent pa on pa.party_cnt = ca.party_cnt  
        Where Party_type = @PArty_type_id  
        And   ca.Party_cnt = @party_id  
        And   ca.Product_id = @product_id  
        And   ca.Risk_type_id = @risk_type_id  
        And   ca.Transaction_type_id = @transaction_type_id  
        And   ca.Commission_band_id = @commission_band_id  
        And   ca.commission_grouping = @commission_group_id  
 And   ca.Effective_Date = @Effective_Date 
 And   ca.commission_level_id =@commission_level_id  
 And   IsNull(pa.is_ssp_subagent,0) <> 1  
  
    End  
	
GO