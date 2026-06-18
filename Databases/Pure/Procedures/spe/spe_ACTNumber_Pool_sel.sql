SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Pool_sel'
GO
/*************************************************************************/
/* ERWIN generated select all fields for a given key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 07/07/97 RFC Original (Based on Original by SP) */
/* ECK 18/05/00 company ID parameter */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Pool_sel
    -- @actnumber_pool_id int,
    @actnumber_range_id int,
    @company_id smallint
AS

SELECT
    actnumber_pool_id,
    actnumber_range_id,
    allocated_at,
    user_id
FROM ACTNumber_Pool

WHERE actnumber_range_id = @actnumber_range_id
AND company_id = @company_id

GO

