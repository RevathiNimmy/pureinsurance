SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_client_inst_chk'
GO


CREATE PROCEDURE spu_pmproduct_client_inst_chk  
    @pmproduct_code char(10),  
    @pmproduct_id integer OUTPUT,  
    @is_existing_install tinyint OUTPUT,
    @description varchar(255) OUTPUT
AS  
  
/********************************************************************************************************/  
/* sp_pmproduct_client_inst_chk Checks the Client Install Details for the Product Code */  
/********************************************************************************************************/  
/********************************************************************************************************/  
/* Revision Description of Modification Date Who */  
/* -------- --------------------------- ---- --- */  
/* 1.0 Original 02/02/1999 RFC */  
/********************************************************************************************************/  
BEGIN  
    /* Get the Product ID for the Code */  
    SELECT @pmproduct_id = pmproduct_id,
			@description = [description]
    FROM pmproduct  
    WHERE code = @pmproduct_code  
  
    /* If it is not found, exit */  
    IF (@pmproduct_id IS NULL)  
        RETURN  
  
    /* Does a Client Install Already Exist for this Product */  
    IF @pmproduct_id IN (SELECT pmproduct_id FROM pmproduct_client_install)  
        SELECT @is_existing_install = 1  
    ELSE  
        SELECT @is_existing_install = 0  
END
GO


