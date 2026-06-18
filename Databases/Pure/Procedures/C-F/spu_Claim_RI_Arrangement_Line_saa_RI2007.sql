SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_Line_saa_RI2007'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_Line_saa_RI2007    
    @claim_id int,    
    @ri_arrangement_id int,    
    @Mode int=1  
 --Arul Stephen  
    ,@Recovery int=0    
 --End Arul Stephen  
AS    
    
DECLARE @MultiActs INT    
    
--Retrieve the Number of FX rows in the Ri_Arrangement_Line table    
SELECT  @MultiActs = Count(*)    
FROM  Claim_Ri_Arrangement_Line    
WHERE  ri_arrangement_id = @ri_arrangement_id    
AND  type = 'FX'    
    
 IF ISNULL(@MultiActs,0) = 0 OR @MultiActs <=1  --IF We have 1 or No FX Row then execute the following SP    
   EXEC spu_Claim_RI_Arrangement_Line_Select_RI2007    
        @claim_id = @claim_id,    
        @ri_arrangement_id = @ri_arrangement_id,    
        @Mode=@Mode    
 --Arul Stephen  
        ,@Recovery=@Recovery  
 --End Arul Stephen  
 ELSE IF @MultiActs>1  --IF We have 1 or No FX Row then execute the following SP    
   EXEC spu_Claim_RI_Arrangement_Line_MultiActs_RI2007    
        @claim_id = @claim_id,    
        @ri_arrangement_id = @ri_arrangement_id,    
        @Mode=@Mode    
 --Arul Stephen  
        ,@Recovery=@Recovery  
 --End Arul Stephen  
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
  
