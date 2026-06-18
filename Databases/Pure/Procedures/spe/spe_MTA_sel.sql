SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_sel'
GO

CREATE PROCEDURE spe_MTA_sel
    @mta_cnt int
AS
SELECT
    mta_cnt,
    mta_id,
    source_id,
    insurance_folder_cnt,
    mta_type_id,
    is_quote,
    cover_start_date,
    expiry_date,
    date_issued,
    is_permanent,
    description,
    main_event_id,
    sub_event_id,
    created_by_id,
    date_created,
    modified_by_id,
    last_modified
 FROM MTA
WHERE mta_cnt = @mta_cnt

GO

