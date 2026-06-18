SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Rep_TB'
GO


CREATE PROCEDURE spu_ACT_Select_Rep_TB
    @rep_TB_id int
AS


SELECT
    rep_TB_id,
    reportheader_id,
    accounting_date
FROM Rep_TB
WHERE rep_TB_id = @rep_TB_id
GO


