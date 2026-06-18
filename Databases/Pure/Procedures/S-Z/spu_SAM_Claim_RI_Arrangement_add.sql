SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Claim_RI_Arrangement_add'
GO

CREATE PROCEDURE spu_SAM_Claim_RI_Arrangement_add  
 @ri_arrangement_id int output,  
 @claim_id int,  
 @risk_cnt int,  
 @claim_allocation_type tinyint,  
 @ri_band_id int,  
 @ri_model_id int,  
 @sum_insured money,  
 @reserve money,  
 @payment money,  
 @salvage money,  
 @recovery money,  
 @is_modified int,  
 @this_reserve money,  
 @this_payment money,  
 @this_salvage money,  
 @this_recovery money,  
 @original_ri_arrangement_id int,  
 @ri_arrangement_version int  
  
AS  
  
 DECLARE @version_id int  
 DECLARE @claim_ri_arrangement_id int  
  
 EXEC spu_CLM_Get_Claim_version  
  @claim_id = @claim_id,  
  @version_id = @version_id OUTPUT  
  
 -- this value is just an initial default it is overriden by the update further down  
 -- Get new id (we don't have an identity column in claims)  
 SELECT  @ri_arrangement_id = ISNULL(MAX(ri_arrangement_id), 0) + 1  
 FROM    claim_ri_arrangement  
 WHERE   claim_id = @claim_id  
  
 -- Insert record  
 INSERT  claim_ri_arrangement(  
 claim_id,      
 ri_arrangement_id,   
 risk_cnt,      
 ri_band_id,    
 ri_model_id,   
 claim_allocation_type,   
 sum_insured,             
 reserve,                 
 payment,                 
 salvage,                 
 recovery,               
 is_modified,   
 this_reserve,            
 this_payment,            
 this_salvage,            
 this_recovery,           
 version_id,    
 original_ri_arrangement_id,   
 ri_arrangement_version)  
 VALUES (  
 @claim_id,      
 @ri_arrangement_id,   
 @risk_cnt,      
 @ri_band_id,    
 @ri_model_id,   
 @claim_allocation_type,   
 @sum_insured,             
 @reserve,                 
 @payment,                 
 @salvage,                 
 @recovery,               
 @is_modified,   
 @this_reserve,            
 @this_payment,            
 @this_salvage,            
 @this_recovery,           
 @version_id,    
 @original_ri_arrangement_id,   
 @ri_arrangement_version)  
  
 SELECT @claim_ri_arrangement_id = @@IDENTITY  
  
 UPDATE claim_ri_arrangement  
 SET base_claim_ri_arrangement_id = @claim_ri_arrangement_id ,  
     ri_arrangement_id = @claim_ri_arrangement_id  
 WHERE claim_ri_arrangement_id = @claim_ri_arrangement_id  
  
 SELECT  @ri_arrangement_id = @claim_ri_arrangement_id



GO
