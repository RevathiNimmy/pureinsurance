SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_gis_policy_link_upd_schid'
GO


CREATE PROCEDURE spu_gis_policy_link_upd_schid
    @gis_policy_link_id integer,
    @gis_scheme_id integer
AS


BEGIN

/********************************************************************************************************/
/* Stored Procedure spu_gis_policy_link_upd_schid. Updates gis_scheme_id as used in NBTransact (bGIS)    */
/********************************************************************************************************/

/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                                                        01/02/2000  CL  */
/********************************************************************************************************/

UPDATE gis_policy_link
SET gis_scheme_id = @gis_scheme_id
WHERE gis_policy_link_id = @gis_policy_link_id

END
GO


