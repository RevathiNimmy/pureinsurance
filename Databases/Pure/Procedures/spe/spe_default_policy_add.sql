SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_add'
GO

CREATE PROCEDURE spe_default_policy_add
    @default_policy_id int,
    @code char(10),
    @caption_id int,
    @description varchar(255),
    @is_deleted tinyint,
    @effective_date datetime,
    @gis_business_type_id int,
    @gis_policy_link_id int
AS
BEGIN
INSERT INTO default_policy (
    default_policy_id,
    code,
    caption_id,
    description,
    is_deleted,
    effective_date,
    gis_business_type_id,
    gis_policy_link_id)
VALUES (
    @default_policy_id,
    @code,
    @caption_id,
    @description,
    @is_deleted,
    @effective_date,
    @gis_business_type_id,
    @gis_policy_link_id)
END

GO

