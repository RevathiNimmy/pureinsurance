SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_Batch'
GO


CREATE PROCEDURE spu_ACT_Select_Batch
    @batch_id int
AS


SELECT
    batch_id,
    company_id,
    batchstatus_id,
    user_id,
    batch_ref,
    created_date,
    authorised_date,
    accounting_date,
    comment,
-- *** pkh 07/10/2002 starts - Added for Front Office Receipting module
    batch_type_id ,
    batch_source_id,
    xml_object
-- *** pkh 07/10/2002 ends   - Added for Front Office Receipting module
FROM Batch
WHERE batch_id = @batch_id
GO


