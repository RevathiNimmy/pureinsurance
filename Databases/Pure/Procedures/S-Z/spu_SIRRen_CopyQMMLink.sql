SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_CopyQMMLink'
GO


CREATE PROCEDURE spu_SIRRen_CopyQMMLink
    @old_policy_link_id int,
    @new_policy_link_id int
AS


BEGIN
    /*
            CTAF 180601 - Copies a QMM link record after the risk has been copied
    */
    IF NOT EXISTS (SELECT * FROM GII_QMM_Link WHERE gis_policy_link_id = @new_policy_link_id)
    BEGIN
        INSERT INTO GII_QMM_Link
        ( gis_policy_link_id, vbs_key, ref, client_key, pol_system_num, pol_version_num, cli_system_num, cli_version_num )
        SELECT @new_policy_link_id, vbs_key, ref, client_key, pol_system_num, pol_version_num, cli_system_num, cli_version_num
        FROM GII_QMM_Link
        WHERE gis_policy_link_id = @old_policy_link_id
    END
END
GO


