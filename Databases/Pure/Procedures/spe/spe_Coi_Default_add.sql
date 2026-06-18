SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Coi_Default_add'
GO

CREATE PROCEDURE spe_Coi_Default_add
    @coi_default_id int,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @source_id smallint,
    @is_recovered tinyint,
    @is_recovered_overrideable tinyint,
    @is_surcharged tinyint,
    @is_surcharged_overrideable tinyint,
    @standard_surcharge_percent numeric(12,8),
    @compulsory_coi_party_cnt int,
    @compulsory_coi_com_percent numeric(12,8),
    @effective_date datetime,
    @is_deleted tinyint
AS
BEGIN
INSERT INTO Coi_Default (
    coi_default_id ,
    code ,
    caption_id ,
    description ,
    source_id ,
    is_recovered ,
    is_recovered_overrideable ,
    is_surcharged ,
    is_surcharged_overrideable ,
    standard_surcharge_percent ,
    compulsory_coi_party_cnt ,
    compulsory_coi_com_percent ,
    effective_date ,
    is_deleted )
VALUES (
    @coi_default_id,
    @code,
    @caption_id,
    @description,
    @source_id,
    @is_recovered,
    @is_recovered_overrideable,
    @is_surcharged,
    @is_surcharged_overrideable,
    @standard_surcharge_percent,
    @compulsory_coi_party_cnt,
    @compulsory_coi_com_percent,
    @effective_date,
    @is_deleted)
END

GO

