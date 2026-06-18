SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Rep_TB'
GO


CREATE PROCEDURE spu_ACT_SelAll_Rep_TB
AS


SELECT
    rep_TB_id,
    reportheader_id,
    accounting_date
FROM Rep_TB
GO


