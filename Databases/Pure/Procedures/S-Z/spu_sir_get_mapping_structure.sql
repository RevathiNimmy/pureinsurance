SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_sir_get_mapping_structure'
GO


CREATE PROCEDURE spu_sir_get_mapping_structure
    @model_code char(10),
    @target_field_name varchar(255)
AS


BEGIN
/******************************************************************************************/
/* spu_sir_get_mapping_structure     builds a record defined in mapping tables     */
/*                                            */
/* two parameters are passed in                               */
/******************************************************************************************/
/* Revision Description of Modification         Date        Who       */
/* --------     ---------------------------         ----        ---       */
/* 1.0      Original                    29/04/1998  TF    */
/******************************************************************************************/
    DECLARE @model_id   int,
        @detail_id  int
    /* Get model & detail id */
    SELECT  @model_id = M.export_map_model_id,
        @detail_id = D.export_map_detail_id
    FROM    Export_Map_Model    M,
        Export_Map_Detail   D
    WHERE   M.code = @model_code
    AND D.export_map_model_id = M.export_map_model_id
    AND D.target_field_name = @target_field_name
    /* Get format details */
    SELECT  *
    FROM    Export_Map_Format   F
    WHERE   F.export_map_model_id = @model_id
    AND F.export_map_detail_id = @detail_id
    ORDER BY F.sequence

END
GO


