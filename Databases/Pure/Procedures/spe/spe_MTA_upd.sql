SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_upd'
GO

CREATE PROCEDURE spe_MTA_upd
    @mta_cnt int,
    @mta_id int,
    @source_id smallint,
    @insurance_folder_cnt int,
    @mta_type_id int,
    @is_quote tinyint,
    @cover_start_date datetime,
    @expiry_date datetime,
    @date_issued datetime,
    @is_permanent tinyint,
    @description varchar(255),
    @main_event_id int,
    @sub_event_id int,
    @created_by_id smallint,
    @date_created datetime,
    @modified_by_id smallint,
    @last_modified datetime
AS
BEGIN
UPDATE MTA
    SET
    mta_id=@mta_id,
    source_id=@source_id,
    insurance_folder_cnt=@insurance_folder_cnt,
    mta_type_id=@mta_type_id,
    is_quote=@is_quote,
    cover_start_date=@cover_start_date,
    expiry_date=@expiry_date,
    date_issued=@date_issued,
    is_permanent=@is_permanent,
    description=@description,
    main_event_id=@main_event_id,
    sub_event_id=@sub_event_id,
    created_by_id=@created_by_id,
    date_created=@date_created,
    modified_by_id=@modified_by_id,
    last_modified=@last_modified
WHERE mta_cnt = @mta_cnt
END

GO

