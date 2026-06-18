SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_sel'
GO

CREATE PROCEDURE spe_default_policy_sel
    @default_policy_id int
AS
SELECT
    default_policy_id,
    code,
    caption_id,
    description,
    is_deleted,
    effective_date,
    gis_business_type_id,
    gis_policy_link_id
 FROM default_policy
WHERE default_policy_id = @default_policy_id

GO

