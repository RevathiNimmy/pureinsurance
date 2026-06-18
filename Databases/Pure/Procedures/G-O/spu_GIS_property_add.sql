SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GIS_property_add'
GO

CREATE PROCEDURE spu_GIS_property_add
(
    @gis_property_id              INT OUTPUT,
    @gis_object_id                INT,
    @property_name                VARCHAR(70),
    @column_name                  VARCHAR(70),
    @data_type                    TINYINT,
    @is_input_property            TINYINT,
    @is_identifying_property      TINYINT,
    @is_primary_key               TINYINT,
    @polaris_property_id          INT,
    @is_deleted                   TINYINT,
    @is_search_property           TINYINT,
    @index_linking_id             INT,
    @Edit_Flags                   TINYINT,
    @Specials_Type                INT,
    @Specials_Type_Reference      VARCHAR(50),
    @is_in_mis_export             TINYINT,
    @is_formatted_text            TINYINT,
    @is_chase_cycle_property      TINYINT,
    @is_claim360display           TINYINT = 0,
    @UserId                       INT = NULL,
    @UniqueId                     VARCHAR(50) = NULL,
    @ScreenHierarchy              VARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    /* Generate GIS_property_id (legacy MAX+1 pattern) */
    IF @gis_property_id = 0
        SET @gis_property_id = NULL;

    IF @gis_property_id IS NULL
        SELECT @gis_property_id = MAX(gis_property_id) + 1
        FROM GIS_property;

    IF @gis_property_id IS NULL
        SET @gis_property_id = 1;

    /* Insert into GIS_property */
    INSERT INTO GIS_property
    (
        gis_property_id,
        gis_object_id,
        property_name,
        column_name,
        data_type,
        is_input_property,
        is_identifying_property,
        is_primary_key,
        polaris_property_id,
        is_deleted,
        is_search_property,
        index_linking_id,
        Edit_Flags,
        Specials_Type,
        Specials_Type_Reference,
        is_in_mis_export,
        is_formatted_text,
        is_chase_cycle_property,
        is_claim360display
    )
    VALUES
    (
        @gis_property_id,
        @gis_object_id,
        @property_name,
        @column_name,
        @data_type,
        @is_input_property,
        @is_identifying_property,
        @is_primary_key,
        @polaris_property_id,
        @is_deleted,
        @is_search_property,
        @index_linking_id,
        @Edit_Flags,
        @Specials_Type,
        @Specials_Type_Reference,
        @is_in_mis_export,
        @is_formatted_text,
        @is_chase_cycle_property,
        @is_claim360display
    )

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

		;WITH RecursiveParents AS (
			SELECT gis_object_id, parent_object_id, object_name
			FROM GIS_Object
			WHERE gis_object_id = @gis_object_id

			UNION ALL

			SELECT gob.gis_object_id, gob.parent_object_id, gob.object_name
			FROM GIS_Object gob
			INNER JOIN RecursiveParents rp ON gob.gis_object_id = rp.parent_object_id
		)

		-- Step 3: Check if object_name is found in @ScreenHierarchy
		SELECT 1 AS match_found
		INTO #MatchFound
		FROM RecursiveParents
		WHERE @ScreenHierarchy LIKE '%Property(%' + object_name + '_id)%';

		-- Check if any match found
		IF EXISTS (SELECT 1 FROM #MatchFound)
		BEGIN
			DROP TABLE #MatchFound;
			RETURN;
		END

        IF NOT EXISTS
        (
            SELECT 1
            FROM configuration_audit_details cad
            WHERE cad.configuration_audit_master_id = @Configuration_Audit_Master_Id
              AND cad.Type            = 'I'
              AND cad.TableName       = 'GIS_property'
              AND cad.key_field_name  = 'gis_property_id'
              AND cad.key_field_value = @gis_property_id
              AND (
                    cad.key_field_desc = @ScreenHierarchy
                    OR (cad.key_field_desc IS NULL AND @ScreenHierarchy IS NULL)
                  )
              AND cad.FieldName        = 'New GIS Property added'
              AND cad.FieldDisplayName = 'New GIS Property added'
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
                'GIS_property',
                'gis_property_id',
                @gis_property_id,
                @ScreenHierarchy,
                'New GIS Property added',
                'New GIS Property added',
                NULL,
                NULL
            );
        END
    END;

    SELECT GIS_property_id = @gis_property_id;
END
GO
