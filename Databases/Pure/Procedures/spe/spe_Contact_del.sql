SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Contact_del'
GO

CREATE PROCEDURE spe_Contact_del
    @contact_cnt int
AS
DELETE FROM Contact
WHERE contact_cnt = @contact_cnt

GO

