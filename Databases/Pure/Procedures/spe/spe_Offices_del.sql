SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Offices_del'
GO

CREATE PROCEDURE spe_Offices_del
    @insurance_file_cnt int
AS
DELETE FROM Offices
WHERE insurance_file_cnt = @insurance_file_cnt

GO

