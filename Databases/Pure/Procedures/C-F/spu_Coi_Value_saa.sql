SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_Coi_Value_saa
GO

CREATE PROCEDURE spu_Coi_Value_saa
    @insurance_file_cnt INT  
AS  
BEGIN  

SELECT  c.insurance_file_cnt,
        c.coi_value_id,
        c.party_cnt,  
        c.arrangement_ref,  
        c.share_percent,  
        c.share_premium,  
        c.commission_percent,  
        c.commission_value,  
        c.surcharge_percent,  
        c.surcharge_value,  
        c.is_standard_surcharge,  
        c.premium_tax_recovery_percent,  
        c.premium_tax_recovery_value,  
        c.is_manual_premium_tax_rec,  
        p.name  
    FROM Coi_Value c
        LEFT OUTER JOIN Party p  
            ON c.party_cnt = p.party_cnt  
    WHERE   
        c.insurance_file_cnt = @insurance_file_cnt  
    ORDER BY 
        coi_value_id ASC

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO