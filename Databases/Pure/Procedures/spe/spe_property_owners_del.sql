SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_property_owners_del'
GO

CREATE PROCEDURE spe_property_owners_del
    @insurance_file_cnt int
AS
DELETE FROM property_owners
WHERE insurance_file_cnt = @insurance_file_cnt

GO

