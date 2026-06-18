SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_sel'
GO


CREATE PROCEDURE spu_pmproduct_sel
    @code VARCHAR(12)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original version 29/11/1999 DAK */
/********************************************************************************************************/
SELECT pmproduct_id pmproduct_id
FROM PMProduct
WHERE code = @code
GO


