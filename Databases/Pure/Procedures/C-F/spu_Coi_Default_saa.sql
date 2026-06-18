SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Coi_Default_saa'
GO


CREATE PROCEDURE spu_Coi_Default_saa
AS

/* End of insurer file procedures */

/* Coinsurance procedures */
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

ORDER BY coi_default_id ASC
GO


