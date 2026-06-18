SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Renewal_Status_Type_sel'
GO

CREATE PROCEDURE spe_Renewal_Status_Type_sel
    @renewal_status_type_id int
AS
SELECT
    renewal_status_type_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date
 FROM Renewal_Status_Type
WHERE renewal_status_type_id = @renewal_status_type_id

GO

