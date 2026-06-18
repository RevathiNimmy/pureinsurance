SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_ACTNumber_upd'
GO
/*************************************************************************/
/* ERWIN generated update a record based on the key */
/*************************************************************************/
/*************************************************************************/
/* 1.0 06/08/1997 RFC Original (Based on SP Original) */
/* ECK 18/05/00 company ID parameter */
/*************************************************************************/
CREATE PROCEDURE spe_ACTNumber_upd
    @actnumber_id int,
    @actnumber_range_id int,
    @user_id smallint,
    @company_id smallint
AS
BEGIN

UPDATE ACTNumber
    SET
    allocated_at=GetDate(),
    user_id=@user_id

WHERE actnumber_id = @actnumber_id AND actnumber_range_id = @actnumber_range_id
AND company_id = @company_id
END

GO

