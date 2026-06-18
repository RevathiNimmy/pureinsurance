SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_document_spooler_upd_mo'
GO


CREATE PROCEDURE spu_document_spooler_upd_mo
    @document_spooler_id int,
    @modified_by_id int,
    @date_modified datetime
AS

    DECLARE @UwOrAgency char(1)

    -- Check what product is installed
    EXEC spu_GetUwOrAgency @UwOrAgency output

    IF @UwOrAgency = 'U' BEGIN
        -- If we are S4I reset the printed and archive counts as the modified version has not yet
        -- been printed or archived. This is required for auto-archive as it will only archive
        -- once per edited version and it makes more sense than the original behaviour.
        UPDATE  document_spooler
        SET     modified_by_id = @modified_by_id,
                date_modified = @date_modified,
                times_printed = 0,
                times_archived = 0
        WHERE   document_spooler_id = @document_spooler_id
    END ELSE BEGIN
        UPDATE  document_spooler
        SET     modified_by_id = @modified_by_id,
                date_modified = @date_modified
        WHERE   document_spooler_id = @document_spooler_id
    END


GO


