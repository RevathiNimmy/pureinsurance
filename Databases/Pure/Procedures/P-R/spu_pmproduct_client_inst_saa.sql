SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_pmproduct_client_inst_saa
GO

CREATE PROCEDURE spu_pmproduct_client_inst_saa
    @language_id INTEGER
AS  
  
/********************************************************************************************************/  
/* sp_pmproduct_client_inst_saa Selects all of the PMproduct Client Install records. */  
/********************************************************************************************************/  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 Original 21/01/1999 RFC */  
/********************************************************************************************************/  
BEGIN  
  
    SELECT  
        i.pmproduct_id,
        p.code,
        c.caption,
        i.required_server_version,
        i.server_software_date,
        i.latest_client_version,
        i.client_software_date,
        i.is_latest_client_mandatory,
        i.is_client_auto_installable,
        i.client_install_path,
        i.client_install_program,
        i.client_install_description,
        i.client_reboot_level
    FROM     
        pmproduct_client_install i
        INNER JOIN pmproduct p
            ON i.pmproduct_id = p.pmproduct_id
        LEFT OUTER JOIN pmcaption c
            ON p.caption_id = c.caption_id
            AND c.language_id = @language_id
    ORDER BY caption ASC

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO