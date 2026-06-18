ddldropprocedure 'spu_APIE_get_UML_Details_Script'
GO
CREATE PROCEDURE spu_APIE_get_UML_Details_Script
@nFromGisUserDefId INT,
@nToGisUserDefId INT
AS
BEGIN

CREATE TABLE #SQLCommands(ID INT IDENTITY,sQuery TEXT)

INSERT INTO #SQLCommands
	SELECT
	'----SCRIPT FOR GIS_USER_DEF_DETAIL TABLE FROM '  + CONVERT(VARCHAR(50),@nFromGisUserDefId) 
INSERT INTO #SQLCommands		
	SELECT	
	'DECLARE @DetailCaption' + RTRIM(LTRIM(GUDD.gis_user_def_detail_id)) + ' INT ' + CHAR(13) +
	'EXECUTE spu_pm_caption_id_return 1, '''+ REPLACE(ISNULL(CONVERT(VARCHAR(50),GUDD.description),'NULL'),'''','''''') + ''', @DetailCaption' + RTRIM(LTRIM(GUDD.gis_user_def_detail_id)) +' OUTPUT ' + CHAR(13) +
	'UPDATE GIS_User_Def_Detail SET ' +  CHAR(13) +
	'caption_id = @DetailCaption' + RTRIM(LTRIM(GUDD.gis_user_def_detail_id))  + ',' +
	'code = '''+ ISNULL(RTRIM(CONVERT(VARCHAR(50),GUDD.code)),'NULL') +  ''',' +
	'description =''' + REPLACE(ISNULL(CONVERT(VARCHAR(50),GUDD.description),'NULL'),'''','''''')+''',' +
	'is_deleted ='  + ISNULL(CONVERT(VARCHAR(50),GUDD.is_deleted),'NULL')+ ',' +
	'effective_date ='''+ ISNULL(CONVERT(VARCHAR(50),GUDD.effective_date),'NULL')+''','+
	'Parent = '+ ISNULL(CONVERT(VARCHAR(50),GUDD.Parent),'NULL') + ' ' + CHAR(13) +
	'WHERE  gis_user_def_detail_id = ' +  RTRIM(LTRIM(GUDD.gis_user_def_detail_id))	+ CHAR(13) 
	FROM GIS_User_Def_Detail GUDD
	WHERE GIS_USER_DEF_DETAIL_ID BETWEEN @nFromGisUserDefId AND @nToGisUserDefId
	
SELECT sQuery FROM #SQLCommands
DROP TABLE #SQLCommands

END

