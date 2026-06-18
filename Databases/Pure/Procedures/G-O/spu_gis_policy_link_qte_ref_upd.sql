SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_gis_policy_link_qte_ref_upd'
GO


CREATE PROCEDURE spu_gis_policy_link_qte_ref_upd
    @gis_policy_link_id INTEGER,
    @quote_ref CHAR(11),
    @quote_ref_password VARCHAR(30) = NULL
AS


BEGIN

/********************************************************************************************************/
/* Stored Procedure spu_gis_policy_link_qte_ref_upd, Updates the GIS Policy Link Quote Reference Fields. */
/********************************************************************************************************/

/********************************************************************************************************/
/* Revision     Description of Modification         Date        Who */
/* --------     ---------------------------         ----        --- */
/* 1.0          Original                            31/08/1999  RFC */
/* 1.1          Change quote_ref length to be 11 to 
                cater for new format. GW046 
                                                    25/01/2000  CJB */
/* 1.2          Made Quote Password optional so that 
                just Quote Reference update could occur
                on Renewal Copy Dataset.            
                                                    20/03/2002  DD  */
/********************************************************************************************************/

UPDATE
    gis_policy_link
SET
    quote_ref = @quote_ref ,
    quote_ref_password = isnull(@quote_ref_password, quote_ref_password) 
WHERE
    gis_policy_link_id = @gis_policy_link_id
END
GO


