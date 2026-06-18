SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_Get_Source_From_Code'
GO


CREATE PROCEDURE spu_SAM_Get_Source_From_Code
    @source_code char(10)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date       Who */
/* -------- --------------------------- ----       --- */
/* 1.0      New procedure               08-02-2006 PW  */
/********************************************************************************************************/
   SELECT source_id,
          base_currency_id
     FROM Source
    WHERE code = @source_code
      AND is_deleted = 0
GO


