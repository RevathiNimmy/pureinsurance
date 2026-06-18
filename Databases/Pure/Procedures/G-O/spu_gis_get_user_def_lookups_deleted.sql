SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_get_user_def_lookups_deleted'
GO
/*******************************************************************************************************/
/* spu_gis_get_user_def_lookups_deleted selects the deleted entries from the gis_user_def_detail       */
/* table based on the Header ID and the is_deleted flag.                                               */
/* RFC13/11/2002                                                                                       */
/*******************************************************************************************************/
CREATE PROCEDURE spu_gis_get_user_def_lookups_deleted
    @gis_user_def_header_id int,
    @language_id  integer

AS
BEGIN
   
    SELECT GD.GIS_user_def_detail_id, 
           cap.caption, 
           RTRIM(GD.code) 'code'
    FROM GIS_user_def_detail GD
    inner join pmcaption cap
       on GD.caption_id = cap.caption_id
    WHERE GD.GIS_user_def_header_id = @gis_user_def_header_id
      AND GD.is_deleted = 1
      AND cap.language_id = @language_id

END
GO

