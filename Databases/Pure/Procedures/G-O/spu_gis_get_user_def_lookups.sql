SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_get_user_def_lookups'
GO
/*******************************************************************************************************/
/* spu_gis_get_user_def_lookups the entries from the GIS_User_Def_header & data table,                 */
/* based on the table name and effective_date.                                                         */
/* RFC13/11/2002                                                                                       */
/* CTAF 20021206 - Added header_code optional parameter. Made the Id optional                          */
/*******************************************************************************************************/
CREATE PROCEDURE spu_gis_get_user_def_lookups
    @gis_user_def_header_id int = NULL,
    @gis_user_def_header_code char(10) = NULL,
    @effective_date datetime,
    @language_id  integer

AS
BEGIN

    IF (@gis_user_def_header_id IS NULL)
    BEGIN

        SELECT GD.GIS_user_def_detail_id, 
               cap.caption, 
               RTRIM(GD.code) 'code',
               gd.parent
        FROM GIS_user_def_detail GD
        INNER JOIN pmcaption cap
           ON GD.caption_id = cap.caption_id
        INNER JOIN gis_user_def_header GH
           ON GH.gis_user_def_header_id = GD.gis_user_def_header_id
        WHERE GH.code = @gis_user_def_header_code 
          AND GH.effective_date <= @effective_date
          AND GD.is_deleted = 0
          AND cap.language_id = @language_id

    END
    ELSE
    BEGIN

        SELECT GD.GIS_user_def_detail_id, 
               cap.caption, 
               RTRIM(GD.code) 'code',
               gd.parent
        FROM GIS_user_def_detail GD
        INNER JOIN pmcaption cap
           ON GD.caption_id = cap.caption_id
        WHERE GD.GIS_user_def_header_id = @gis_user_def_header_id
          AND GD.is_deleted = 0
          AND GD.effective_date <= @effective_date
          AND cap.language_id = @language_id

    END

	/* This is the Old Embedded SQL from bGISUserDefLookup
	"SELECT GD.GIS_user_def_detail_id, cap.caption, GD.code " & _
	"FROM GIS_user_def_detail GD, pmcaption cap " & _
	"WHERE GD.is_deleted = 0 " & _
	"AND GD.GIS_user_def_header_id = {table} " & _
	"AND GD.effective_date <= {Effective_Date} " & _
	"AND GD.caption_id = cap.caption_id " & _
	"AND cap.language_id = {Language_ID}"
    	*/

END
GO

