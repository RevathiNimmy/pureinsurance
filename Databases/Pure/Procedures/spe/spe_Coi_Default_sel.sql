SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Default_sel'
GO

CREATE PROCEDURE spe_Coi_Default_sel
    @coi_default_id int
AS
SELECT
    coi_default_id,
    code,
    caption_id,
    description,
    source_id,
    is_recovered,
    is_recovered_overrideable,
    is_surcharged,
    is_surcharged_overrideable,
    standard_surcharge_percent,
    compulsory_coi_party_cnt,
    compulsory_coi_com_percent,
    effective_date,
    is_deleted
 FROM Coi_Default
WHERE coi_default_id = @coi_default_id

GO

