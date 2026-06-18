SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON 
GO


EXECUTE DDLDropProcedure 'spu_RiskType_RI_Model_Is_Deferred_Selall'
EXECUTE DDLDropProcedure 'spu_RiskType_RI_Model_Is_Deferred_saa'
EXECUTE DDLDropProcedure 'spu_Risk_Type_RI_Model_Is_Deferred_saa'
GO


CREATE PROCEDURE spu_Risk_Type_RI_Model_Is_Deferred_saa
    @is_deferred tinyint
AS
    
    If ISNULL(@is_deferred, 0) = 0
        SELECT  m.ri_model_id,
                m.code,
                m.[description],
                m.is_deleted,
                m.effective_date,
                0 
        FROM    ri_model AS m
       WHERE   m.ri_model_type IN(0,4) -- normal only        
        AND     m.is_deleted = 0
        ORDER BY 
                m.[description] ASC
    Else
        SELECT  m.ri_model_id,
                m.code,
                m.[description],
                m.is_deleted,
                m.effective_date,
                1
        FROM    ri_model AS m
        WHERE   m.ri_model_type = 2 -- deferred only        
        AND     m.is_deleted = 0
        ORDER BY 
                m.[description] ASC

    
GO


