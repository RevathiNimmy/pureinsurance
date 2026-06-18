SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_default_policy_upd'
GO

CREATE PROCEDURE spe_default_policy_upd
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
UPDATE default_policy
    SET
    code=@code,
    caption_id=@caption_id,
    description=@description,
    is_deleted=@is_deleted,
    effective_date=@effective_date,
    gis_business_type_id=@gis_business_type_id,
    gis_policy_link_id=@gis_policy_link_id
WHERE default_policy_id = @default_policy_id
END

GO

