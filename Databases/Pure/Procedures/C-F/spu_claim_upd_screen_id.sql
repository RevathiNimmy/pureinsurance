SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_claim_upd_screen_id'
GO

CREATE PROCEDURE spu_claim_upd_screen_id  
    @claim_id integer,  
    @screen_id integer,  
    @peril_level tinyint  
AS  
BEGIN  
    IF @peril_level = 0 BEGIN  
        UPDATE claim  
           SET gis_screen_id = @screen_id  
         WHERE claim_id = @claim_id  
    END  
    ELSE  
    BEGIN  
        UPDATE claim_peril  
           SET gis_screen_id = @screen_id  
         WHERE claim_peril_id = @claim_id  
    END  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
