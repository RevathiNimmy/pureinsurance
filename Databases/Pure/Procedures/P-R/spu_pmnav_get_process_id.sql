SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pmnav_get_process_id'
GO


CREATE PROCEDURE spu_pmnav_get_process_id
    @product_code char(10),
    @process_code char(10),
    @effective_date datetime,
    @process_id int OUTPUT
AS

/*******************************************************************************************************/
/* sp_pmnav_get_process_id selects the Process ID of the effective record,                             */
/* the product_code, process_code and the effective date supplied.                                     */
/*******************************************************************************************************/
/*******************************************************************************************************/
/* Revision Description of Modification Date        Who                                                */
/* -------- --------------------------- ----        ---                                                */
/* 1.0      Original                    18/09/1998  RFC                                                */
/*******************************************************************************************************/
BEGIN
    SELECT @process_id = pmnav_process_id
    FROM pmnav_process process,
        pmproduct product
    WHERE RTRIM(process.code) = RTRIM(@process_code)
    AND RTRIM(product.code) = RTRIM(@product_code)
    AND process.pmproduct_id = product.pmproduct_id
    AND process.effective_date <= @effective_date
    AND process.is_deleted = 0
END
GO


