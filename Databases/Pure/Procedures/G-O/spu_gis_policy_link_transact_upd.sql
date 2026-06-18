SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_gis_policy_link_transact_upd'
GO


CREATE PROCEDURE spu_gis_policy_link_transact_upd
    @gis_policy_link_id INTEGER,
    @transact_date DATETIME,
    @transact_type VARCHAR(20),
    @gis_scheme_id INTEGER = NULL,
    @new_transact_type VARCHAR(20) OUTPUT
AS


BEGIN
/********************************************************************************************************/
/* Stored Procedure spu_gis_policy_link_transact_upd, Updates the GIS Policy Link Transact Fields.       */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date     Who    */
/* --------             ---------------------------                                     ----     ---    */
/* 1.0                  Original       13/06/2000  RAG                                                  */
/* 1.1                  Amended to include Transaction Point numeric values. The char   28/7/00  RFC    */
/* 1.1                  values (NB & MTA) will remain as the transaction complete values.               */
/* 1.2                  Allow updates to NB & MTA for processing of Cancellations.      05/08/00 RFC    */
/* 1.3                  Reverse 1.2 change. i.e. do not allow updates to NB or MTA      14/08/00 RFC    */
/********************************************************************************************************/

DECLARE @current_transact_type VARCHAR(20)

/* Get the Current Transaction Type */
SELECT  @current_transact_type = transact_type
FROM    gis_policy_link
WHERE   gis_policy_link_id = @gis_policy_link_id

IF @current_transact_type IS NULL
    SELECT @current_transact_type = '0'

/* If the Current Transaction Type is NOT Numeric i.e. NB or MTA then */
/* we do not want to do anything else with it, so return current value */
IF (ISNUMERIC(@current_transact_type) <> 1)
    BEGIN

    SELECT @new_transact_type = @current_transact_type
    RETURN

    END

/* If the Transaction Type we are setting is numeric, i.e. NOT NB or MTA */
/* then we need to check to see if we have done this step before */
IF (ISNUMERIC(@transact_type) = 1)

    /* If the Current Transaction Type is greater the the one that we */
    /* are trying to set then we have done this step before, so return current value */
    IF (@current_transact_type > @transact_type)

        BEGIN

        SELECT @new_transact_type = @current_transact_type
        RETURN

        END

/* We have not done this Transaction Stage Before */
/* So Update the policy Link */
IF @gis_scheme_id IS NULL
    UPDATE  gis_policy_link
    SET     transact_date   = @transact_date ,
        transact_type   = @transact_type
    WHERE   gis_policy_link_id = @gis_policy_link_id
ELSE
    UPDATE  gis_policy_link
    SET     transact_date   = @transact_date ,
        transact_type   = @transact_type ,
        gis_scheme_id   = @gis_scheme_id
    WHERE   gis_policy_link_id = @gis_policy_link_id

/* Return the New Transact Type */
SELECT @new_transact_type = @transact_type

END
GO


