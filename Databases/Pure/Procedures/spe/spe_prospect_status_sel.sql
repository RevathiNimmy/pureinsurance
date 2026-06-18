SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_prospect_status_sel'
GO

CREATE PROCEDURE spe_prospect_status_sel
    @prospect_status_id int
AS
SELECT
    prospect_status_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date
 FROM prospect_status
WHERE prospect_status_id = @prospect_status_id

GO

