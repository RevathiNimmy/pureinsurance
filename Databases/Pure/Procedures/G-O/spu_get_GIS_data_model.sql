SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_GIS_data_model'
GO


CREATE PROCEDURE spu_get_GIS_data_model
    @risk_cnt int
AS


/****************************************************************************************************/
/* spu_get_GIS_data_model gets the GIS data model for a given risk.               */
/*                                                                                                      */
/* 1 parameter is passed in - @risk_cnt                     */
/*                                                                                                      */
/* This stored procedure was originally written for use in document production to   */
/* enable the associated standard wordings to be retrieved.                     */
/*                                      */
/* A failure in this procedure will be passed back to the calling procedure.        */
/****************************************************************************************************/
/* Revision Description of Modification     Date        Who */
/* --------         ---------------------------         ----        ---     */
/* 1.0      Original                    23/04/2001  RWH */
/*                                      */
/****************************************************************************************************/

SELECT  gdm.code

FROM        GIS_data_model      gdm,
        Risk            r,
        GIS_screen      gs

WHERE   r.risk_cnt = @risk_cnt
AND     r.gis_screen_id = gs.gis_screen_id
AND     gs.gis_data_model_id = gdm.gis_data_model_id
GO


