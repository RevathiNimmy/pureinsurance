SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Add_Rep_TB'
GO


CREATE PROCEDURE spu_ACT_Add_Rep_TB
    @rep_TB_id int OUTPUT,
    @reportheader_id int,
    @accounting_date datetime
AS


BEGIN
INSERT INTO Rep_TB (
    reportheader_id,
    accounting_date)
VALUES (
    @reportheader_id,
    @accounting_date)
END
BEGIN
SELECT @rep_TB_id = @@IDENTITY
END
GO


