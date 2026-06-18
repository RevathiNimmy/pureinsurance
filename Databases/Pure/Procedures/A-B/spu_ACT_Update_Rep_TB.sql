SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_Rep_TB'
GO


CREATE PROCEDURE spu_ACT_Update_Rep_TB
    @rep_TB_id int,
    @reportheader_id int,
    @accounting_date datetime
AS


BEGIN
UPDATE Rep_TB
    SET
    reportheader_id=@reportheader_id,
    accounting_date=@accounting_date
WHERE rep_TB_id = @rep_TB_id
END
GO


