SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_MTA_add'
GO

CREATE PROCEDURE spe_MTA_add
    @mta_cnt int OUTPUT ,
    @mta_id int ,
    @source_id smallint ,
    @insurance_folder_cnt int ,
    @mta_type_id int ,
    @is_quote tinyint ,
    @cover_start_date datetime ,
    @expiry_date datetime ,
    @date_issued datetime ,
    @is_permanent tinyint ,
    @description varchar(255) ,
    @main_event_id int ,
    @sub_event_id int ,
    @created_by_id smallint ,
    @date_created datetime ,
    @modified_by_id smallint ,
    @last_modified datetime
AS
BEGIN
IF @source_id = 0
                SELECT @source_id = 1
IF @source_id IS NULL
                SELECT @source_id = 1
IF @MTA_id = 0
                SELECT @MTA_id = NULL
IF @MTA_id IS NULL
    SELECT @MTA_id = MAX(MTA_id) + 1
        FROM MTA

        WHERE source_id = @source_id
IF @MTA_id IS NULL
    SELECT @MTA_id = 1
INSERT INTO MTA (
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
    last_modified)
VALUES (
    @mta_id,
    @source_id,
    @insurance_folder_cnt,
    @mta_type_id,
    @is_quote,
    @cover_start_date,
    @expiry_date,
    @date_issued,
    @is_permanent,
    @description,
    @main_event_id,
    @sub_event_id,
    @created_by_id,
    @date_created,
    @modified_by_id,
    @last_modified)
END
BEGIN
SELECT @mta_cnt = @@IDENTITY
END

GO

