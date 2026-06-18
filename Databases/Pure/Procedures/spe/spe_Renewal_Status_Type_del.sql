SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_Type_del'
GO

CREATE PROCEDURE spe_Renewal_Status_Type_del
    @renewal_status_type_id int
AS
DELETE FROM Renewal_Status_Type
WHERE renewal_status_type_id = @renewal_status_type_id

GO

