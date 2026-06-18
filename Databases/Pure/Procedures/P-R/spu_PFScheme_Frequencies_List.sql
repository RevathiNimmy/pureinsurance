EXECUTE DDLDropProcedure 'spu_PFScheme_Frequencies_List'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_PFScheme_Frequencies_List

AS BEGIN

    SELECT pfFrequency_id, description,
        CASE period
        WHEN 'm' then 12/amount
        ELSE  52/amount
        END AS DisplayOrder
    FROM pfFrequency
    WHERE is_deleted=0
    AND is_available_on_instalment_screen =1
    ORDER BY DisplayOrder DESC
END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO