SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_gis_policy_link_party_upd'
GO


CREATE PROCEDURE spu_gis_policy_link_party_upd
    @gis_policy_link_id INTEGER,
    @party_cnt INTEGER
AS


BEGIN

/********************************************************************************************************/
/* Stored Procedure spu_gis_policy_link_party_upd, Updates the GIS Policy Link party_cnt field. */
/********************************************************************************************************/

/********************************************************************************************************/
/* Revision             Description of Modification                                     Date        Who */
/* --------             ---------------------------                                     ----        --- */
/* 1.0                  Original                            14/06/2000  RAG */
/********************************************************************************************************/

UPDATE  gis_policy_link
SET     party_cnt       = @party_cnt
WHERE   gis_policy_link_id  = @gis_policy_link_id

END
GO


