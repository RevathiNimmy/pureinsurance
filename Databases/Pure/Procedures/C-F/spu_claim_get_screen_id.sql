SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claim_get_screen_id'
GO

CREATE PROCEDURE spu_claim_get_screen_id  
    @claim_id integer,  
    @peril_level tinyint  
AS  
BEGIN  
    IF @peril_level = 0 BEGIN  
        SELECT gis_screen_id  
          FROM claim  
         WHERE claim_id = @claim_id  
    END  
    ELSE  
    BEGIN  
        SELECT gis_screen_id  
          FROM claim_peril  
         WHERE claim_peril_id = @claim_id  
    END  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
