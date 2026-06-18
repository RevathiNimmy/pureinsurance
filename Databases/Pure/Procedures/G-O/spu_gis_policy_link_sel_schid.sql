SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_gis_policy_link_sel_schid'
GO


CREATE PROCEDURE spu_gis_policy_link_sel_schid
    @gis_policy_link_id integer
AS


BEGIN

/********************************************************************************************************/
/* Stored Procedure spu_gis_policy_link_sel_schid. Get gis_scheme_id as stored in NBTransact (bGIS)      */
/********************************************************************************************************/
/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                                                        01/02/2000  CL  */
/********************************************************************************************************/

SELECT gis_scheme_id FROM gis_policy_link
WHERE gis_policy_link_id = @gis_policy_link_id

END
GO


