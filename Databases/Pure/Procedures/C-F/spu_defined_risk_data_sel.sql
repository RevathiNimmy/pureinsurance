SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_defined_risk_data_sel
GO

CREATE PROCEDURE spu_defined_risk_data_sel
    @risk_group_id INT,  
    @source_id INT  
AS
BEGIN  

    SELECT
        drd.defined_risk_data_id,
        drd.level_1,
        drd.level_2,
        drd.display_order,
        drd.code,
        drd.description,
        drd.caption,
        drd.type,  
        count(insurance_file_cnt) as howmany  
    FROM defined_risk_data drd
        LEFT OUTER JOIN user_defined_risk_data udrd  
            ON udrd.defined_risk_data_id = drd.defined_risk_data_id  
    WHERE   drd.risk_group_id = @risk_group_id  
        AND drd.source_id = @source_id  
    GROUP BY drd.defined_risk_data_id,  
             drd.level_1,  
             drd.level_2,  
             drd.display_order,  
             drd.code,  
             drd.description,  
             drd.caption,  
             drd.type  
    ORDER BY drd.level_1,  
             drd.level_2  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
