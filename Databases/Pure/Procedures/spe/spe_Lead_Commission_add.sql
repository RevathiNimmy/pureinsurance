SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Lead_Commission_add'
GO

CREATE PROCEDURE spe_Lead_Commission_add
    @insurance_file_cnt int,
    @commission_band tinyint,
    @premium numeric(19,4),
    @percent numeric(12,8),
    @value numeric(19,4)
AS
BEGIN
INSERT INTO Lead_Commission (
    insurance_file_cnt ,
    commission_band ,
    premium ,
    [percent] ,
    value)
VALUES (
    @insurance_file_cnt,
    @commission_band,
    @premium,
    @percent,
    @value)
END

GO

