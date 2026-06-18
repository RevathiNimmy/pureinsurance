SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_pm_get_eff_id_from_id'
GO


CREATE PROCEDURE spu_pm_get_eff_id_from_id
    @tablename varchar(255),
    @effective_date datetime,
    @id int OUTPUT
AS

/*******************************************************************************************************/
/* sp_pm_get_eff_id_from_id selects the ID of the effective record, based on */
/* the ID, tablename and effective date supplied. */
/* Return values 0 OK */
/* -100 Error */
/*******************************************************************************************************/
/*******************************************************************************************************/
/* Revision Description of Modification Date Who */
/* -------- --------------------------- ---- --- */
/* 1.0 Original 10/11/1997 RFC */
/*******************************************************************************************************/
BEGIN
    DECLARE @code char(10) /* Code for supplied id */
    DECLARE @SQL varchar(255) /* SQL code for EXECUTE */
    DECLARE @return_status integer /* Return Status from sp call */

    /* If id is null then output the null */
    IF (@id = NULL)
    BEGIN
        SELECT @id = NULL
        RETURN -100
    END

    /***************************************************************/
    /* Get the code for the supplied id from @tablename */
    /***************************************************************/

    /* Call sp to get the code for the supplied id */
    EXECUTE @return_status=
    spu_pm_get_code_from_id
        @tablename=@tablename,
        @id=@id,
        @code=@code OUTPUT

    IF (@return_status <> 0)
        RETURN @return_status

    /***************************************************************/
    /* Get the ID of the effective record with the same code */
    /***************************************************************/

    /* Call sp to get effective record */
    EXECUTE @return_status=
    spu_pm_get_eff_id_from_code
        @tablename=@tablename,
        @code=@code,
    @effective_date=@effective_date,
        @id=@id OUTPUT

    RETURN @return_status

END
GO


