SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_RI_Arrangement_upd'
GO

CREATE PROCEDURE spu_Claim_RI_Arrangement_upd  
    @claim_id int,  
    @ri_arrangement_id int,  
    @is_modified tinyint  
AS  
  
    Update  claim_ri_arrangement  
    Set     is_modified = @is_modified  
    Where   claim_id = @claim_id  
    And     ri_arrangement_id = @ri_arrangement_id  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
