SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_User_Def_Caption'
GO


CREATE PROCEDURE spu_User_Def_Caption
    @Tableid int
AS


SELECT  cap.caption FROM gis_user_def_header tn, pmcaption cap
WHERE tn.GIS_user_def_header_id=@tableid AND tn.caption_id = cap.caption_id and cap.language_id = 1
GO


