SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMProduct_Client_Insta_sel'
GO
CREATE PROCEDURE spe_PMProduct_Client_Insta_sel
    @pmproduct_id smallint
AS
SELECT
    pmproduct_id,
    required_server_version,
    server_software_date,
    latest_client_version,
    client_software_date,
    is_latest_client_mandatory,
    is_client_auto_installable,
    client_install_path,
    client_install_program,
    client_install_description,
    client_reboot_level
    FROM PMProduct_Client_Install
    WHERE pmproduct_id = @pmproduct_id

GO

