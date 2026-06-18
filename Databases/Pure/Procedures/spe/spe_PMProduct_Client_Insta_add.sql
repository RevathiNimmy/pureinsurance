SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_PMProduct_Client_Insta_add'
GO
CREATE PROCEDURE spe_PMProduct_Client_Insta_add
    @pmproduct_id smallint,
    @required_server_version varchar(40),
    @server_software_date datetime,
    @latest_client_version varchar(40),
    @client_software_date datetime,
    @is_latest_client_mandatory tinyint,
    @is_client_auto_installable tinyint,
    @client_install_path varchar(255),
    @client_install_program varchar(255),
    @client_install_description varchar(255),
    @client_reboot_level tinyint
AS
BEGIN
INSERT INTO PMProduct_Client_Install (
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
    client_reboot_level )
VALUES (
    @pmproduct_id,
    @required_server_version,
    @server_software_date,
    @latest_client_version,
    @client_software_date,
    @is_latest_client_mandatory,
    @is_client_auto_installable,
    @client_install_path,
    @client_install_program,
    @client_install_description,
    @client_reboot_level)
END
GO

