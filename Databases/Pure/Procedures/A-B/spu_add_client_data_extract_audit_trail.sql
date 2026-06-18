SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_add_client_data_extract_audit_trail'
GO

-- ============================================================
-- PBI 39544: System Events on Extracting Client Data
-- Stored Procedure: spu_add_client_data_extract_audit_trail
--
-- Purpose:
--   Writes one row to configuration_audit_master and one row to
--   configuration_audit_details so that the extraction event
--   appears in the System Events view page (secure/SystemEvents.aspx).
--
-- Parameters:
--   @UserId     INT           - PMUser.user_id of the extracting user
--   @ClientCode VARCHAR(100)  - Party shortname / client code
--   @ModuleId   INT           - Audit_Trail_Modules.Modules_id for
--                               'Extract Client Data' (seeded by Task 1)
-- ============================================================
CREATE PROCEDURE spu_add_client_data_extract_audit_trail
    @UserId     INT,
    @ClientCode VARCHAR(100),
    @ModuleId   INT
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @MasterId   INT
    DECLARE @UniqueId   VARCHAR(50)

    SET @UniqueId = CAST(NEWID() AS VARCHAR(50))

    BEGIN TRY

        BEGIN TRANSACTION

        -- Insert master record (one per extraction event)
        INSERT INTO configuration_audit_master
        (
            UniqueId,
            Module_Id,
            ModuleName,
            UpdateDate,
            UserId
        )
        VALUES
        (
            @UniqueId,
            @ModuleId,
            'Extract Client Data',
            GETDATE(),
            @UserId
        )

        SELECT @MasterId = SCOPE_IDENTITY()

        -- Insert detail record
        -- key_field_desc  => maps to 'Level' column (ScreenDescription) in System Events grid
        -- FieldDisplayName => maps to 'Property' column (FieldDescription) in System Events grid
        -- OldValue / NewValue are blank — no value changed, this is a read-only audit event
        -- diff_count will be 1 (single unique row) so spu_get_audit_trail_details CTE will return it
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
            @MasterId,
            'U',
            'Party',
            'party_cnt',
            '',
            'Extract Client Data / ' + ISNULL(@ClientCode, ''),
            '',
            '',
            '',
            ''
        )

        COMMIT TRANSACTION

    END TRY
    BEGIN CATCH
        -- Audit logging failure must never interrupt the extract flow; swallow silently
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION
    END CATCH

    SET NOCOUNT OFF
END
GO
