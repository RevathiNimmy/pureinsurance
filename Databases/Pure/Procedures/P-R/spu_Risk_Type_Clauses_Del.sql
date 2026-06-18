SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Risk_Type_Clauses_Del'
GO


CREATE PROCEDURE spu_Risk_Type_Clauses_Del
    @risk_type_id INT
AS


DELETE
FROM    wording_risk_type_link
WHERE   risk_type_id = @risk_type_id
GO


