SET QUOTED_IDENTIFIER ON
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_GIS_property_upd'
GO

CREATE PROCEDURE spe_GIS_property_upd
(
    @gis_property_id           INT,
    @gis_object_id             INT,
    @property_name             VARCHAR(70),
    @column_name               VARCHAR(70),
    @data_type                 TINYINT,
    @is_input_property         TINYINT,
    @is_identifying_property   TINYINT,
    @is_primary_key            TINYINT,
    @polaris_property_id       INT,
    @is_deleted                TINYINT,
    @is_search_property        TINYINT,
    @index_linking_id          INT,
    @Edit_Flags                TINYINT,
    @Specials_Type             INT,
    @Specials_Type_Reference   VARCHAR(50),
    @is_in_mis_export          TINYINT,
    @is_formatted_text         TINYINT,
    @is_chase_cycle_property   TINYINT,
    @is_claim360display        TINYINT,
    @UserId                    INT = NULL,
    @UniqueId                  VARCHAR(50) = NULL,
    @ScreenHierarchy           VARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    IF @UniqueId IS NOT NULL
    BEGIN
		DECLARE
			@old_gis_object_id            INT,
			@old_property_name            VARCHAR(70),
			@old_column_name              VARCHAR(70),
			@old_data_type                TINYINT,
			@old_is_input_property        TINYINT,
			@old_is_identifying_property  TINYINT,
			@old_is_primary_key           TINYINT,
			@old_polaris_property_id      INT,
			@old_is_deleted               TINYINT,
			@old_is_search_property       TINYINT,
			@old_index_linking_id         INT,
			@old_Edit_Flags               TINYINT,
			@old_Specials_Type            INT,
			@old_Specials_Type_Reference  VARCHAR(50),
			@old_is_in_mis_export         TINYINT,
			@old_is_formatted_text        TINYINT,
			@old_is_chase_cycle_property  TINYINT,
			@old_is_claim360display       TINYINT;

		SELECT
			@old_gis_object_id           = gp.gis_object_id,
			@old_property_name           = gp.property_name,
			@old_column_name             = gp.column_name,
			@old_data_type               = gp.data_type,
			@old_is_input_property       = gp.is_input_property,
			@old_is_identifying_property = gp.is_identifying_property,
			@old_is_primary_key          = gp.is_primary_key,
			@old_polaris_property_id     = gp.polaris_property_id,
			@old_is_deleted              = gp.is_deleted,
			@old_is_search_property      = gp.is_search_property,
			@old_index_linking_id        = gp.index_linking_id,
			@old_Edit_Flags              = gp.Edit_Flags,
			@old_Specials_Type           = gp.Specials_Type,
			@old_Specials_Type_Reference = gp.Specials_Type_Reference,
			@old_is_in_mis_export        = gp.is_in_mis_export,
			@old_is_formatted_text       = gp.is_formatted_text,
			@old_is_chase_cycle_property = gp.is_chase_cycle_property,
			@old_is_claim360display      = gp.is_claim360display
		FROM GIS_property gp
		WHERE gp.gis_property_id = @gis_property_id
		  AND gp.gis_object_id   = @gis_object_id;

		IF NOT EXISTS (SELECT 1 FROM GIS_property WHERE gis_property_id = @gis_property_id AND gis_object_id = @gis_object_id)
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

        /* --- Insert audit rows only for changed fields --- */
        IF ISNULL(@old_gis_object_id, -1) <> ISNULL(@gis_object_id, -1)
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'gis_object_id', 'GIS Object',
             CONVERT(VARCHAR(50), @old_gis_object_id), CONVERT(VARCHAR(50), @gis_object_id));

        IF ISNULL(@old_property_name, '') <> ISNULL(@property_name, '')
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'property_name', 'Property Name', @old_property_name, @property_name);

        IF ISNULL(@old_column_name, '') <> ISNULL(@column_name, '')
            INSERT INTO configuration_audit_details
            (configuration_audit_master_id, Type, TableName, key_field_name, key_field_value, key_field_desc,
             FieldName, FieldDisplayName, OldValue, NewValue)
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'column_name', 'Column Name', @old_column_name, @column_name);


        IF ISNULL(CAST(@old_data_type AS INT), -1) <> ISNULL(CAST(@data_type AS INT), -1) AND ISNULL(CAST(@old_data_type AS INT), 0) <> ISNULL(CAST(@data_type AS INT), 0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'data_type', 'Data Type',
             CONVERT(VARCHAR(50), @old_data_type), CONVERT(VARCHAR(50), @data_type));

        IF ISNULL(CAST(@old_is_input_property AS INT), -1) <> ISNULL(CAST(@is_input_property AS INT), -1)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_input_property', 'Input Property',
             CONVERT(VARCHAR(50), @old_is_input_property), CONVERT(VARCHAR(50), @is_input_property));

        IF ISNULL(CAST(@old_is_identifying_property AS INT), -1) <> ISNULL(CAST(@is_identifying_property AS INT), -1) AND ISNULL(CAST(@old_is_identifying_property AS INT), 0) <> ISNULL(CAST(@is_identifying_property AS INT), 0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_identifying_property', 'Identifying Property',
             CONVERT(VARCHAR(50), @old_is_identifying_property), CONVERT(VARCHAR(50), @is_identifying_property));

        IF ISNULL(CAST(@old_is_primary_key AS INT), -1) <> ISNULL(CAST(@is_primary_key AS INT), -1) AND ISNULL(CAST(@old_is_primary_key AS INT), 0) <> ISNULL(CAST(@is_primary_key AS INT), 0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_primary_key', 'Primary Key',
             CONVERT(VARCHAR(50), @old_is_primary_key), CONVERT(VARCHAR(50), @is_primary_key));

        IF ISNULL(@old_polaris_property_id, -1) <> ISNULL(@polaris_property_id, -1) AND ISNULL(@old_polaris_property_id,0) <> ISNULL(@polaris_property_id,0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'polaris_property_id', 'Polaris Property',
             CONVERT(VARCHAR(50), @old_polaris_property_id), CONVERT(VARCHAR(50), @polaris_property_id));

        IF ISNULL(CAST(@old_is_deleted AS INT), -1) <> ISNULL(CAST(@is_deleted AS INT), -1) AND ISNULL(CAST(@old_is_deleted AS INT),0) <> ISNULL(CAST(@is_deleted AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_deleted', 'Deleted',
             CONVERT(VARCHAR(50), @old_is_deleted), CONVERT(VARCHAR(50), @is_deleted));

        IF ISNULL(CAST(@old_is_search_property AS INT), -1) <> ISNULL(CAST(@is_search_property AS INT), -1) AND ISNULL(CAST(@old_is_search_property AS INT),0) <> ISNULL(CAST(@is_search_property AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_search_property', 'Search Property',
             CONVERT(VARCHAR(50), @old_is_search_property), CONVERT(VARCHAR(50), @is_search_property));

        IF ISNULL(@old_index_linking_id, -1) <> ISNULL(@index_linking_id, -1) AND ISNULL(@old_index_linking_id,0) <> ISNULL(@index_linking_id,0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'index_linking_id', 'Index Linking',
             CONVERT(VARCHAR(50), @old_index_linking_id), CONVERT(VARCHAR(50), @index_linking_id));

        IF ISNULL(CAST(@old_Edit_Flags AS INT), -1) <> ISNULL(CAST(@Edit_Flags AS INT), -1) AND ISNULL(CAST(@old_Edit_Flags AS INT),0) <> ISNULL(CAST(@Edit_Flags AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'Edit_Flags', 'Edit Flags',
             CONVERT(VARCHAR(50), @old_Edit_Flags), CONVERT(VARCHAR(50), @Edit_Flags));

        IF ISNULL(@old_Specials_Type, -1) <> ISNULL(@Specials_Type, -1) AND ISNULL(@old_Specials_Type,0) <> ISNULL(@Specials_Type,0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'Specials_Type', 'Specials Type',
             CONVERT(VARCHAR(50), @old_Specials_Type), CONVERT(VARCHAR(50), @Specials_Type));

        IF ISNULL(@old_Specials_Type_Reference, '') <> ISNULL(@Specials_Type_Reference, '')
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'Specials_Type_Reference', 'Specials Type Reference',
             @old_Specials_Type_Reference, @Specials_Type_Reference);

        IF ISNULL(CAST(@old_is_in_mis_export AS INT), -1) <> ISNULL(CAST(@is_in_mis_export AS INT), -1) AND ISNULL(CAST(@old_is_in_mis_export AS INT),0) <> ISNULL(CAST(@is_in_mis_export AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_in_mis_export', 'MIS Export',
             CONVERT(VARCHAR(50), @old_is_in_mis_export), CONVERT(VARCHAR(50), @is_in_mis_export));

        IF ISNULL(CAST(@old_is_formatted_text AS INT), -1) <> ISNULL(CAST(@is_formatted_text AS INT), -1) AND ISNULL(CAST(@old_is_formatted_text AS INT),0) <> ISNULL(CAST(@is_formatted_text AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_formatted_text', 'Formatted Text',
             CONVERT(VARCHAR(50), @old_is_formatted_text), CONVERT(VARCHAR(50), @is_formatted_text));

        IF ISNULL(CAST(@old_is_chase_cycle_property AS INT), -1) <> ISNULL(CAST(@is_chase_cycle_property AS INT), -1) AND ISNULL(CAST(@old_is_chase_cycle_property AS INT),0) <> ISNULL(CAST(@is_chase_cycle_property AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_chase_cycle_property', 'Chase Cycle Property',
             CONVERT(VARCHAR(50), @old_is_chase_cycle_property), CONVERT(VARCHAR(50), @is_chase_cycle_property));

        IF ISNULL(CAST(@old_is_claim360display AS INT), -1) <> ISNULL(CAST(@is_claim360display AS INT), -1) AND ISNULL(CAST(@old_is_claim360display AS INT),0) <> ISNULL(CAST(@is_claim360display AS INT),0)
            INSERT INTO configuration_audit_details
            VALUES
            (@Configuration_Audit_Master_Id, 'U', 'GIS_property', 'gis_property_id', @gis_property_id, @ScreenHierarchy,
             'is_claim360display', 'Claim360 Display',
             CONVERT(VARCHAR(50), @old_is_claim360display), CONVERT(VARCHAR(50), @is_claim360display));
    END;

    UPDATE GIS_property
       SET gis_object_id           = @gis_object_id,
           property_name           = @property_name,
           column_name             = @column_name,
           data_type               = @data_type,
           is_input_property       = @is_input_property,
           is_identifying_property = @is_identifying_property,
           is_primary_key          = @is_primary_key,
           polaris_property_id     = @polaris_property_id,
           is_deleted              = @is_deleted,
           is_search_property      = @is_search_property,
           index_linking_id        = @index_linking_id,
           Edit_Flags              = @Edit_Flags,
           Specials_Type           = @Specials_Type,
           Specials_Type_Reference = @Specials_Type_Reference,
           is_in_mis_export        = @is_in_mis_export,
           is_formatted_text       = @is_formatted_text,
           is_chase_cycle_property = @is_chase_cycle_property,
           is_claim360display      = @is_claim360display
     WHERE gis_property_id = @gis_property_id
       AND gis_object_id   = @gis_object_id;
END
GO
