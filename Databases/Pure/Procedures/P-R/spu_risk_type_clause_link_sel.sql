SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_risk_type_clause_link_sel
GO

CREATE PROCEDURE spu_risk_type_clause_link_sel
    @clause_id INT,  
    @language_id INT,  
    @effective_date DATETIME  
AS  
  
/* Start of document production */  
BEGIN  

    SELECT  
        rt.risk_type_id,  
        rt.code,  
        c.caption,  
        wrtl.document_template_id  
    FROM  
        risk_type rt
        INNER JOIN PMCaption c
            ON rt.caption_id = c.caption_id 
            AND rt.effective_date <= @effective_date
            AND c.language_id = @language_id   
        LEFT OUTER JOIN wording_risk_type_link wrtl
            ON rt.risk_type_id = wrtl.risk_type_id
            AND wrtl.document_template_id = @clause_id  
    ORDER BY  
        rt.code ASC  
  
END  

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO
