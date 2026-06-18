SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_pmproduct_lookup_del'
GO


CREATE PROCEDURE spu_pmproduct_lookup_del
    @pmproduct_id SMALLINT,
    @table_name VARCHAR(30)
AS

/********************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 10/11/1999 DAK */
/********************************************************************************************************/
DELETE pmproduct_lookup
    WHERE pmproduct_id = @pmproduct_id
    AND lookup_table_name = @table_name
GO


