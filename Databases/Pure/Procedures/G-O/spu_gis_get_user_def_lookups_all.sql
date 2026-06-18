SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_gis_get_user_def_lookups_all'
GO
/*******************************************************************************************************/
/* spu_gis_get_user_def_lookups_all                                                                    */
/* RFC05/02/2004                                                                                       */
/* Selects ALL of the entries in a Gis User Def table, whether deleted or effective.                   */
/*******************************************************************************************************/

CREATE PROCEDURE spu_gis_get_user_def_lookups_all 
    @gis_user_def_header_id int = NULL,  
    @gis_user_def_header_code char(10) = NULL,  
    @language_id  integer,  
    @parent_value integer= 0,
	@ExcludeDeletedRecords bit = 1 
AS  
BEGIN  
    IF (@gis_user_def_header_id IS NULL)  
    BEGIN  
        SELECT GD.GIS_user_def_detail_id,  
               cap.caption,  
               RTRIM(GD.code) 'code',  
        GD.effective_date,  
               GD.is_deleted,  
        GD.Parent as parent_id  
        FROM GIS_user_def_detail GD WITH(NOLOCK)  
        INNER JOIN pmcaption cap  
           ON GD.caption_id = cap.caption_id  
        INNER JOIN gis_user_def_header GH WITH(NOLOCK)  
           ON GH.gis_user_def_header_id = GD.gis_user_def_header_id  
        WHERE GH.code = @gis_user_def_header_code  
          AND cap.language_id = @language_id  
          AND (@parent_value = 0 or GD.parent = @parent_value)
		  AND ((@ExcludeDeletedRecords = 1 AND GD.is_deleted =0)
					OR (@ExcludeDeletedRecords <> 1 ))
    END  
    ELSE  
    BEGIN  
        SELECT GD.GIS_user_def_detail_id,  
               cap.caption,  
               RTRIM(GD.code) 'code',  
        GD.effective_date,  
               GD.is_deleted,  
           GD.Parent as parent_id  
        FROM GIS_user_def_detail GD WITH(NOLOCK)  
        INNER JOIN pmcaption cap WITH(NOLOCK)  
           ON GD.caption_id = cap.caption_id  
        WHERE GD.GIS_user_def_header_id = @gis_user_def_header_id  
          AND cap.language_id = @language_id  
          AND (@parent_value = 0 or GD.parent = @parent_value)
		  AND ((@ExcludeDeletedRecords = 1 AND GD.is_deleted =0)
				OR (@ExcludeDeletedRecords <> 1 ))
    END  
END  

GO
