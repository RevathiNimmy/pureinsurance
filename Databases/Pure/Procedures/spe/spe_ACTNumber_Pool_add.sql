SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_Pool_add'
GO
/*************************************************************************/
/* ERWIN generated add record and generate ID column if required. */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/* ECK 18/05/00 company ID parameter */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_Pool_add
    @actnumber_pool_id int,
    @actnumber_range_id int,
    @user_id int,
    @company_id smallint
AS

BEGIN

INSERT INTO ACTNumber_Pool (
    actnumber_pool_id ,
    actnumber_range_id ,
    user_id,
    company_id )
VALUES (
    @actnumber_pool_id,
    @actnumber_range_id,
    @user_id,
    @company_id)
END

GO

