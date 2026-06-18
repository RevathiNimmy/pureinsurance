SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_GIS_object_upd'
GO

CREATE PROCEDURE spe_GIS_object_upd
(
    @gis_object_id              INT,
    @gis_data_model_id          INT,
    @object_name                VARCHAR(70),
    @table_name                 VARCHAR(70),
    @max_instances              INT,
    @is_quote_object            TINYINT,
    @parent_object_id           INT,
    @polaris_object_id          INT,
    @is_selectable_for_screen   TINYINT,
    @is_non_gis                 TINYINT,
    @Edit_Flags                 TINYINT,
    @UserId                     INT = NULL,
    @UniqueId                   VARCHAR(50) = NULL,
    @ScreenHierarchy            VARCHAR(500) = NULL
)
AS
BEGIN

    /* --- Audit master --- */
    IF @UniqueId IS NOT NULL
    BEGIN

		DECLARE
			@old_gis_data_model_id        INT,
			@old_object_name              VARCHAR(70),
			@old_table_name               VARCHAR(70),
			@old_max_instances            INT,
			@old_is_quote_object          TINYINT,
			@old_parent_object_id         INT,
			@old_polaris_object_id        INT,
			@old_is_selectable_for_screen TINYINT,
			@old_is_non_gis               TINYINT,
			@old_Edit_Flags               TINYINT;

		SELECT
			@old_gis_data_model_id        = g.gis_data_model_id,
			@old_object_name              = g.object_name,
			@old_table_name               = g.table_name,
			@old_max_instances            = g.max_instances,
			@old_is_quote_object          = g.is_quote_object,
			@old_parent_object_id         = g.parent_object_id,
			@old_polaris_object_id        = g.polaris_object_id,
			@old_is_selectable_for_screen = g.is_selectable_for_screen,
			@old_is_non_gis               = g.is_non_gis,
			@old_Edit_Flags               = g.Edit_Flags
		FROM GIS_object g
		WHERE g.gis_object_id = @gis_object_id;

		IF @old_gis_data_model_id IS NULL AND NOT EXISTS (SELECT 1 FROM GIS_object WHERE gis_object_id = @gis_object_id)
			RETURN;

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

        /* Insert audit rows only for changed fields */
        IF ISNULL(@old_object_name, '') <> ISNULL(@object_name, '')
        BEGIN
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_object', 'gis_object_id', @gis_object_id, @ScreenHierarchy,
             'object_name', 'Object Name', @old_object_name, @object_name);
        END;

        IF ISNULL(@old_max_instances, -1) <> ISNULL(@max_instances, -1)
        BEGIN
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_object', 'gis_object_id', @gis_object_id, @ScreenHierarchy,
             'max_instances', 'Max Instances', CONVERT(VARCHAR(50), @old_max_instances), CONVERT(VARCHAR(50), @max_instances));
        END;

        IF ISNULL(@old_parent_object_id, -1) <> ISNULL(@parent_object_id, -1)
        BEGIN
			DECLARE @old_parent_object_name VARCHAR(70);
			DECLARE @new_parent_object_name VARCHAR(70);

			SELECT @old_parent_object_name = g.object_name
			FROM GIS_object g
			WHERE g.gis_object_id = @old_parent_object_id;

			SELECT @new_parent_object_name = g.object_name
			FROM GIS_object g
			WHERE g.gis_object_id = @parent_object_id;

            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_object', 'gis_object_id', @gis_object_id, @ScreenHierarchy,
             'parent_object_id', 'Parent Object', @old_parent_object_name, CONVERT(VARCHAR(50), @new_parent_object_name));
        END;

        IF ISNULL(@old_polaris_object_id, -1) <> ISNULL(@polaris_object_id, -1)
        BEGIN
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_object', 'gis_object_id', @gis_object_id, @ScreenHierarchy,
             'polaris_object_id', 'Polaris Object', CONVERT(VARCHAR(50), @old_polaris_object_id), CONVERT(VARCHAR(50), @polaris_object_id));
        END;

        IF ISNULL(@old_is_selectable_for_screen, -1) <> ISNULL(@is_selectable_for_screen, -1)
        BEGIN
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_object', 'gis_object_id', @gis_object_id, @ScreenHierarchy,
             'is_selectable_for_screen', 'Selectable For Screen',
             CONVERT(VARCHAR(50), @old_is_selectable_for_screen), CONVERT(VARCHAR(50), @is_selectable_for_screen));
        END;
    END;

    UPDATE GIS_object
       SET gis_data_model_id        = @gis_data_model_id,
           object_name              = @object_name,
           table_name               = @table_name,
           max_instances            = @max_instances,
           is_quote_object          = @is_quote_object,
           parent_object_id         = @parent_object_id,
           polaris_object_id        = @polaris_object_id,
           is_selectable_for_screen = @is_selectable_for_screen,
           is_non_gis               = @is_non_gis,
           Edit_Flags               = @Edit_Flags
        WHERE gis_object_id = @gis_object_id;
END
GO
