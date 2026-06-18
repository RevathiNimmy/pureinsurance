ddldropprocedure 'spu_APIE_get_UML_Headers_Script'
GO
CREATE PROCEDURE spu_APIE_get_UML_Headers_Script

AS
BEGIN

CREATE TABLE #SQLCommands(ID INT IDENTITY,sQuery TEXT)

--CREATE TEMP RECORDS FIRST
DECLARE @nThisMaxCountHeader INT
DECLARE @nThisMaxCount INT
SELECT @nThisMaxCountHeader =MAX(gis_user_def_header_id) FROM GIS_User_Def_Header
SELECT @nThisMaxCount =MAX(gis_user_def_detail_id) FROM GIS_User_Def_Detail

INSERT INTO #SQLCommands
SELECT
'----CREATE TEMP HEADER RECORDS -- ' +  CHAR(13) +
'DECLARE @nMaxCountHeader INT ' +  CHAR(13) +
'SELECT @nMaxCountHeader =MAX(gis_user_def_header_id) FROM GIS_User_Def_Header' +  CHAR(13) +
'WHILE (@nMaxCountHeader<' +ISNULL(CONVERT(VARCHAR(50),@nThisMaxCountHeader),'0') +') ' +  CHAR(13) +
'BEGIN ' +  CHAR(13) +
'INSERT INTO GIS_User_Def_Header (caption_id,code,description,is_deleted,effective_date,Parent) '  + CHAR(13) +
	'values(1,''NA'',''NA'',1,GETDATE(),-1) ' + CHAR(13) +
	'SET @nMaxCountHeader = @nMaxCountHeader + 1 ' + CHAR(13) +	
'END '+  CHAR(13) 
	
INSERT INTO #SQLCommands
SELECT
'----CREATE TEMP DETAIL RECORDS -- ' +  CHAR(13) +
'DECLARE @nMaxCount INT ' +  CHAR(13) +
'SELECT @nMaxCount =MAX(gis_user_def_detail_id) FROM GIS_User_Def_Detail' +  CHAR(13) +
'WHILE (@nMaxCount<' +ISNULL(CONVERT(VARCHAR(50),@nThisMaxCount),'0') +') ' +  CHAR(13) +
'BEGIN ' +  CHAR(13) +
'INSERT INTO GIS_User_Def_Detail (gis_user_def_header_id,caption_id,code,description,is_deleted,effective_date,Parent) ' + CHAR(13) +
	'values(1,1,''NA'',''NA'',1,GETDATE(),-1) ' + CHAR(13) +	
	'SET @nMaxCount = @nMaxCount + 1 ' + CHAR(13) +	
'END '+  CHAR(13) 
	

INSERT INTO #SQLCommands
	SELECT
	'----UPDATE SCRIPT FOR GIS_USER_DEF_HEADER TABLE' 	

INSERT INTO #SQLCommands
	SELECT
	'DECLARE @Caption' + RTRIM(LTRIM(GUDH.gis_user_def_header_id)) + ' INT ' +  CHAR(13) +		
	'EXECUTE spu_pm_caption_id_return 1, '''+ REPLACE(ISNULL(CONVERT(VARCHAR(50),GUDH.description),'NULL'),'''','''''') + ''', @Caption' + RTRIM(LTRIM(GUDH.gis_user_def_header_id)) +' OUTPUT ' + CHAR(13) +
	
	'UPDATE GIS_User_Def_Header SET ' +  CHAR(13) +
	'caption_id = @Caption' + RTRIM(LTRIM(GUDH.gis_user_def_header_id))  + ',' +
	'code = '''+ ISNULL(RTRIM(CONVERT(VARCHAR(50),GUDH.code)),'NULL') +  ''',' +
	'description =''' + REPLACE(ISNULL(CONVERT(VARCHAR(50),GUDH.description),'NULL'),'''','''''')+''',' +
	'is_deleted ='  + ISNULL(CONVERT(VARCHAR(50),GUDH.is_deleted),'NULL')+ ',' +
	'effective_date ='''+ ISNULL(CONVERT(VARCHAR(50),GUDH.effective_date),'NULL')+''','+
	'Parent = '+ ISNULL(CONVERT(VARCHAR(50),GUDH.Parent),'NULL') + ' ' + CHAR(13) +
	'WHERE  gis_user_def_header_id = ' +  RTRIM(LTRIM(GUDH.gis_user_def_header_id)) + CHAR(13) 

	FROM GIS_User_Def_Header GUDH

SELECT sQuery FROM #SQLCommands

DROP TABLE #SQLCommands
END