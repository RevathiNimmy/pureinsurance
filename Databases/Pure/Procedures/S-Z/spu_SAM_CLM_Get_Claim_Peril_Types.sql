SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Get_Claim_Peril_Types'
GO

CREATE PROCEDURE spu_SAM_CLM_Get_Claim_Peril_Types

@risk_cnt integer

AS

BEGIN

    SELECT  distinct p.peril_type_id,  
	    pt.code, 
            pt.description
    FROM    peril p  
    JOIN    peril_type pt     ON pt.peril_type_id = p.peril_type_id  
    JOIN    rating_section rs ON rs.rating_section_id = p.rating_section_id  
                             AND rs.risk_cnt = p.risk_cnt  
    WHERE   p.risk_cnt = @risk_cnt  
    AND     rs.original_flag = 0  
    GROUP BY  
            p.peril_type_id,  
            pt.description, 
	    pt.code

END



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
