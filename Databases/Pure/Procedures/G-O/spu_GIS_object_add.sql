SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_GIS_object_add'
GO


CREATE PROCEDURE spu_GIS_object_add
    @gis_object_id int OUTPUT,
    @gis_data_model_id int,
    @object_name varchar(70),
    @table_name varchar(70),
    @max_instances int,
    @is_quote_object tinyint,
    @parent_object_id int,
    @polaris_object_id int,
    @is_selectable_for_screen tinyint,
    @is_non_gis tinyint,
    @Edit_Flags tinyint,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS

BEGIN
IF @GIS_object_id = 0
                SELECT @GIS_object_id = NULL

IF @GIS_object_id IS NULL
                SELECT @GIS_object_id = MAX(GIS_object_id) + 1
    FROM GIS_object

IF @GIS_object_id IS NULL
    SELECT @GIS_object_id = 1

INSERT INTO GIS_object (
    gis_object_id ,
    gis_data_model_id ,
    object_name ,
    table_name ,
    max_instances ,
    is_quote_object ,
    parent_object_id ,
    polaris_object_id ,
    is_selectable_for_screen ,
    is_non_gis,
    Edit_Flags)
VALUES (
    @gis_object_id,
    @gis_data_model_id,
    @object_name,
    @table_name,
    @max_instances,
    @is_quote_object,
    @parent_object_id,
    @polaris_object_id,
    @is_selectable_for_screen,
    @is_non_gis,
    @Edit_Flags)
END

BEGIN

SELECT GIS_object_id = @GIS_object_id

END

IF @UniqueId IS NOT NULL
    BEGIN
        DECLARE @Configuration_Audit_Master_Id INT;
        DECLARE @UpdateDate VARCHAR(30);

        SET @UpdateDate = CONVERT(VARCHAR(30), GETDATE(), 126);

        SELECT @Configuration_Audit_Master_Id = cam.configuration_audit_master_id
        FROM configuration_audit_master cam
        WHERE cam.UniqueId = @UniqueId;

        IF @Configuration_Audit_Master_Id IS NULL
        BEGIN
            INSERT INTO configuration_audit_master
            (
                UniqueId,
                Module_Id,
                ModuleName,
                UpdateDate,
                UserID
            )
            VALUES
            (
                @UniqueId,
                13,
                'Lookup Maintenance',
                @UpdateDate,
                @UserId
            );

            SET @Configuration_Audit_Master_Id = SCOPE_IDENTITY();
        END;

        IF NOT EXISTS
        (
            SELECT 1
            FROM configuration_audit_details cad
            WHERE cad.configuration_audit_master_id = @Configuration_Audit_Master_Id
              AND cad.Type              = 'I'
              AND cad.TableName         = 'GIS_object'
              AND cad.key_field_name    = 'gis_object_id'
              AND cad.key_field_value   = @gis_object_id
              AND cad.key_field_desc = @ScreenHierarchy
              AND cad.FieldName         = 'New GIS Object Added'
              AND cad.FieldDisplayName  = 'New GIS Object Added'
              AND cad.OldValue IS NULL
              AND cad.NewValue IS NULL
        )
        BEGIN
            INSERT INTO configuration_audit_details
            (
                configuration_audit_master_id,
                Type,
                TableName,
                key_field_name,
                key_field_value,
                key_field_desc,
                FieldName,
                FieldDisplayName,
                OldValue,
                NewValue
            )
            VALUES
            (
                @Configuration_Audit_Master_Id,
                'I',
                'GIS_object',
                'gis_object_id',
                @gis_object_id,
                @ScreenHierarchy,
                'New GIS Object Added',
                'New GIS Object Added',
                NULL,
                NULL
            );
        END
    END;
GO