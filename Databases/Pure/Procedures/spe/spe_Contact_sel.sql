SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Contact_sel'
GO

CREATE PROCEDURE spe_Contact_sel
    @contact_cnt int
AS
SELECT
    contact_cnt,
    contact_type_id,
    source_id,
    contact_id,
    country_id,
    description,
    area_code,
    number,
    extension,
    created_by_id,
    date_created,
    modified_by_id,
    last_modified
 FROM Contact
WHERE contact_cnt = @contact_cnt

GO

