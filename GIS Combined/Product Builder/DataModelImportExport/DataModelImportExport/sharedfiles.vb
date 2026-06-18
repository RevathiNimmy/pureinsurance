Module sharedfiles

    Public Const kGISData As String = "spu_GIS_ExportMPDataModel"
    Public Const kCheckSP As String = "SELECT 1 FROM sysobjects WHERE xtype='P' AND name='spu_GIS_ExportMPDataModel'"
    Public Const kExportSP As String =  "DDLDropProcedure 'spu_GetProductAndRisk'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_GetProductAndRisk @sDataModelCode VARCHAR(10)" & vbCrLf &  _
"AS" & vbCrLf &  _
"    SELECT RT.RISK_TYPE_ID," & vbCrLf &  _
"           RT.CODE" & vbCrLf &  _
"    INTO   #RISKCODES" & vbCrLf &  _
"    FROM   RISK_TYPE RT" & vbCrLf &  _
"           JOIN GIS_SCREEN GSCR" & vbCrLf &  _
"             ON RT.GIS_SCREEN_ID = GSCR.GIS_SCREEN_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GDM" & vbCrLf &  _
"             ON GDM.GIS_DATA_MODEL_ID = GSCR.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GDM.GIS_DATA_MODEL_TYPE_ID = 1" & vbCrLf &  _
"           AND GDM.CODE = @sDataModelCode" & vbCrLf &  _
"    SELECT DISTINCT RTG.*" & vbCrLf &  _
"    INTO   #RISKTYPEGROUP" & vbCrLf &  _
"    FROM   RISK_TYPE_GROUP RTG" & vbCrLf &  _
"           JOIN RISK_TYPE_USAGE RTU" & vbCrLf &  _
"             ON RTU.RISK_TYPE_GROUP_ID = RTG.RISK_TYPE_GROUP_ID" & vbCrLf &  _
"           JOIN #RISKCODES" & vbCrLf &  _
"             ON #RISKCODES.RISK_TYPE_ID = RTU.RISK_TYPE_ID" & vbCrLf &  _
"    SELECT PROD.*" & vbCrLf &  _
"    INTO   #PRODUCTS" & vbCrLf &  _
"    FROM   PRODUCT PROD" & vbCrLf &  _
"           JOIN PRODUCT_RISK_TYPE_GROUP PRTG" & vbCrLf &  _
"             ON PROD.PRODUCT_ID = PRTG.PRODUCT_ID" & vbCrLf &  _
"           JOIN #RISKTYPEGROUP RTG" & vbCrLf &  _
"             ON RTG.RISK_TYPE_GROUP_ID = PRTG.RISK_TYPE_GROUP_ID" & vbCrLf &  _
"    IF EXISTS(SELECT *" & vbCrLf &  _
"              FROM   #RISKCODES)" & vbCrLf &  _
"      BEGIN" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '---THIS BIT OF SQL WILL COPY RISK_TYPE_GROUP------- '" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @MAXGROUPID INT '" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @GROUPCAPTIONID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @MAXGROUPID =MAX(RISK_TYPE_GROUP_ID) FROM RISK_TYPE_GROUP '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'IF NOT EXISTS(SELECT RISK_TYPE_GROUP_ID FROM RISK_TYPE_GROUP WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(CODE)) + ''') ' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'SET @MAXGROUPID = @MAXGROUPID + 1 '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"                 + RTRIM(LTRIM(DESCRIPTION))" & vbCrLf &  _
"                 + ''', @GROUPCAPTIONID OUTPUT ' + CHAR(13)" & vbCrLf &  _
"                 + 'INSERT INTO RISK_TYPE_GROUP (" & vbCrLf &  _
"					RISK_TYPE_GROUP_ID," & vbCrLf &  _
"					CAPTION_ID," & vbCrLf &  _
"					CODE," & vbCrLf &  _
"					DESCRIPTION," & vbCrLf &  _
"					IS_DELETED," & vbCrLf &  _
"					EFFECTIVE_DATE) VALUES(" & vbCrLf &  _
"					@MAXGROUPID," & vbCrLf &  _
"					@GROUPCAPTIONID,'''" & vbCrLf &  _
"                 + ISNULL(CONVERT(CHAR(10), CODE), 'NULL')" & vbCrLf &  _
"                 + ''','''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(255), DESCRIPTION), 'NULL')" & vbCrLf &  _
"                 + ''','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(10), IS_DELETED), 'NULL')" & vbCrLf &  _
"                 + ','''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), EFFECTIVE_DATE), 'NULL')" & vbCrLf &  _
"                 + ''')' + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"          FROM   #RISKTYPEGROUP" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '---THIS BIT OF SQL WILL COPY RISK_TYPE-------'" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @MAXRISKTYPEID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'DECLARE @RISKTYPECAPTIONID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @MAXRISKTYPEID =MAX(RISK_TYPE_ID) FROM RISK_TYPE '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'IF NOT EXISTS(SELECT RISK_TYPE_ID FROM RISK_TYPE WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(RT.CODE)) + ''') ' + CHAR(13)" & vbCrLf &  _
"                 + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"                 + 'SET @MAXRISKTYPEID = @MAXRISKTYPEID + 1 '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"                 + RTRIM(LTRIM(DESCRIPTION))" & vbCrLf &  _
"                 + ''', @RISKTYPECAPTIONID OUTPUT ' + CHAR(13)" & vbCrLf &  _
"                 + 'INSERT INTO RISK_TYPE ( RISK_TYPE_ID,RISK_FOLDER_TYPE_ID,CAPTION_ID,CODE,DESCRIPTION,EFFECTIVE_DATE,IS_DELETED,ACCUMULATION_LEVEL," & vbCrLf &  _
"					GIS_SCREEN_ID,IS_DEFERRED_RI_PERMITTED,CLAIMS_GIS_SCREEN_ID,CLAIMS_IS_POST_TAXES,DISPLAY_REINSURANCE_SCREEN,SHOW_INFORMATION_CHECKLIST) VALUES ( " & vbCrLf &  _
"					@MAXRISKTYPEID,NULL,@RISKTYPECAPTIONID,'''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), RT.CODE), 'NULL')" & vbCrLf &  _
"                 + ''','''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), DESCRIPTION), 'NULL')" & vbCrLf &  _
"                 + ''','''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), EFFECTIVE_DATE), 'NULL')" & vbCrLf &  _
"                 + ''','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), IS_DELETED), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), ACCUMULATION_LEVEL), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), GIS_SCREEN_ID), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), IS_DEFERRED_RI_PERMITTED), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), CLAIMS_GIS_SCREEN_ID), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), CLAIMS_IS_POST_TAXES), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), DISPLAY_REINSURANCE_SCREEN), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), SHOW_INFORMATION_CHECKLIST), 'NULL')" & vbCrLf &  _
"                 + ')' + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"          FROM   RISK_TYPE RT" & vbCrLf &  _
"                 JOIN #RISKCODES" & vbCrLf &  _
"                   ON #RISKCODES.RISK_TYPE_ID = RT.RISK_TYPE_ID" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '---THIS BIT OF SQL WILL CREATE RISK_TYPE_USUAGE ENTRIES-------'" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @TEMPRISK_TYPE_ID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'DECLARE @TEMPRISK_TYPE_GROUP_ID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'SELECT @TEMPRISK_TYPE_ID = RISK_TYPE_ID FROM RISK_TYPE FROM WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(RC.CODE)) + '''' + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @TEMPRISK_TYPE_GROUP_ID = RISK_TYPE_GROUP_ID FROM RISK_TYPE_GROUP FROM WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(RTG.CODE)) + '''' + CHAR(13)" & vbCrLf &  _
"                 + 'IF NOT EXISTS(SELECT * FROM RISK_TYPE_USAGE WHERE RISK_TYPE_ID =@TEMPRISK_TYPE_ID AND RISK_TYPE_GROUP_ID =@TEMPRISK_TYPE_GROUP_ID) '" & vbCrLf &  _
"                 + CHAR(13) + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"                 + 'INSERT INTO RISK_TYPE_USAGE (RISK_TYPE_ID,RISK_TYPE_GROUP_ID) VALUES (@TEMPRISK_TYPE_ID,@TEMPRISK_TYPE_GROUP_ID) '" & vbCrLf &  _
"                 + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"          FROM   RISK_TYPE_USAGE RTU" & vbCrLf &  _
"                 JOIN #RISKTYPEGROUP RTG" & vbCrLf &  _
"                   ON RTG.RISK_TYPE_GROUP_ID = RTU.RISK_TYPE_GROUP_ID" & vbCrLf &  _
"                 JOIN #RISKCODES RC" & vbCrLf &  _
"                   ON RC.RISK_TYPE_ID = RTU.RISK_TYPE_ID" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '---THIS BIT OF SQL WILL COPY PRODUCT-------'" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @MAXPRODUCTID INT '" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @PRODUCTCAPTIONID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @MAXPRODUCTID =MAX(PRODUCT_ID) FROM PRODUCT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'IF NOT EXISTS(SELECT PRODUCT_ID FROM PRODUCT WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(CODE)) + ''')' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'SET @MAXPRODUCTID = @MAXPRODUCTID + 1 '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"                 + RTRIM(LTRIM(DESCRIPTION))" & vbCrLf &  _
"                 + ''', @PRODUCTCAPTIONID OUTPUT ' + CHAR(13)" & vbCrLf &  _
"                 + 'INSERT INTO PRODUCT (" & vbCrLf &  _
"					PRODUCT_ID," & vbCrLf &  _
"					CAPTION_ID," & vbCrLf &  _
"					CODE," & vbCrLf &  _
"					DESCRIPTION," & vbCrLf &  _
"					EFFECTIVE_DATE," & vbCrLf &  _
"					IS_DELETED," & vbCrLf &  _
"					CAN_MAKE_LIVE_INVOICE," & vbCrLf &  _
"					CAN_MAKE_LIVE_INSTALMENTS," & vbCrLf &  _
"					CAN_MAKE_LIVE_PAYNOW," & vbCrLf &  _
"					PRODUCE_SCHEDULE," & vbCrLf &  _
"					PRODUCE_CERTIFICATE," & vbCrLf &  _
"					PRODUCE_DEBIT_NOTE) VALUES (@MAXPRODUCTID,@PRODUCTCAPTIONID,'" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), CODE), 'NULL')" & vbCrLf &  _
"                 + ''','''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), DESCRIPTION), 'NULL')" & vbCrLf &  _
"                 + ''','''" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), EFFECTIVE_DATE), 'NULL')" & vbCrLf &  _
"                 + ''','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), IS_DELETED), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), CAN_MAKE_LIVE_INVOICE), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), CAN_MAKE_LIVE_INSTALMENTS), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), CAN_MAKE_LIVE_PAYNOW), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), PRODUCE_SCHEDULE), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), PRODUCE_CERTIFICATE), 'NULL')" & vbCrLf &  _
"                 + ','" & vbCrLf &  _
"                 + ISNULL(CONVERT(VARCHAR(20), PRODUCE_DEBIT_NOTE), 'NULL')" & vbCrLf &  _
"                 + ') ' + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"          FROM   #PRODUCTS" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '---THIS BIT OF SQL WILL CREATE PRODUCT_RISK_TYPE_GROUP ENTRIES-------'" & vbCrLf &  _
"                 + CHAR(13) + 'DECLARE @PPRODUCT_ID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'DECLARE @PRISK_TYPE_GROUP_ID INT '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'SELECT @PPRODUCT_ID = RISK_TYPE_ID FROM RISK_TYPE FROM WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(P.CODE)) + '''' + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @PRISK_TYPE_GROUP_ID = RISK_TYPE_GROUP_ID FROM RISK_TYPE_GROUP FROM WHERE CODE ='''" & vbCrLf &  _
"                 + RTRIM(LTRIM(RTG.CODE)) + '''' + CHAR(13)" & vbCrLf &  _
"                 + 'IF NOT EXISTS(SELECT * FROM PRODUCT_RISK_TYPE_GROUP WHERE PRODUCT_ID =@PPRODUCT_ID AND RISK_TYPE_GROUP_ID =@PRISK_TYPE_GROUP_ID) '" & vbCrLf &  _
"                 + CHAR(13) + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"                 + 'INSERT INTO RISK_TYPE_USAGE (PRODUCT_ID,RISK_TYPE_GROUP_ID) VALUES (@PPRODUCT_ID,@PRISK_TYPE_GROUP_ID) '" & vbCrLf &  _
"                 + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"          FROM   PRODUCT_RISK_TYPE_GROUP PRTG" & vbCrLf &  _
"                 JOIN #PRODUCTS P" & vbCrLf &  _
"                   ON PRTG.PRODUCT_ID = P.PRODUCT_ID" & vbCrLf &  _
"                 JOIN #RISKTYPEGROUP RTG" & vbCrLf &  _
"                   ON PRTG.RISK_TYPE_GROUP_ID = RTG.RISK_TYPE_GROUP_ID" & vbCrLf &  _
"      END" & vbCrLf &  _
"    DROP TABLE #RISKCODES" & vbCrLf &  _
"    DROP TABLE #RISKTYPEGROUP" & vbCrLf &  _
"    DROP TABLE #PRODUCTS" & vbCrLf &  _
"GO" & vbCrLf &  _
"--USE @DATAMODELID AS DATAMODEL ID" & vbCrLf &  _
"DDLDropProcedure 'spu_CreatePolicyBinder'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_CreatePolicyBinder @sDataModelCode VARCHAR(10)," & vbCrLf &  _
"                                        @DATA           VARCHAR(MAX) OUTPUT" & vbCrLf &  _
"AS" & vbCrLf &  _
"    SET @sDataModelCode =RTRIM(@sDataModelCode)" & vbCrLf &  _
"    SELECT @DATA = 'DECLARE  @MAXPBOID INT ' + CHAR(13)" & vbCrLf &  _
"                   + 'DECLARE  @MAXPBPID INT ' + CHAR(13)" & vbCrLf &  _
"                   + 'SELECT @MAXPBOID = MAX(GIS_OBJECT_ID)+1 FROM GIS_OBJECT WITH(NOLOCK) '" & vbCrLf &  _
"                   + CHAR(13)" & vbCrLf &  _
"                   + 'SELECT @MAXPBPID = MAX(GIS_PROPERTY_ID)+1 FROM GIS_PROPERTY WITH(NOLOCK) '" & vbCrLf &  _
"                   + CHAR(13) + 'INSERT INTO GIS_OBJECT (GIS_OBJECT_ID," & vbCrLf &  _
"					GIS_DATA_MODEL_ID," & vbCrLf &  _
"					OBJECT_NAME," & vbCrLf &  _
"					TABLE_NAME," & vbCrLf &  _
"					MAX_INSTANCES," & vbCrLf &  _
"					IS_QUOTE_OBJECT," & vbCrLf &  _
"					PARENT_OBJECT_ID," & vbCrLf &  _
"					POLARIS_OBJECT_ID," & vbCrLf &  _
"					IS_SELECTABLE_FOR_SCREEN," & vbCrLf &  _
"					IS_NON_GIS," & vbCrLf &  _
"					EDIT_FLAGS) ' + CHAR(13) + 'VALUES ('" & vbCrLf &  _
"                   + '@MAXPBOID,@DATAMODELID,'''" & vbCrLf &  _
"                   + @sDataModelCode + '_POLICY_BINDER'','''" & vbCrLf &  _
"                   + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER'',1,0,NULL,NULL,0,0,0) '" & vbCrLf &  _
"                   + CHAR(13) + 'INSERT INTO GIS_PROPERTY (GIS_PROPERTY_ID," & vbCrLf &  _
"					GIS_OBJECT_ID," & vbCrLf &  _
"					PROPERTY_NAME," & vbCrLf &  _
"					COLUMN_NAME," & vbCrLf &  _
"					DATA_TYPE," & vbCrLf &  _
"					IS_INPUT_PROPERTY," & vbCrLf &  _
"					IS_IDENTIFYING_PROPERTY," & vbCrLf &  _
"					IS_PRIMARY_KEY," & vbCrLf &  _
"					POLARIS_PROPERTY_ID," & vbCrLf &  _
"					IS_DELETED," & vbCrLf &  _
"					IS_SEARCH_PROPERTY," & vbCrLf &  _
"					INDEX_LINKING_ID," & vbCrLf &  _
"					EDIT_FLAGS," & vbCrLf &  _
"					SPECIALS_TYPE," & vbCrLf &  _
"					SPECIALS_TYPE_REFERENCE)  '" & vbCrLf &  _
"                   + CHAR(13) + 'VALUES ('" & vbCrLf &  _
"                   + '@MAXPBPID,@MAXPBOID,''' + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER_ID'',''' + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER_ID'',2,0,1,1,NULL,0,0,NULL,0,0,NULL)'" & vbCrLf &  _
"                   + CHAR(13) + 'SET @MAXPBPID= @MAXPBPID+1 '" & vbCrLf &  _
"                   + CHAR(13) + 'INSERT INTO GIS_PROPERTY (GIS_PROPERTY_ID," & vbCrLf &  _
"					GIS_OBJECT_ID," & vbCrLf &  _
"					PROPERTY_NAME," & vbCrLf &  _
"					COLUMN_NAME," & vbCrLf &  _
"					DATA_TYPE," & vbCrLf &  _
"					IS_INPUT_PROPERTY," & vbCrLf &  _
"					IS_IDENTIFYING_PROPERTY," & vbCrLf &  _
"					IS_PRIMARY_KEY," & vbCrLf &  _
"					POLARIS_PROPERTY_ID," & vbCrLf &  _
"					IS_DELETED," & vbCrLf &  _
"					IS_SEARCH_PROPERTY," & vbCrLf &  _
"					INDEX_LINKING_ID," & vbCrLf &  _
"					EDIT_FLAGS," & vbCrLf &  _
"					SPECIALS_TYPE," & vbCrLf &  _
"					SPECIALS_TYPE_REFERENCE)  '" & vbCrLf &  _
"                   + CHAR(13) + 'VALUES ('" & vbCrLf &  _
"                   + '@MAXPBPID,@MAXPBOID,''GIS_POLICY_LINK_ID'',''GIS_POLICY_LINK_ID'',2,0,0,0,NULL,0,0,NULL,0,0,NULL)'" & vbCrLf &  _
"                   + CHAR(13) + 'CREATE TABLE ' + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER(' + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER_ID INT NOT NULL," & vbCrLf &  _
"					GIS_POLICY_LINK_ID INT NULL)' + CHAR(13)" & vbCrLf &  _
"                   + ' ALTER TABLE ' + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER ADD PRIMARY KEY NONCLUSTERED ('" & vbCrLf &  _
"                   + @sDataModelCode + '_POLICY_BINDER_ID ASC '" & vbCrLf &  _
"                   + ')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] '" & vbCrLf &  _
"                   + CHAR(13) + ' ALTER TABLE ' + @sDataModelCode" & vbCrLf &  _
"                   + '_POLICY_BINDER  WITH CHECK ADD FOREIGN KEY(GIS_POLICY_LINK_ID) '" & vbCrLf &  _
"                   + 'REFERENCES GIS_POLICY_LINK (GIS_POLICY_LINK_ID) ON DELETE CASCADE '" & vbCrLf &  _
"                   + CHAR(13)" & vbCrLf &  _
"GO" & vbCrLf &  _
"DDLDropProcedure 'spu_GetUDLFieldNames'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_GetUDLFieldNames @TABLECODE          VARCHAR(255)," & vbCrLf &  _
"                                      @FIELDS             VARCHAR(255) OUTPUT," & vbCrLf &  _
"                                      @FIELDSWITHDATATYPE VARCHAR(300) OUTPUT" & vbCrLf &  _
"AS" & vbCrLf &  _
"    DECLARE @TABLENAMEID VARCHAR(255)" & vbCrLf &  _
"    DECLARE @TABLENAME VARCHAR(255)" & vbCrLf &  _
"    SET @TABLENAME = 'UDL_' + LTRIM(RTRIM(@TABLECODE))" & vbCrLf &  _
"    SET @TABLENAMEID=RTRIM(@TABLENAME) + '_ID'" & vbCrLf &  _
"    SET @FIELDS=''" & vbCrLf &  _
"    SET @FIELDSWITHDATATYPE=''" & vbCrLf &  _
"    SELECT @FIELDS = @FIELDS + ',' + RTRIM(LTRIM(NAME))," & vbCrLf &  _
"           @FIELDSWITHDATATYPE = @FIELDSWITHDATATYPE + ',' + RTRIM(LTRIM(NAME))" & vbCrLf &  _
"                                 + ' VARCHAR(255)'" & vbCrLf &  _
"    FROM   SYS.ALL_COLUMNS" & vbCrLf &  _
"    WHERE  OBJECT_ID IN" & vbCrLf &  _
"           --SELECT  NAME FROM SYS.ALL_COLUMNS WHERE OBJECT_ID IN " & vbCrLf &  _
"           (SELECT OBJECT_ID" & vbCrLf &  _
"            FROM   SYS.OBJECTS" & vbCrLf &  _
"            WHERE  NAME = @TABLENAME)" & vbCrLf &  _
"           AND NAME NOT IN ( @TABLENAMEID, 'CAPTION_ID', 'CODE', 'DESCRIPTION'," & vbCrLf &  _
"                             'IS_DELETED', 'EFFECTIVE_DATE', 'UDL_VERSION' )" & vbCrLf &  _
"    SELECT @FIELDS = SUBSTRING(@FIELDS, 2, LEN(@FIELDS))" & vbCrLf &  _
"    SELECT @FIELDSWITHDATATYPE = SUBSTRING(@FIELDSWITHDATATYPE, 2, LEN(@FIELDSWITHDATATYPE))" & vbCrLf &  _
"GO" & vbCrLf &  _
"DDLDropProcedure 'spu_GetUDLValue'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_GetUDLValue @TABLECODE VARCHAR(255)," & vbCrLf &  _
"                                 @FIELDS    VARCHAR(MAX)," & vbCrLf &  _
"                                 @FIELDCODE VARCHAR(MAX)," & vbCrLf &  _
"                                 @DATA      VARCHAR(MAX) OUTPUT" & vbCrLf &  _
"AS" & vbCrLf &  _
"    DECLARE @SQL VARCHAR(MAX)" & vbCrLf &  _
"    DECLARE @MODIFIEDFILEDS VARCHAR(MAX)" & vbCrLf &  _
"    IF @FIELDS <> ''" & vbCrLf &  _
"--      SET @FIELDS = ',' + @FIELDS" & vbCrLf &  _
"    SET @MODIFIEDFILEDS = 'CODE,DESCRIPTION,' + @FIELDS" & vbCrLf &  _
"    SET @MODIFIEDFILEDS = '''#''+ISNULL('" & vbCrLf &  _
"                          + REPLACE(@MODIFIEDFILEDS, ',', ',''NULL'')+''#,#''+ISNULL(')" & vbCrLf &  _
"                          + ',''NULL'')+''#'''" & vbCrLf &  _
"    CREATE TABLE #TEMPUDL" & vbCrLf &  _
"      (" & vbCrLf &  _
"         SQLDATA VARCHAR(500)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    SELECT @SQL = 'INSERT INTO #TEMPUDL  SELECT '" & vbCrLf &  _
"                  + @MODIFIEDFILEDS + ' FROM UDL_'" & vbCrLf &  _
"                  + LTRIM(@TABLECODE) + ' WHERE CODE = '''" & vbCrLf &  _
"                  + @FIELDCODE + ''''" & vbCrLf &  _
"    EXECUTE(@SQL)" & vbCrLf &  _
"    SELECT TOP 1 @DATA = REPLACE(#TEMPUDL.SQLDATA, '#', '''''')" & vbCrLf &  _
"    FROM   #TEMPUDL" & vbCrLf &  _
"    DROP TABLE #TEMPUDL" & vbCrLf &  _
"GO" & vbCrLf &  _
"--THIS SP IS CALLED VIA OTHER SP SO EXPECTS TEMPTABLE TO BE THERE WITH FOLLOWING STRUCTURE" & vbCrLf &  _
"--CREATE TABLE #QUERIES(ID INT IDENTITY,QUERY TEXT)" & vbCrLf &  _
"DDLDropProcedure 'spu_UDL_Script'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_UDL_Script @UDLCODE VARCHAR(255)" & vbCrLf &  _
"AS" & vbCrLf &  _
"    SET @UDLCODE= LTRIM(@UDLCODE)" & vbCrLf &  _
"    DECLARE @UDLDESCRIPTION VARCHAR(255)" & vbCrLf &  _
"    DECLARE @FIELDS VARCHAR(200)" & vbCrLf &  _
"    DECLARE @FIELDSWITHDATATYPE VARCHAR(400)" & vbCrLf &  _
"    EXEC spu_GetUDLFieldNames" & vbCrLf &  _
"      @UDLCODE," & vbCrLf &  _
"      @FIELDS OUTPUT," & vbCrLf &  _
"      @FIELDSWITHDATATYPE OUTPUT" & vbCrLf &  _
"    DECLARE @COMMAND VARCHAR(200)" & vbCrLf &  _
"    EXECUTE DDLDropTable" & vbCrLf &  _
"      'TRSCURRENTTABLE'" & vbCrLf &  _
"    SELECT @UDLDESCRIPTION = RTRIM(LTRIM(DESCRIPTION))" & vbCrLf &  _
"    FROM   GIS_LIST_TYPE" & vbCrLf &  _
"    WHERE  CODE = @UDLCODE" & vbCrLf &  _
"    INSERT INTO #QUERIES" & vbCrLf &  _
"    SELECT '----**THIS SCRIPT IS GENERATED FOR UDL '" & vbCrLf &  _
"           + RTRIM(LTRIM(@UDLCODE)) + ' **------'" & vbCrLf &  _
"    INSERT INTO #QUERIES" & vbCrLf &  _
"    SELECT 'IF  NOT EXISTS(SELECT GIS_LIST_TYPE_ID FROM GIS_LIST_TYPE WHERE CODE= '''" & vbCrLf &  _
"           + @UDLCODE + ''' AND DESCRIPTION= '''" & vbCrLf &  _
"           + @UDLDESCRIPTION + ''') ' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"           + CHAR(13) + 'EXEC SPU_GIS_LIST_TYPE_ADD '''" & vbCrLf &  _
"           + @UDLCODE + ''',''' + @UDLDESCRIPTION + ''' '" & vbCrLf &  _
"           + CHAR(13)" & vbCrLf &  _
"           + 'EXEC SPU_GIS_LIST_PM_CAPTION ''UDL_'" & vbCrLf &  _
"           + @UDLCODE + ''',''' + @FIELDSWITHDATATYPE + ''' '" & vbCrLf &  _
"           + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"    IF EXISTS(SELECT NULL" & vbCrLf &  _
"              FROM   SYSOBJECTS" & vbCrLf &  _
"              WHERE  NAME = 'UDL_' + RTRIM(@UDLCODE)" & vbCrLf &  _
"                     AND XTYPE = 'U')" & vbCrLf &  _
"      BEGIN" & vbCrLf &  _
"          SELECT @COMMAND = 'SELECT UDL_' + RTRIM(@UDLCODE)" & vbCrLf &  _
"                            + '_ID AS UDLID,* INTO TRSCURRENTTABLE FROM UDL_'" & vbCrLf &  _
"                            + @UDLCODE" & vbCrLf &  _
"          EXECUTE(@COMMAND)" & vbCrLf &  _
"          DECLARE @MAXCNT INT" & vbCrLf &  _
"          DECLARE @INDEX INT" & vbCrLf &  _
"          DECLARE @DATA VARCHAR(500)" & vbCrLf &  _
"          DECLARE @FIELDCODE VARCHAR(25)" & vbCrLf &  _
"          SELECT @INDEX = 1," & vbCrLf &  _
"                 @MAXCNT = MAX(UDLID)" & vbCrLf &  _
"          FROM   TRSCURRENTTABLE" & vbCrLf &  _
"          IF @MAXCNT > 0" & vbCrLf &  _
"            BEGIN" & vbCrLf &  _
"                WHILE( @INDEX <= @MAXCNT )" & vbCrLf &  _
"                  BEGIN" & vbCrLf &  _
"                      SELECT @FIELDCODE = CODE" & vbCrLf &  _
"                      FROM   TRSCURRENTTABLE" & vbCrLf &  _
"                      WHERE  UDLID = @INDEX" & vbCrLf &  _
"                      EXEC spu_GetUDLValue" & vbCrLf &  _
"                        @UDLCODE," & vbCrLf &  _
"                        @FIELDS," & vbCrLf &  _
"                        @FIELDCODE," & vbCrLf &  _
"                        @DATA OUTPUT" & vbCrLf &  _
"                      INSERT INTO #QUERIES" & vbCrLf &  _
"                      SELECT 'IF NOT EXISTS(SELECT * FROM UDL_'" & vbCrLf &  _
"                             + @UDLCODE + ' WHERE CODE = '''" & vbCrLf &  _
"                             + TRSCURRENTTABLE.CODE + ''') ' + CHAR(13)" & vbCrLf &  _
"                             + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"                             + 'EXEC SPU_GIS_LISTENTRY_ADD @TABLE=N''UDL_'" & vbCrLf &  _
"                             + @UDLCODE + ''', ' + '@FIELDS=N'',' + @FIELDS + ''', '" & vbCrLf &  _
"                             + '@DATA=N'',' + @DATA + ''',@EFFDATE=N'''" & vbCrLf &  _
"                             + CONVERT(VARCHAR(30), TRSCURRENTTABLE.EFFECTIVE_DATE)" & vbCrLf &  _
"                             + ''',@LANGUAGE_ID=1,@CAPTION=N'''" & vbCrLf &  _
"                             + TRSCURRENTTABLE.DESCRIPTION + ''' ' + CHAR(13)" & vbCrLf &  _
"                             + 'EXEC SPU_GIS_LIST_ADD_USAGE @LISTTYPE=N'''" & vbCrLf &  _
"                             + @UDLCODE + ''',@CODE=N'''" & vbCrLf &  _
"                             + TRSCURRENTTABLE.CODE" & vbCrLf &  _
"                             + ''',@VERSION=1,@EFFDATE='''" & vbCrLf &  _
"                             + CONVERT(VARCHAR(30), TRSCURRENTTABLE.EFFECTIVE_DATE)" & vbCrLf &  _
"                             + ''' ' + CHAR(13) + ' END ' + CHAR(13)" & vbCrLf &  _
"                      FROM   TRSCURRENTTABLE" & vbCrLf &  _
"                      WHERE  TRSCURRENTTABLE.UDLID = @INDEX" & vbCrLf &  _
"                      SET @INDEX=@INDEX + 1" & vbCrLf &  _
"                  END" & vbCrLf &  _
"            END" & vbCrLf &  _
"          DROP TABLE TRSCURRENTTABLE" & vbCrLf &  _
"      END" & vbCrLf &  _
"GO" & vbCrLf &  _
"DDLDropProcedure 'spu_CreateGISProperty'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_CreateGISProperty @TABLENAME  VARCHAR(100)," & vbCrLf &  _
"                                       @COLUMNNAME VARCHAR(100)," & vbCrLf &  _
"                                       @RSQL       VARCHAR(300) OUTPUT" & vbCrLf &  _
"AS" & vbCrLf &  _
"    SELECT @RSQL = 'EXECUTE DDLADDCOLUMN ''' + @TABLENAME" & vbCrLf &  _
"                   + ''',''' + @COLUMNNAME + ''' ,'' ' + CASE WHEN DATA_TYPE = 1 THEN 'DATETIME' WHEN DATA_TYPE = 2 THEN 'INT' WHEN DATA_TYPE = 5 THEN 'VARCHAR(255)' WHEN DATA_TYPE = 7 THEN 'VARCHAR(4000)' WHEN DATA_TYPE = 20 THEN 'TINYINT' WHEN DATA_TYPE = 21 THEN 'NUMERIC(19,4)' WHEN DATA_TYPE = 22 THEN 'NUMERIC(7,4)' WHEN DATA_TYPE = 23 THEN 'INT' ELSE 'VARCHAR(255)' END + ''',0'" & vbCrLf &  _
"    FROM   GIS_PROPERTY GISP" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID" & vbCrLf &  _
"    WHERE  GISO.TABLE_NAME = @TABLENAME" & vbCrLf &  _
"           AND GISP.COLUMN_NAME = @COLUMNNAME" & vbCrLf &  _
"GO" & vbCrLf &  _
"DDLDropProcedure 'spu_CreateGISObject'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_CreateGISObject @TABLENAME VARCHAR(100)," & vbCrLf &  _
"                                     @RSQL      VARCHAR(MAX) OUTPUT" & vbCrLf &  _
"AS" & vbCrLf &  _
"    DECLARE @CLMMAX INT" & vbCrLf &  _
"    DECLARE @CLMINDEX INT" & vbCrLf &  _
"    DECLARE @sDataModelCode VARCHAR(10)" & vbCrLf &  _
"    DECLARE @OTHERKEYS VARCHAR(500)" & vbCrLf &  _
"    DECLARE @COLUMNNAME VARCHAR(60)" & vbCrLf &  _
"    DECLARE @TABLEKEY VARCHAR(60)" & vbCrLf &  _
"    DECLARE @PARENTOBJECTNAME VARCHAR(60)" & vbCrLf &  _
"    DECLARE @OTHERFOREIGNKEYS VARCHAR(500)" & vbCrLf &  _
"    SELECT @sDataModelCode = LTRIM(RTRIM(GISDM.CODE))" & vbCrLf &  _
"    FROM   GIS_OBJECT GISO" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISDM.GIS_DATA_MODEL_ID = GISO.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISO.TABLE_NAME = @TABLENAME" & vbCrLf &  _
"    SET @TABLEKEY = @TABLENAME + '_ID'" & vbCrLf &  _
"    SELECT @PARENTOBJECTNAME = TABLE_NAME" & vbCrLf &  _
"    FROM   GIS_OBJECT" & vbCrLf &  _
"    WHERE  GIS_OBJECT_ID IN (SELECT PARENT_OBJECT_ID" & vbCrLf &  _
"                             FROM   GIS_OBJECT" & vbCrLf &  _
"                             WHERE  TABLE_NAME = @TABLENAME)" & vbCrLf &  _
"    CREATE TABLE #COLDETAILS" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID          INT IDENTITY," & vbCrLf &  _
"         COLUMN_NAME VARCHAR(100)," & vbCrLf &  _
"         DATA_TYPE   VARCHAR(100)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    INSERT INTO #COLDETAILS" & vbCrLf &  _
"    SELECT COLUMN_NAME," & vbCrLf &  _
"           CASE" & vbCrLf &  _
"             WHEN DATA_TYPE = 1 THEN 'DATETIME'" & vbCrLf &  _
"             WHEN DATA_TYPE = 2 THEN 'INT'" & vbCrLf &  _
"             WHEN DATA_TYPE = 5 THEN 'VARCHAR(255)'" & vbCrLf &  _
"             WHEN DATA_TYPE = 7 THEN 'VARCHAR(4000)'" & vbCrLf &  _
"             WHEN DATA_TYPE = 20 THEN 'TINYINT'" & vbCrLf &  _
"             WHEN DATA_TYPE = 21 THEN 'NUMERIC(19,4)'" & vbCrLf &  _
"             WHEN DATA_TYPE = 22 THEN 'NUMERIC(7,4)'" & vbCrLf &  _
"             WHEN DATA_TYPE = 23 THEN 'INT'" & vbCrLf &  _
"             ELSE 'VARCHAR(255)'" & vbCrLf &  _
"           END" & vbCrLf &  _
"    FROM   GIS_PROPERTY GISP" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID" & vbCrLf &  _
"    WHERE  GISO.TABLE_NAME = @TABLENAME" & vbCrLf &  _
"    ORDER  BY GISP.GIS_PROPERTY_ID ASC" & vbCrLf &  _
"    SELECT @CLMINDEX = 1," & vbCrLf &  _
"           @CLMMAX = MAX(ID)" & vbCrLf &  _
"    FROM   #COLDETAILS" & vbCrLf &  _
"    SELECT @OTHERKEYS = ''" & vbCrLf &  _
"    DECLARE @SETNN CHAR(10)" & vbCrLf &  _
"    IF @CLMMAX > 0" & vbCrLf &  _
"      BEGIN" & vbCrLf &  _
"          --SELECT @RSQL = 'DROP TABLE ' + @TABLENAME + CHAR(13) " & vbCrLf &  _
"          SELECT @RSQL = ' EXECUTE DDLDROPTABLE ' + @TABLENAME" & vbCrLf &  _
"                         + CHAR(13)" & vbCrLf &  _
"          SELECT @RSQL = @RSQL + ' CREATE TABLE ' + @TABLENAME + '( '" & vbCrLf &  _
"          WHILE( @CLMINDEX <= @CLMMAX )" & vbCrLf &  _
"            BEGIN" & vbCrLf &  _
"                SELECT @COLUMNNAME = COLUMN_NAME" & vbCrLf &  _
"                FROM   #COLDETAILS" & vbCrLf &  _
"                WHERE  ID = @CLMINDEX" & vbCrLf &  _
"                SET @SETNN=''" & vbCrLf &  _
"                IF SUBSTRING(@COLUMNNAME, 1, LEN(@sDataModelCode)) = @sDataModelCode" & vbCrLf &  _
"                   AND @COLUMNNAME <> @TABLEKEY" & vbCrLf &  _
"                  BEGIN" & vbCrLf &  _
"                      SELECT @OTHERKEYS = @OTHERKEYS + @COLUMNNAME + '###'" & vbCrLf &  _
"                  END" & vbCrLf &  _
"                IF SUBSTRING(@COLUMNNAME, 1, LEN(@sDataModelCode)) = @sDataModelCode" & vbCrLf &  _
"                  BEGIN" & vbCrLf &  _
"                      SET @SETNN=' NOT NULL '" & vbCrLf &  _
"                  END" & vbCrLf &  _
"                SELECT @RSQL = CASE" & vbCrLf &  _
"                                 WHEN @CLMINDEX < @CLMMAX THEN @RSQL + RTRIM(LTRIM(COLUMN_NAME)) + ' '" & vbCrLf &  _
"                                                               + RTRIM(LTRIM(DATA_TYPE)) + @SETNN + ', '" & vbCrLf &  _
"                                 ELSE @RSQL + RTRIM(LTRIM(COLUMN_NAME)) + ' '" & vbCrLf &  _
"                                      + RTRIM(LTRIM(DATA_TYPE)) + @SETNN + ')'" & vbCrLf &  _
"                               END" & vbCrLf &  _
"                FROM   #COLDETAILS" & vbCrLf &  _
"                WHERE  ID = @CLMINDEX" & vbCrLf &  _
"                SET @CLMINDEX = @CLMINDEX + 1" & vbCrLf &  _
"            END" & vbCrLf &  _
"      END" & vbCrLf &  _
"    --PRIMARY KEY ADD" & vbCrLf &  _
"    SELECT @RSQL = @RSQL + CHAR(13) + ' ALTER TABLE ' + @TABLENAME" & vbCrLf &  _
"                   + ' ADD PRIMARY KEY CLUSTERED ( ' + CHAR(13)" & vbCrLf &  _
"    SELECT @RSQL = @RSQL + REPLACE(@OTHERKEYS, '###', ' ASC,')" & vbCrLf &  _
"                   + @TABLEKEY + ' ASC ' + CHAR(13)" & vbCrLf &  _
"    SELECT @RSQL = @RSQL" & vbCrLf &  _
"                   + ')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]'" & vbCrLf &  _
"                   + CHAR(13)" & vbCrLf &  _
"    -- FOREIGN KEY	" & vbCrLf &  _
"    SELECT @OTHERFOREIGNKEYS = LEFT(@OTHERKEYS, LEN(@OTHERKEYS) - 3)" & vbCrLf &  _
"    SELECT @RSQL = @RSQL + ' ALTER TABLE ' + @TABLENAME" & vbCrLf &  _
"                   + '  WITH CHECK ADD FOREIGN KEY('" & vbCrLf &  _
"                   + REPLACE(@OTHERFOREIGNKEYS, '###', ',') + ')'" & vbCrLf &  _
"                   + CHAR(13)" & vbCrLf &  _
"    SELECT @RSQL = @RSQL + 'REFERENCES ' + @PARENTOBJECTNAME + ' ( '" & vbCrLf &  _
"                   + REPLACE(@OTHERFOREIGNKEYS, '###', ',')" & vbCrLf &  _
"                   + ') ON DELETE CASCADE ' + CHAR(13)" & vbCrLf &  _
"    --PRINT @RSQL" & vbCrLf &  _
"    --SELECT @RSQL" & vbCrLf &  _
"    DROP TABLE #COLDETAILS" & vbCrLf &  _
"GO" & vbCrLf &  _
"--THIS SP IS CALLED VIA OTHER SP SO EXPECTS TEMPTABLE TO BE THERE WITH FOLLOWING STRUCTURE" & vbCrLf &  _
"--CREATE TABLE #QUERIES(ID INT IDENTITY,QUERY TEXT)" & vbCrLf &  _
"DDLDropProcedure 'spu_UML_Script'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_UML_Script @UMLCODE     VARCHAR(20)," & vbCrLf &  _
"                                @CHILDCODE   VARCHAR(20)=NULL," & vbCrLf &  _
"                                @NEWPARENTID VARCHAR(20)=NULL OUTPUT" & vbCrLf &  _
"AS" & vbCrLf &  _
"    DECLARE @PARENTID INT" & vbCrLf &  _
"    DECLARE @PARENTCODE VARCHAR(20)" & vbCrLf &  _
"    DECLARE @CURRENTHEADERID INT" & vbCrLf &  _
"    DECLARE @PARENTOFCAPTION VARCHAR(10)" & vbCrLf &  _
"    SELECT @CURRENTHEADERID = GIS_USER_DEF_HEADER_ID," & vbCrLf &  _
"           @PARENTID = ISNULL(PARENT, -1)" & vbCrLf &  _
"    FROM   GIS_USER_DEF_HEADER" & vbCrLf &  _
"    WHERE  CODE = @UMLCODE" & vbCrLf &  _
"    IF @PARENTID > 0" & vbCrLf &  _
"       AND @NEWPARENTID IS NULL" & vbCrLf &  _
"      BEGIN" & vbCrLf &  _
"          SET @PARENTOFCAPTION=CONVERT(VARCHAR(10), @PARENTID)" & vbCrLf &  _
"          SELECT @PARENTCODE = CODE" & vbCrLf &  _
"          FROM   GIS_USER_DEF_HEADER" & vbCrLf &  _
"          WHERE  GIS_USER_DEF_HEADER_ID = @PARENTID" & vbCrLf &  _
"          EXECUTE spu_UML_Script" & vbCrLf &  _
"            @PARENTCODE," & vbCrLf &  _
"            @UMLCODE," & vbCrLf &  _
"            @NEWPARENTID OUTPUT" & vbCrLf &  _
"      END" & vbCrLf &  _
"    SET @PARENTOFCAPTION= ISNULL(@CHILDCODE, '')" & vbCrLf &  _
"    INSERT INTO #QUERIES" & vbCrLf &  _
"    SELECT '----**THIS SCRIPT IS GENERATED FOR UML '" & vbCrLf &  _
"           + RTRIM(LTRIM(@UMLCODE)) + ' **------'" & vbCrLf &  _
"    INSERT INTO #QUERIES" & vbCrLf &  _
"    SELECT 'DECLARE @CAPTION'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ' INT ' + CHAR(13)" & vbCrLf &  _
"           + 'DECLARE @ID'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ' INT ' + CHAR(13)" & vbCrLf &  _
"           + 'IF NOT EXISTS(SELECT GIS_USER_DEF_HEADER_ID FROM GIS_USER_DEF_HEADER WHERE CODE='''" & vbCrLf &  _
"           + RTRIM(LTRIM(@UMLCODE)) + ''') ' + CHAR(13)" & vbCrLf &  _
"           + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"           + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')" & vbCrLf &  _
"           + ''', @CAPTION'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ' OUTPUT ' + CHAR(13)" & vbCrLf &  _
"           + 'INSERT INTO GIS_USER_DEF_HEADER '" & vbCrLf &  _
"           + CHAR(13) + 'VALUES(@CAPTION'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ','''" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), CODE), 'NULL')" & vbCrLf &  _
"           + ''','''" & vbCrLf &  _
"           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')" & vbCrLf &  _
"           + ''','" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), IS_DELETED), 'NULL')" & vbCrLf &  _
"           + ','''" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), EFFECTIVE_DATE), 'NULL')" & vbCrLf &  _
"           + ''','" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), @NEWPARENTID), '-1')" & vbCrLf &  _
"           + ','" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), SYSTEM_GENERATED), '0')" & vbCrLf &  _
"           + ') ' + CHAR(13) + ' END ' + CHAR(13) + 'SELECT @ID'" & vbCrLf &  _
"           + RTRIM(LTRIM(@CURRENTHEADERID))" & vbCrLf &  _
"           + @PARENTOFCAPTION" & vbCrLf &  _
"           + ' = GIS_USER_DEF_HEADER_ID FROM GIS_USER_DEF_HEADER WHERE CODE='''" & vbCrLf &  _
"           + @UMLCODE + ''''" & vbCrLf &  _
"    FROM   GIS_USER_DEF_HEADER" & vbCrLf &  _
"    WHERE  GIS_USER_DEF_HEADER_ID = @CURRENTHEADERID" & vbCrLf &  _
"    INSERT INTO #QUERIES" & vbCrLf &  _
"    SELECT 'DECLARE @DETAILCAPTION'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_DETAIL_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ' INT ' + CHAR(13)" & vbCrLf &  _
"           + 'IF NOT EXISTS(SELECT GIS_USER_DEF_DETAIL_ID FROM GIS_USER_DEF_DETAIL WHERE GIS_USER_DEF_HEADER_ID = @ID'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ' AND  CODE = ''' + CODE" & vbCrLf &  _
"           + ''') ' + CHAR(13) + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"           + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')" & vbCrLf &  _
"           + ''', @DETAILCAPTION'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_DETAIL_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ' OUTPUT ' + CHAR(13)" & vbCrLf &  _
"           + 'INSERT INTO GIS_USER_DEF_DETAIL VALUES(@ID'" & vbCrLf &  _
"           + RTRIM(LTRIM(@CURRENTHEADERID))" & vbCrLf &  _
"           + ',@DETAILCAPTION'" & vbCrLf &  _
"           + RTRIM(LTRIM(GIS_USER_DEF_DETAIL_ID))" & vbCrLf &  _
"           + @PARENTOFCAPTION + ','''" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), CODE), 'NULL')" & vbCrLf &  _
"           + ''','''" & vbCrLf &  _
"           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')" & vbCrLf &  _
"           + ''','" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), IS_DELETED), 'NULL')" & vbCrLf &  _
"           + ','''" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), EFFECTIVE_DATE), 'NULL')" & vbCrLf &  _
"           + ''','" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), -1), 'NULL')" & vbCrLf &  _
"           + ',NULL,'" & vbCrLf &  _
"           + ISNULL(CONVERT(VARCHAR(50), SYSTEM_GENERATED), '0')" & vbCrLf &  _
"           + ') ' + CHAR(13) + --PUTTING LAST COLUMN NULL AS OF NOW " & vbCrLf &  _
"           ' END ' + CHAR(13)" & vbCrLf &  _
"    FROM   GIS_USER_DEF_DETAIL" & vbCrLf &  _
"    WHERE  GIS_USER_DEF_HEADER_ID = @CURRENTHEADERID" & vbCrLf &  _
"    SELECT @NEWPARENTID = '@ID' + RTRIM(LTRIM(@CURRENTHEADERID))" & vbCrLf &  _
"                          + @PARENTOFCAPTION" & vbCrLf &  _
"    RETURN" & vbCrLf &  _
"GO" & vbCrLf &  _
"DDLDropProcedure 'spu_GIS_ExportMPDataModel'" & vbCrLf &  _
"GO" & vbCrLf &  _
"CREATE PROCEDURE spu_GIS_ExportMPDataModel @sDataModelCode     VARCHAR(10)," & vbCrLf &  _
"                                           @COPYPRODUCTANDRISK INT=0," & vbCrLf &  _
"                                           @CAPTURESCREENS     INT=1" & vbCrLf &  _
"AS" & vbCrLf &  _
"    CREATE TABLE #QUERIES" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID    INT IDENTITY," & vbCrLf &  _
"         QUERY TEXT" & vbCrLf &  _
"      )" & vbCrLf &  _
"    INSERT INTO #QUERIES" & vbCrLf &  _
"    SELECT '--' + @sDataModelCode" & vbCrLf &  _
"    IF @CAPTURESCREENS = 1" & vbCrLf &  _
"      BEGIN" & vbCrLf &  _
"          DECLARE @sScreenCodes VARCHAR(MAX)" & vbCrLf &  _
"          SELECT @sScreenCodes = COALESCE(@sScreenCodes + ''',''', '')" & vbCrLf &  _
"                                 + RTRIM(GISS.CODE)" & vbCrLf &  _
"          FROM   GIS_SCREEN GISS" & vbCrLf &  _
"                 JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"                   ON GISS.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"          WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"          SET @sScreenCodes = '''' + @sScreenCodes + ''''" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'DECLARE @sScreenCodes VARCHAR(MAX) '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'DECLARE @sErrorMessage VARCHAR(MAX) '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'IF EXISTS(SELECT NULL FROM GIS_SCREEN GISS JOIN GIS_DATA_MODEL GISDM ON GISS.GIS_DATA_MODEL_ID =GISDM.GIS_DATA_MODEL_ID WHERE GISDM.CODE<>'''" & vbCrLf &  _
"                 + @sDataModelCode" & vbCrLf &  _
"                 + ''' AND RTRIM(GISS.CODE) IN ('" & vbCrLf &  _
"                 + @sScreenCodes + ')) ' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @sScreenCodes = COALESCE(@sScreenCodes + '','','''') + RTRIM(GISS.code) FROM GIS_SCREEN GISS JOIN GIS_DATA_MODEL GISDM ON GISS.GIS_DATA_MODEL_ID =GISDM.GIS_DATA_MODEL_ID WHERE GISDM.CODE<>'''" & vbCrLf &  _
"                 + @sDataModelCode" & vbCrLf &  _
"                 + ''' AND RTRIM(GISS.CODE) IN ('" & vbCrLf &  _
"                 + @sScreenCodes + ') ' + CHAR(13)" & vbCrLf &  _
"                 + 'SET @sErrorMessage = @sScreenCodes + '' Screen codes already exists, Please rename first and then import again.'' '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'RAISERROR (@sErrorMessage,16,1); '" & vbCrLf &  _
"                 + CHAR(13) + 'RETURN; ' + CHAR(13) + 'END '" & vbCrLf &  _
"      END" & vbCrLf &  _
"    DECLARE @CAPTUREOBJECTS INT" & vbCrLf &  _
"    SET @CAPTUREOBJECTS=1 --ALWAYS CAPTURE OBJECTS" & vbCrLf &  _
"    SET @sDataModelCode=RTRIM(LTRIM(@sDataModelCode))" & vbCrLf &  _
"    DECLARE @MAXCNT INT" & vbCrLf &  _
"    DECLARE @INDEX INT" & vbCrLf &  _
"    --**GET DETAILS OF ASSOCIATED UMLS***--" & vbCrLf &  _
"    CREATE TABLE #UML" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID      INT IDENTITY," & vbCrLf &  _
"         UMLID   INT," & vbCrLf &  _
"         UMLCODE VARCHAR(10)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    INSERT INTO #UML" & vbCrLf &  _
"    SELECT DISTINCT GISUDH.GIS_USER_DEF_HEADER_ID," & vbCrLf &  _
"                    GISUDH.CODE" & vbCrLf &  _
"    FROM   GIS_USER_DEF_HEADER GISUDH" & vbCrLf &  _
"           JOIN GIS_PROPERTY GISP" & vbCrLf &  _
"             ON GISUDH.GIS_USER_DEF_HEADER_ID = CONVERT(INT, GISP.SPECIALS_TYPE_REFERENCE)" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode           " & vbCrLf &  _
"           AND GISP.SPECIALS_TYPE = 6" & vbCrLf &  _
"           AND ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 'NULL') <> 'NULL'" & vbCrLf &  _
"    ORDER  BY GISUDH.GIS_USER_DEF_HEADER_ID ASC" & vbCrLf &  _
"    --**GET DETAILS OF ASSOCIATED UDLS***--" & vbCrLf &  _
"    CREATE TABLE #UDL" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID      INT IDENTITY," & vbCrLf &  _
"         UDLNAME VARCHAR(255)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    INSERT INTO #UDL" & vbCrLf &  _
"    SELECT DISTINCT GISP.SPECIALS_TYPE_REFERENCE" & vbCrLf &  _
"    FROM   GIS_PROPERTY GISP" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"           AND GISP.SPECIALS_TYPE = 2" & vbCrLf &  _
"           AND ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 'NULL') <> 'NULL'" & vbCrLf &  _
"    INSERT INTO #UDL" & vbCrLf &  _
"    SELECT DISTINCT REPLACE(GFM.ViewName,'UDL_','')" & vbCrLf &  _
"    FROM   GIS_FIND_MAPPING GFM" & vbCrLf &  _
"		   JOIN GIS_PROPERTY GISP" & vbCrLf &  _
"			 ON GISP.GIS_PROPERTY_ID = GFM.GIS_PROPERTY_ID" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISO.GIS_OBJECT_ID = GFM.GIS_OBJECT_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"    -------------**************** GET DETAILS OF OBJECTS ************------------------------" & vbCrLf &  _
"    CREATE TABLE #GISOBJECTS" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID                       INT IDENTITY," & vbCrLf &  _
"         OBJECT_NAME              VARCHAR(100)," & vbCrLf &  _
"         TABLE_NAME               VARCHAR(100)," & vbCrLf &  _
"         MAX_INSTANCES            VARCHAR (10)," & vbCrLf &  _
"         IS_QUOTE_OBJECT          VARCHAR (10)," & vbCrLf &  _
"         PARENT_TABLE_NAME        VARCHAR(100)," & vbCrLf &  _
"         POLARIS_OBJECT_ID        VARCHAR (10)," & vbCrLf &  _
"         IS_SELECTABLE_FOR_SCREEN VARCHAR (10)," & vbCrLf &  _
"         IS_NON_GIS               VARCHAR (10)," & vbCrLf &  _
"         EDIT_FLAGS               VARCHAR (10)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    INSERT INTO #GISOBJECTS" & vbCrLf &  _
"    SELECT GISO.OBJECT_NAME," & vbCrLf &  _
"           GISO.TABLE_NAME," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISO.MAX_INSTANCES), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISO.IS_QUOTE_OBJECT), 'NULL')," & vbCrLf &  _
"           GISPO.TABLE_NAME," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISO.POLARIS_OBJECT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISO.IS_SELECTABLE_FOR_SCREEN), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISO.IS_NON_GIS), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISO.EDIT_FLAGS), 'NULL')" & vbCrLf &  _
"    FROM   GIS_OBJECT GISO" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"           JOIN GIS_OBJECT GISPO" & vbCrLf &  _
"             ON GISO.PARENT_OBJECT_ID = GISPO.GIS_OBJECT_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"    ORDER  BY GISO.PARENT_OBJECT_ID ASC" & vbCrLf &  _
"    -------------************* GET DETAILS OF PROPERTIES ************-----------------------" & vbCrLf &  _
"    CREATE TABLE #GISPROPERTIES" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID                      INT IDENTITY," & vbCrLf &  _
"         GIS_OBJECT_TABLE_NAME   VARCHAR(100)," & vbCrLf &  _
"         PROPERTY_NAME           VARCHAR(100)," & vbCrLf &  _
"         COLUMN_NAME             VARCHAR(100)," & vbCrLf &  _
"         DATA_TYPE               VARCHAR(10),--INT," & vbCrLf &  _
"         IS_INPUT_PROPERTY       VARCHAR(10),-- INT," & vbCrLf &  _
"         IS_IDENTIFYING_PROPERTY VARCHAR(10),--INT," & vbCrLf &  _
"         IS_PRIMARY_KEY          VARCHAR(10),--INT," & vbCrLf &  _
"         POLARIS_PROPERTY_ID     VARCHAR(10),-- IT IS ALWAYS NULL" & vbCrLf &  _
"         IS_DELETED              VARCHAR(10),--INT," & vbCrLf &  _
"         IS_SEARCH_PROPERTY      VARCHAR(10),--INT," & vbCrLf &  _
"         INDEX_LINKING_ID        VARCHAR(10),--INT," & vbCrLf &  _
"         EDIT_FLAGS              VARCHAR(10),--INT," & vbCrLf &  _
"         SPECIALS_TYPE           VARCHAR(10),--INT," & vbCrLf &  _
"         SPECIALS_TYPE_REFERENCE VARCHAR(100)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    INSERT INTO #GISPROPERTIES" & vbCrLf &  _
"    SELECT GISO.TABLE_NAME," & vbCrLf &  _
"           GISP.PROPERTY_NAME," & vbCrLf &  _
"           GISP.COLUMN_NAME," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.DATA_TYPE), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.IS_INPUT_PROPERTY), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.IS_IDENTIFYING_PROPERTY), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.IS_PRIMARY_KEY), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.POLARIS_PROPERTY_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.IS_DELETED), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.IS_SEARCH_PROPERTY), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.INDEX_LINKING_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.EDIT_FLAGS), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISP.SPECIALS_TYPE), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(100), SPECIALS_TYPE_REFERENCE), 'NULL')" & vbCrLf &  _
"    FROM   GIS_PROPERTY GISP" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"    ORDER  BY GISO.GIS_OBJECT_ID ASC" & vbCrLf &  _
"    ------------*********** UPDATE PROPERTY TABLE BASED ON UML ***********------------------ " & vbCrLf &  _
"    --THIS WILL REPLACE THE ID OF UML IN UPDATE WITH CODE INSTEAD WHICH WILL BE LATER USED TO UPDATE " & vbCrLf &  _
"    UPDATE #GISPROPERTIES" & vbCrLf &  _
"    SET    SPECIALS_TYPE_REFERENCE = #UML.UMLCODE" & vbCrLf &  _
"    FROM   #GISPROPERTIES GISP" & vbCrLf &  _
"           JOIN #UML" & vbCrLf &  _
"             ON #UML.UMLID = CONVERT(INT, ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 0))" & vbCrLf &  _
"    WHERE  GISP.SPECIALS_TYPE = 6" & vbCrLf &  _
"           AND ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 'NULL') <> 'NULL'" & vbCrLf &  _
"    --------------************* GET DETAILS OF SCREENS ************------------------------" & vbCrLf &  _
"    CREATE TABLE #GISSCREEN" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID                   INT IDENTITY,         " & vbCrLf &  _
"         CODE                 VARCHAR(20)," & vbCrLf &  _
"         DESCRIPTION          VARCHAR(100)," & vbCrLf &  _
"         IS_DELETED           VARCHAR(10)," & vbCrLf &  _
"         EFFECTIVE_DATE       VARCHAR(50)," & vbCrLf &  _
"         PARENT_SCREEN_CODE   VARCHAR(20)," & vbCrLf &  _
"         IS_MAINTAINABLE      VARCHAR(10)," & vbCrLf &  _
"         GIS_DATA_MODEL_ID    VARCHAR(10)," & vbCrLf &  _
"         SCRIPT_DEFAULTS      VARCHAR(MAX)," & vbCrLf &  _
"         SCRIPT_DYNAMIC_LOGIC VARCHAR(MAX)," & vbCrLf &  _
"         SCREEN_TYPE          VARCHAR(10)," & vbCrLf &  _
"         SCREEN_HEIGHT        VARCHAR(10)," & vbCrLf &  _
"         SCREEN_WIDTH         VARCHAR(10)," & vbCrLf &  _
"         PRODUCT_OPTION       VARCHAR(10)" & vbCrLf &  _
"      )" & vbCrLf &  _
"    --'INSERT PARENT SCREENS FIRST" & vbCrLf &  _
"    INSERT INTO #GISSCREEN" & vbCrLf &  _
"    SELECT GISS.CODE," & vbCrLf &  _
"           GISS.DESCRIPTION," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.IS_DELETED), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(50), GISS.EFFECTIVE_DATE), 'NULL')," & vbCrLf &  _
"           'NULL',--NO PARENT CODE HERE" & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.IS_MAINTAINABLE), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.GIS_DATA_MODEL_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(GISS.SCRIPT_DEFAULTS, 'NULL')," & vbCrLf &  _
"           ISNULL(GISS.SCRIPT_DYNAMIC_LOGIC, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_TYPE), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_HEIGHT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.PRODUCT_OPTION), 'NULL')" & vbCrLf &  _
"    FROM   GIS_SCREEN GISS" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISS.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"           AND GISS.PARENT_ID IS NULL" & vbCrLf &  _
"    ORDER  BY GISS.GIS_SCREEN_ID ASC" & vbCrLf &  _
"    --'INSERT CHILD SCREENS " & vbCrLf &  _
"    INSERT INTO #GISSCREEN" & vbCrLf &  _
"    SELECT GISS.CODE," & vbCrLf &  _
"           GISS.DESCRIPTION," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.IS_DELETED), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(50), GISS.EFFECTIVE_DATE), 'NULL')," & vbCrLf &  _
"           GISSP.CODE," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.IS_MAINTAINABLE), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.GIS_DATA_MODEL_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(GISS.SCRIPT_DEFAULTS, 'NULL')," & vbCrLf &  _
"           ISNULL(GISS.SCRIPT_DYNAMIC_LOGIC, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_TYPE), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_HEIGHT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISS.PRODUCT_OPTION), 'NULL')" & vbCrLf &  _
"    FROM   GIS_SCREEN GISS" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISS.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID" & vbCrLf &  _
"           JOIN GIS_SCREEN GISSP" & vbCrLf &  _
"             ON GISS.PARENT_ID = GISSP.GIS_SCREEN_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"    ORDER  BY GISS.GIS_SCREEN_ID ASC" & vbCrLf &  _
"    UPDATE #GISSCREEN" & vbCrLf &  _
"    SET    SCRIPT_DEFAULTS = REPLACE(SCRIPT_DEFAULTS, '''', '^')" & vbCrLf &  _
"    WHERE  SCRIPT_DEFAULTS <> 'NULL'" & vbCrLf &  _
"    UPDATE #GISSCREEN" & vbCrLf &  _
"    SET    SCRIPT_DYNAMIC_LOGIC = REPLACE(SCRIPT_DYNAMIC_LOGIC, '''', '^')" & vbCrLf &  _
"    WHERE  SCRIPT_DYNAMIC_LOGIC <> 'NULL'" & vbCrLf &  _
"    -------------******** GET DETAILS OF SCREEN DETAILS *****------------------------------" & vbCrLf &  _
"    CREATE TABLE #GISSCREENDETAIL" & vbCrLf &  _
"      (" & vbCrLf &  _
"         ID                       INT IDENTITY," & vbCrLf &  _
"         GIS_SCREEN_CODE          VARCHAR(10)," & vbCrLf &  _
"         SCREEN_DETAIL_CNT        VARCHAR(10)," & vbCrLf &  _
"         GIS_OBJECT_TABLE_NAME    VARCHAR(200)," & vbCrLf &  _
"         GIS_PROPERTY_COLUMN_NAME VARCHAR(200)," & vbCrLf &  _
"         IS_FRAME                 VARCHAR(10)," & vbCrLf &  _
"         TAB_NUMBER               VARCHAR(10)," & vbCrLf &  _
"         CAPTION                  VARCHAR(255)," & vbCrLf &  _
"         ITEM_TOP                 VARCHAR(10)," & vbCrLf &  _
"         ITEM_LEFT                VARCHAR(10)," & vbCrLf &  _
"         ITEM_HEIGHT              VARCHAR(10)," & vbCrLf &  _
"         ITEM_WIDTH               VARCHAR(10)," & vbCrLf &  _
"         COLUMN_WIDTH             VARCHAR(10)," & vbCrLf &  _
"         PRE_QUOTE_REQUIREMENT    VARCHAR(10)," & vbCrLf &  _
"         POST_QUOTE_REQUIREMENT   VARCHAR(10)," & vbCrLf &  _
"         PURCHASE_REQUIREMENT     VARCHAR(10)," & vbCrLf &  _
"         PARENT_ID                VARCHAR(10)," & vbCrLf &  _
"         HELP_TEXT                VARCHAR(255)," & vbCrLf &  _
"         DEFAULT_OBJECT_ID        VARCHAR(10)," & vbCrLf &  _
"         DEFAULT_PROPERTY_ID      VARCHAR(10)," & vbCrLf &  _
"         IS_VALUATION             VARCHAR(10)," & vbCrLf &  _
"         IS_RATE_AND_PREMIUM      VARCHAR(10)," & vbCrLf &  _
"         CHILD_SCREEN_ID          VARCHAR(10)," & vbCrLf &  _
"         PMFORMAT                 VARCHAR(10)," & vbCrLf &  _
"         COLUMN_POSITION          VARCHAR(10)," & vbCrLf &  _
"         TAB_SET_INDEX            VARCHAR(10)," & vbCrLf &  _
"         DATA_MODEL_TYPE          VARCHAR(10)," & vbCrLf &  _
"         INSERT_TYPE              VARCHAR(10)-- 0 = PROPERTY NULL & OBJECT NULL 1= ONLY PROPERTY NULL 2 = NEITHER NULL" & vbCrLf &  _
"      )" & vbCrLf &  _
"    -- INSERT DETIALS WITH PROPERTY NULL & OBJECT NULL" & vbCrLf &  _
"    INSERT INTO #GISSCREENDETAIL" & vbCrLf &  _
"    SELECT GISS.CODE," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.SCREEN_DETAIL_CNT), 'NULL')," & vbCrLf &  _
"           'NULL'," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.GIS_PROPERTY_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_FRAME), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_NUMBER), 'NULL')," & vbCrLf &  _
"           ISNULL(GISSD.CAPTION, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_TOP), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_LEFT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_HEIGHT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PRE_QUOTE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.POST_QUOTE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PURCHASE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PARENT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(GISSD.HELP_TEXT, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_OBJECT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_PROPERTY_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_VALUATION), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_RATE_AND_PREMIUM), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.CHILD_SCREEN_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PMFORMAT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_POSITION), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_SET_INDEX), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DATA_MODEL_TYPE), 'NULL')," & vbCrLf &  _
"           0" & vbCrLf &  _
"    FROM   GIS_SCREEN_DETAIL GISSD" & vbCrLf &  _
"           JOIN GIS_SCREEN GISS" & vbCrLf &  _
"             ON GISSD.GIS_SCREEN_ID = GISS.GIS_SCREEN_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISDM.GIS_DATA_MODEL_ID = GISS.GIS_DATA_MODEL_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"           AND ISNULL(GISSD.GIS_PROPERTY_ID, 0) < 1" & vbCrLf &  _
"           AND GISSD.GIS_OBJECT_ID IS NULL" & vbCrLf &  _
"    ORDER  BY GISSD.GIS_SCREEN_ID ASC" & vbCrLf &  _
"    -- INSERT DETIALS WITH ONLY PROPERTY NULL" & vbCrLf &  _
"    INSERT INTO #GISSCREENDETAIL" & vbCrLf &  _
"    SELECT GISS.CODE," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.SCREEN_DETAIL_CNT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(200), GISO.TABLE_NAME), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.GIS_PROPERTY_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_FRAME), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_NUMBER), 'NULL')," & vbCrLf &  _
"           ISNULL(GISSD.CAPTION, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_TOP), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_LEFT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_HEIGHT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PRE_QUOTE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.POST_QUOTE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PURCHASE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PARENT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(GISSD.HELP_TEXT, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_OBJECT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_PROPERTY_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_VALUATION), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_RATE_AND_PREMIUM), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.CHILD_SCREEN_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PMFORMAT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_POSITION), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_SET_INDEX), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DATA_MODEL_TYPE), 'NULL')," & vbCrLf &  _
"           1" & vbCrLf &  _
"    FROM   GIS_SCREEN_DETAIL GISSD" & vbCrLf &  _
"           JOIN GIS_SCREEN GISS" & vbCrLf &  _
"             ON GISSD.GIS_SCREEN_ID = GISS.GIS_SCREEN_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISDM.GIS_DATA_MODEL_ID = GISS.GIS_DATA_MODEL_ID" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISO.GIS_OBJECT_ID = GISSD.GIS_OBJECT_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"           AND ISNULL(GISSD.GIS_PROPERTY_ID, 0) < 1" & vbCrLf &  _
"    ORDER  BY GISSD.GIS_SCREEN_ID ASC" & vbCrLf &  _
"    -- INSERT DETIALS WITH  2 = NEITHER NULL" & vbCrLf &  _
"    INSERT INTO #GISSCREENDETAIL" & vbCrLf &  _
"    SELECT GISS.CODE," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.SCREEN_DETAIL_CNT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(200), GISO.TABLE_NAME), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(200), GISP.COLUMN_NAME), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_FRAME), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_NUMBER), 'NULL')," & vbCrLf &  _
"           ISNULL(GISSD.CAPTION, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_TOP), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_LEFT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_HEIGHT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_WIDTH), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PRE_QUOTE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.POST_QUOTE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PURCHASE_REQUIREMENT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PARENT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(GISSD.HELP_TEXT, 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_OBJECT_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_PROPERTY_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_VALUATION), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_RATE_AND_PREMIUM), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.CHILD_SCREEN_ID), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.PMFORMAT), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_POSITION), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_SET_INDEX), 'NULL')," & vbCrLf &  _
"           ISNULL(CONVERT(VARCHAR(10), GISSD.DATA_MODEL_TYPE), 'NULL')," & vbCrLf &  _
"           2" & vbCrLf &  _
"    FROM   GIS_SCREEN_DETAIL GISSD" & vbCrLf &  _
"           JOIN GIS_SCREEN GISS" & vbCrLf &  _
"             ON GISSD.GIS_SCREEN_ID = GISS.GIS_SCREEN_ID" & vbCrLf &  _
"           JOIN GIS_DATA_MODEL GISDM" & vbCrLf &  _
"             ON GISDM.GIS_DATA_MODEL_ID = GISS.GIS_DATA_MODEL_ID" & vbCrLf &  _
"           JOIN GIS_OBJECT GISO" & vbCrLf &  _
"             ON GISO.GIS_OBJECT_ID = GISSD.GIS_OBJECT_ID" & vbCrLf &  _
"           JOIN GIS_PROPERTY GISP" & vbCrLf &  _
"             ON GISP.GIS_PROPERTY_ID = GISSD.GIS_PROPERTY_ID" & vbCrLf &  _
"    WHERE  GISDM.CODE = @sDataModelCode" & vbCrLf &  _
"    ORDER  BY GISSD.GIS_SCREEN_ID ASC" & vbCrLf &  _
"    -------------************************************************------------------------" & vbCrLf &  _
"    IF @CAPTUREOBJECTS <> 0" & vbCrLf &  _
"      BEGIN --A" & vbCrLf &  _
"          --**********NOW MAKE QUERY*****************	" & vbCrLf &  _
"          SELECT @INDEX = 1," & vbCrLf &  _
"                 @MAXCNT = MAX(ID)" & vbCrLf &  _
"          FROM   #GISOBJECTS" & vbCrLf &  _
"          IF @MAXCNT > 0" & vbCrLf &  _
"            BEGIN --B" & vbCrLf &  _
"                DECLARE @ADDDATAMODELSQL VARCHAR(MAX)" & vbCrLf &  _
"                EXECUTE spu_CreatePolicyBinder" & vbCrLf &  _
"                  @sDataModelCode," & vbCrLf &  _
"                  @ADDDATAMODELSQL OUTPUT" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'DECLARE @DATAMODELID INT ' + CHAR(13)" & vbCrLf &  _
"                       + 'DECLARE @OBJECTCNT INT ' + CHAR(13)" & vbCrLf &  _
"                       + 'DECLARE @PARENTOBJECTID INT ' + CHAR(13)" & vbCrLf &  _
"                       + 'DECLARE @DMCAPTIONID INT ' + CHAR(13)" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'IF NOT EXISTS(SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''" & vbCrLf &  _
"                       + @sDataModelCode + ''') ' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'SELECT @DATAMODELID = MAX(GIS_DATA_MODEL_ID)+1 FROM GIS_DATA_MODEL '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"					   + RTRIM(LTRIM(@sDataModelCode))" & vbCrLf &  _
"					   + ''', @DMCAPTIONID OUTPUT ' + CHAR(13)" & vbCrLf &  _
"                       + 'INSERT INTO GIS_DATA_MODEL (GIS_DATA_MODEL_ID," & vbCrLf &  _
"						CODE," & vbCrLf &  _
"						CAPTION_ID," & vbCrLf &  _
"						DESCRIPTION," & vbCrLf &  _
"						IS_DELETED," & vbCrLf &  _
"						EFFECTIVE_DATE," & vbCrLf &  _
"						GIS_DATA_MODEL_TYPE_ID," & vbCrLf &  _
"						PRODUCT_OPTION)	VALUES(@DATAMODELID,'''" & vbCrLf &  _
"                       + RTRIM(LTRIM(ISNULL(CONVERT(VARCHAR(50), CODE), 'NULL')))" & vbCrLf &  _
"                       + ''',@DMCAPTIONID,'''" & vbCrLf &  _
"                       + RTRIM(LTRIM(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL')))" & vbCrLf &  _
"                       + ''','" & vbCrLf &  _
"                       + ISNULL(CONVERT(VARCHAR(50), IS_DELETED), 'NULL')" & vbCrLf &  _
"                       + ','''" & vbCrLf &  _
"                       + ISNULL(CONVERT(VARCHAR(50), EFFECTIVE_DATE), 'NULL')" & vbCrLf &  _
"                       + ''','" & vbCrLf &  _
"                       + ISNULL(CONVERT(VARCHAR(50), GIS_DATA_MODEL_TYPE_ID), 'NULL')" & vbCrLf &  _
"                       + ','" & vbCrLf &  _
"                       + ISNULL(CONVERT(VARCHAR(50), PRODUCT_OPTION), 'NULL')" & vbCrLf &  _
"                       + ') ' + CHAR(13) + @ADDDATAMODELSQL + CHAR(13)" & vbCrLf &  _
"                       + ' END ' + CHAR(13)" & vbCrLf &  _
"                FROM   GIS_DATA_MODEL" & vbCrLf &  _
"                WHERE  CODE = @sDataModelCode" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'SELECT @DATAMODELID = GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WITH(NOLOCK) WHERE CODE = '''" & vbCrLf &  _
"                       + @sDataModelCode + ''' ' + CHAR(13)" & vbCrLf &  _
"                       + 'SELECT @OBJECTCNT = MAX(GIS_OBJECT_ID) FROM GIS_OBJECT WITH(NOLOCK) '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                WHILE( @INDEX <= @MAXCNT )" & vbCrLf &  _
"                  BEGIN --C		" & vbCrLf &  _
"                      DECLARE @TABLENAME VARCHAR(100)" & vbCrLf &  _
"                      DECLARE @ADDOBJECTSQL VARCHAR(MAX)" & vbCrLf &  _
"                      SELECT @TABLENAME = TABLE_NAME" & vbCrLf &  _
"                      FROM   #GISOBJECTS" & vbCrLf &  _
"                      WHERE  ID = @INDEX" & vbCrLf &  _
"                      EXECUTE spu_CreateGISObject" & vbCrLf &  _
"                        @TABLENAME," & vbCrLf &  _
"                        @ADDOBJECTSQL OUTPUT" & vbCrLf &  _
"                      INSERT INTO #QUERIES" & vbCrLf &  _
"                      SELECT 'IF (SELECT GIS_OBJECT_ID FROM GIS_OBJECT WITH(NOLOCK) WHERE TABLE_NAME = '''" & vbCrLf &  _
"                             + TABLE_NAME + ''') IS NULL ' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"                             + CHAR(13)" & vbCrLf &  _
"                             + 'SET @OBJECTCNT = @OBJECTCNT + 1 '" & vbCrLf &  _
"                             + CHAR(13)" & vbCrLf &  _
"                             + 'SELECT TOP 1 @PARENTOBJECTID =GIS_OBJECT_ID FROM GIS_OBJECT WITH (NOLOCK) WHERE TABLE_NAME = '''" & vbCrLf &  _
"                             + PARENT_TABLE_NAME + ''' ' + CHAR(13)" & vbCrLf &  _
"                             + 'INSERT INTO GIS_OBJECT (GIS_OBJECT_ID," & vbCrLf &  _
"								GIS_DATA_MODEL_ID," & vbCrLf &  _
"								OBJECT_NAME," & vbCrLf &  _
"								TABLE_NAME," & vbCrLf &  _
"								MAX_INSTANCES," & vbCrLf &  _
"								IS_QUOTE_OBJECT," & vbCrLf &  _
"								PARENT_OBJECT_ID," & vbCrLf &  _
"								POLARIS_OBJECT_ID," & vbCrLf &  _
"								IS_SELECTABLE_FOR_SCREEN," & vbCrLf &  _
"								IS_NON_GIS," & vbCrLf &  _
"								EDIT_FLAGS) ' + CHAR(13)" & vbCrLf &  _
"                             + 'VALUES(@OBJECTCNT,@DATAMODELID,'''" & vbCrLf &  _
"                             + OBJECT_NAME + ''',''' + TABLE_NAME + ''','" & vbCrLf &  _
"                             + MAX_INSTANCES + ',' + IS_QUOTE_OBJECT" & vbCrLf &  _
"                             + ',@PARENTOBJECTID,' + 'NULL,'" & vbCrLf &  _
"                             + IS_SELECTABLE_FOR_SCREEN + ',' + IS_NON_GIS + ','" & vbCrLf &  _
"                             + EDIT_FLAGS + ') ' + CHAR(13) + @ADDOBJECTSQL" & vbCrLf &  _
"                             + CHAR(13) + ' END ' + CHAR(13) + CHAR(13)" & vbCrLf &  _
"                      FROM   #GISOBJECTS" & vbCrLf &  _
"                      WHERE  ID = @INDEX" & vbCrLf &  _
"                      SET @INDEX=@INDEX + 1" & vbCrLf &  _
"                  END --C" & vbCrLf &  _
"            END --B" & vbCrLf &  _
"      END--A	" & vbCrLf &  _
"    SELECT @INDEX = 1," & vbCrLf &  _
"           @MAXCNT = MAX(ID)" & vbCrLf &  _
"    FROM   #GISPROPERTIES" & vbCrLf &  _
"    IF @MAXCNT > 0" & vbCrLf &  _
"      BEGIN --A2" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT ' '" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '--******************** THIS BIT OF SCRIPT WILL INSERT DATA OF PROPERTIES FOR DATAMODEL ( '" & vbCrLf &  _
"                 + @sDataModelCode" & vbCrLf &  _
"                 + ') *******************'" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'DECLARE @PROPERTYCNT INT ' + CHAR(13)" & vbCrLf &  _
"                 + 'DECLARE @OBJECTID INT ' + CHAR(13)" & vbCrLf &  _
"                 + 'SELECT @PROPERTYCNT = MAX(GIS_PROPERTY_ID) FROM GIS_PROPERTY WITH(NOLOCK) '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"          WHILE( @INDEX <= @MAXCNT )" & vbCrLf &  _
"            BEGIN --B2" & vbCrLf &  _
"                DECLARE @PROPCOLNAME VARCHAR(100)" & vbCrLf &  _
"                DECLARE @PROPTABLENAME VARCHAR(100)" & vbCrLf &  _
"                DECLARE @ADDPROPERTYSQL VARCHAR(300)" & vbCrLf &  _
"                SELECT @PROPTABLENAME = GIS_OBJECT_TABLE_NAME," & vbCrLf &  _
"                       @PROPCOLNAME = COLUMN_NAME" & vbCrLf &  _
"                FROM   #GISPROPERTIES" & vbCrLf &  _
"                WHERE  ID = @INDEX" & vbCrLf &  _
"                EXECUTE spu_CreateGISProperty" & vbCrLf &  _
"                  @PROPTABLENAME," & vbCrLf &  _
"                  @PROPCOLNAME," & vbCrLf &  _
"                  @ADDPROPERTYSQL OUTPUT" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'SELECT TOP 1 @OBJECTID =GIS_OBJECT_ID FROM GIS_OBJECT WITH (NOLOCK) WHERE TABLE_NAME = '''" & vbCrLf &  _
"                       + GIS_OBJECT_TABLE_NAME + ''' ' + CHAR(13)" & vbCrLf &  _
"                       + 'IF (SELECT GIS_PROPERTY_ID FROM GIS_PROPERTY WITH (NOLOCK) WHERE COLUMN_NAME = '''" & vbCrLf &  _
"                       + COLUMN_NAME" & vbCrLf &  _
"                       + ''' AND GIS_OBJECT_ID=@OBJECTID) IS NULL '" & vbCrLf &  _
"                       + CHAR(13) + 'BEGIN ' + CHAR(13)" & vbCrLf &  _
"                       + 'SET @PROPERTYCNT = @PROPERTYCNT + 1 '" & vbCrLf &  _
"                       + CHAR(13) + 'INSERT INTO GIS_PROPERTY (GIS_PROPERTY_ID,GIS_OBJECT_ID," & vbCrLf &  _
"						PROPERTY_NAME," & vbCrLf &  _
"						COLUMN_NAME," & vbCrLf &  _
"						DATA_TYPE," & vbCrLf &  _
"						IS_INPUT_PROPERTY," & vbCrLf &  _
"						IS_IDENTIFYING_PROPERTY," & vbCrLf &  _
"						IS_PRIMARY_KEY," & vbCrLf &  _
"						POLARIS_PROPERTY_ID," & vbCrLf &  _
"						IS_DELETED," & vbCrLf &  _
"						IS_SEARCH_PROPERTY," & vbCrLf &  _
"						INDEX_LINKING_ID," & vbCrLf &  _
"						EDIT_FLAGS," & vbCrLf &  _
"						SPECIALS_TYPE," & vbCrLf &  _
"						SPECIALS_TYPE_REFERENCE) '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'VALUES(@PROPERTYCNT,@OBJECTID,'''" & vbCrLf &  _
"                       + PROPERTY_NAME + ''',''' + COLUMN_NAME + ''','" & vbCrLf &  _
"                       + DATA_TYPE + ',' + IS_INPUT_PROPERTY + ','" & vbCrLf &  _
"                       + IS_IDENTIFYING_PROPERTY + ',' + IS_PRIMARY_KEY" & vbCrLf &  _
"                       + ',' + POLARIS_PROPERTY_ID + ',' + IS_DELETED + ','" & vbCrLf &  _
"                       + IS_SEARCH_PROPERTY + ',' + INDEX_LINKING_ID + ','" & vbCrLf &  _
"                       + EDIT_FLAGS + ',' + SPECIALS_TYPE + ','''" & vbCrLf &  _
"                       + SPECIALS_TYPE_REFERENCE + ''')' + ' END '" & vbCrLf &  _
"                       + CHAR(13) + CHAR(13)" & vbCrLf &  _
"                FROM   #GISPROPERTIES" & vbCrLf &  _
"                WHERE  ID = @INDEX" & vbCrLf &  _
"                SET @INDEX=@INDEX + 1" & vbCrLf &  _
"            END --B2" & vbCrLf &  _
"      END--A2" & vbCrLf &  _
"    -----------******************************************************-----------------------" & vbCrLf &  _
"    IF @CAPTURESCREENS <> 0" & vbCrLf &  _
"      BEGIN --A3	" & vbCrLf &  _
"          --SCREEN INFORMATION" & vbCrLf &  _
"          SELECT @INDEX = 1," & vbCrLf &  _
"                 @MAXCNT = MAX(ID)" & vbCrLf &  _
"          FROM   #GISSCREEN" & vbCrLf &  _
"          IF @MAXCNT > 0" & vbCrLf &  _
"            BEGIN --B3" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT ' '" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT '--*********************************** THIS BIT OF SCRIPT WILL INSERT DATA IN GIS SCREEN IF NEEDED FOR DATAMODEL ( '" & vbCrLf &  _
"                       + @sDataModelCode" & vbCrLf &  _
"                       + ') ***********************************'" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'DECLARE @GISSSCREENCNT INT ' + CHAR(13)" & vbCrLf &  _
"                       + 'DECLARE @GISSCAPTIONID INT ' + CHAR(13)" & vbCrLf &  _
"                       + 'DECLARE @GISSPARENTID INT ' + CHAR(13)" & vbCrLf &  _
"                       + 'SELECT @GISSSCREENCNT = MAX(GIS_SCREEN_ID) FROM GIS_SCREEN '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                BEGIN --B3A" & vbCrLf &  _
"                    WHILE ( @INDEX <= @MAXCNT )" & vbCrLf &  _
"                      BEGIN --B3B" & vbCrLf &  _
"                          INSERT INTO #QUERIES" & vbCrLf &  _
"                          SELECT 'IF (SELECT GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''" & vbCrLf &  _
"                                 + CODE + ''') IS NULL ' + CHAR(13) + 'BEGIN '" & vbCrLf &  _
"                                 + CHAR(13)" & vbCrLf &  _
"                                 + 'SELECT  @GISSPARENTID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE ='''" & vbCrLf &  _
"                                 + PARENT_SCREEN_CODE + ''' ' + CHAR(13)" & vbCrLf &  _
"                                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''" & vbCrLf &  _
"                                 + DESCRIPTION + ''', @GISSCAPTIONID OUTPUT '" & vbCrLf &  _
"                                 + CHAR(13)" & vbCrLf &  _
"                                 + 'SET @GISSSCREENCNT =@GISSSCREENCNT + 1 '" & vbCrLf &  _
"                                 + CHAR(13) + 'INSERT INTO GIS_SCREEN (GIS_SCREEN_ID,CAPTION_ID," & vbCrLf &  _
"									CODE," & vbCrLf &  _
"									DESCRIPTION," & vbCrLf &  _
"									IS_DELETED," & vbCrLf &  _
"									EFFECTIVE_DATE," & vbCrLf &  _
"									PARENT_ID," & vbCrLf &  _
"									IS_MAINTAINABLE," & vbCrLf &  _
"									GIS_DATA_MODEL_ID," & vbCrLf &  _
"									SCRIPT_DEFAULTS," & vbCrLf &  _
"									SCRIPT_DYNAMIC_LOGIC," & vbCrLf &  _
"									SCREEN_TYPE," & vbCrLf &  _
"									SCREEN_HEIGHT," & vbCrLf &  _
"									SCREEN_WIDTH," & vbCrLf &  _
"									PRODUCT_OPTION) ' + CHAR(13)" & vbCrLf &  _
"                                 + 'VALUES(@GISSSCREENCNT,@GISSCAPTIONID,'''" & vbCrLf &  _
"                                 + CODE + ''',''' + DESCRIPTION + ''','" & vbCrLf &  _
"                                 + IS_DELETED + ',''' + EFFECTIVE_DATE + ''','" & vbCrLf &  _
"                                 + '@GISSPARENTID,' + IS_MAINTAINABLE + ','" & vbCrLf &  _
"                                 + '@DATAMODELID,'''" & vbCrLf &  _
"                                 + CONVERT(VARCHAR(MAX), SCRIPT_DEFAULTS)" & vbCrLf &  _
"                                 + ''','''" & vbCrLf &  _
"                                 + CONVERT(VARCHAR(MAX), SCRIPT_DYNAMIC_LOGIC)" & vbCrLf &  _
"                                 + ''',' + SCREEN_TYPE + ',' + SCREEN_HEIGHT + ','" & vbCrLf &  _
"                                 + SCREEN_WIDTH + ',' + PRODUCT_OPTION + ') '" & vbCrLf &  _
"                                 + CHAR(13) + ' END ' + CHAR(13) + CHAR(13)" & vbCrLf &  _
"                          FROM   #GISSCREEN" & vbCrLf &  _
"                          WHERE  ID = @INDEX" & vbCrLf &  _
"                          SET @INDEX = @INDEX + 1" & vbCrLf &  _
"                      END --B3B" & vbCrLf &  _
"                END --B3A" & vbCrLf &  _
"            END --B3" & vbCrLf &  _
"          --SCREEN DETAIL INFORMATION	" & vbCrLf &  _
"          SELECT @INDEX = 1," & vbCrLf &  _
"                 @MAXCNT = MAX(ID)" & vbCrLf &  _
"          FROM   #GISSCREENDETAIL" & vbCrLf &  _
"          IF @MAXCNT > 0" & vbCrLf &  _
"            BEGIN --C3" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT ' '" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT '--*********************************** THIS WILL INSERT DATA IN GIS SCREEN DETAIL IF NEEDED FOR DATAMODEL ( '" & vbCrLf &  _
"                       + @sDataModelCode" & vbCrLf &  _
"                       + ') ***********************************'" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'DELETE FROM GIS_SCREEN_DETAIL 	FROM GIS_SCREEN_DETAIL GISSD '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'JOIN GIS_SCREEN GISS ON GISSD.GIS_SCREEN_ID=GISS.GIS_SCREEN_ID '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'JOIN GIS_DATA_MODEL  GISDM ON GISDM.GIS_DATA_MODEL_ID= GISS.GIS_DATA_MODEL_ID '" & vbCrLf &  _
"                       + CHAR(13) + 'WHERE GISDM.CODE='''" & vbCrLf &  _
"                       + @sDataModelCode + ''''" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT ' DECLARE @GISSDSCREENID INT ' + CHAR(13)" & vbCrLf &  _
"                       + ' DECLARE @GISSDOBJECTID INT ' + CHAR(13)" & vbCrLf &  _
"                       + ' DECLARE @GISSDPROPERTYID INT ' + CHAR(13)" & vbCrLf &  _
"                       + ' DECLARE @CHILDSCREENID INT '" & vbCrLf &  _
"                BEGIN --C3A	" & vbCrLf &  _
"                    WHILE ( @INDEX <= @MAXCNT )" & vbCrLf &  _
"                      BEGIN --C3B" & vbCrLf &  _
"                          DECLARE @GIS_SCREEN_CODE AS VARCHAR(10)" & vbCrLf &  _
"                          SELECT @GIS_SCREEN_CODE = CODE" & vbCrLf &  _
"                          FROM   GIS_SCREEN" & vbCrLf &  _
"                          WHERE  GIS_SCREEN_ID IN(SELECT CONVERT(INT, CHILD_SCREEN_ID)" & vbCrLf &  _
"                                                  FROM   #GISSCREENDETAIL" & vbCrLf &  _
"                                                  WHERE  ID = @INDEX" & vbCrLf &  _
"                                                         AND CHILD_SCREEN_ID <> 'NULL')" & vbCrLf &  _
"                          INSERT INTO #QUERIES" & vbCrLf &  _
"                          SELECT 'SELECT @CHILDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''" & vbCrLf &  _
"                                 + @GIS_SCREEN_CODE + '''' + CHAR(13)" & vbCrLf &  _
"                          DECLARE @INSERTTYPE INT" & vbCrLf &  _
"                          SELECT @INSERTTYPE = INSERT_TYPE" & vbCrLf &  _
"                          FROM   #GISSCREENDETAIL" & vbCrLf &  _
"                          WHERE  ID = @INDEX" & vbCrLf &  _
"                          IF @INSERTTYPE = 0" & vbCrLf &  _
"                            BEGIN --C3BA" & vbCrLf &  _
"                                INSERT INTO #QUERIES" & vbCrLf &  _
"                                SELECT 'SELECT @GISSDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''" & vbCrLf &  _
"                                       + GIS_SCREEN_CODE" & vbCrLf &  _
"                                       + ''' AND GIS_DATA_MODEL_ID = (SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''" & vbCrLf &  _
"                                       + @sDataModelCode + ''') ' + CHAR(13)" & vbCrLf &  _
"                                       + 'INSERT INTO GIS_SCREEN_DETAIL (GIS_SCREEN_ID," & vbCrLf &  _
"										SCREEN_DETAIL_CNT," & vbCrLf &  _
"										GIS_OBJECT_ID," & vbCrLf &  _
"										GIS_PROPERTY_ID," & vbCrLf &  _
"										IS_FRAME," & vbCrLf &  _
"										TAB_NUMBER," & vbCrLf &  _
"										CAPTION," & vbCrLf &  _
"										ITEM_TOP," & vbCrLf &  _
"										ITEM_LEFT," & vbCrLf &  _
"										ITEM_HEIGHT," & vbCrLf &  _
"										ITEM_WIDTH," & vbCrLf &  _
"										COLUMN_WIDTH," & vbCrLf &  _
"										PRE_QUOTE_REQUIREMENT," & vbCrLf &  _
"										POST_QUOTE_REQUIREMENT," & vbCrLf &  _
"										PURCHASE_REQUIREMENT," & vbCrLf &  _
"										PARENT_ID," & vbCrLf &  _
"										HELP_TEXT," & vbCrLf &  _
"										DEFAULT_OBJECT_ID," & vbCrLf &  _
"										DEFAULT_PROPERTY_ID," & vbCrLf &  _
"										IS_VALUATION," & vbCrLf &  _
"										IS_RATE_AND_PREMIUM," & vbCrLf &  _
"										CHILD_SCREEN_ID," & vbCrLf &  _
"										PMFORMAT," & vbCrLf &  _
"										COLUMN_POSITION," & vbCrLf &  _
"										TAB_SET_INDEX," & vbCrLf &  _
"										DATA_MODEL_TYPE) VALUES(@GISSDSCREENID,'" & vbCrLf &  _
"                                       + SCREEN_DETAIL_CNT + ',NULL,'" & vbCrLf &  _
"                                       + GIS_PROPERTY_COLUMN_NAME + ',' + IS_FRAME + ','" & vbCrLf &  _
"                                       + TAB_NUMBER + ',''' + REPLACE(CAPTION, '''', '^')" & vbCrLf &  _
"                                       + ''',' + ITEM_TOP + ',' + ITEM_LEFT + ',' + ITEM_HEIGHT" & vbCrLf &  _
"                                       + ',' + ITEM_WIDTH + ',' + COLUMN_WIDTH + ','" & vbCrLf &  _
"                                       + PRE_QUOTE_REQUIREMENT + ','" & vbCrLf &  _
"                                       + POST_QUOTE_REQUIREMENT + ','" & vbCrLf &  _
"                                       + PURCHASE_REQUIREMENT + ',' + PARENT_ID + ','''" & vbCrLf &  _
"                                       + HELP_TEXT + ''',' + DEFAULT_OBJECT_ID + ','" & vbCrLf &  _
"                                       + DEFAULT_PROPERTY_ID + ',' + IS_VALUATION + ','" & vbCrLf &  _
"                                       + IS_RATE_AND_PREMIUM + ',@CHILDSCREENID,'" & vbCrLf &  _
"                                       + PMFORMAT + ',' + COLUMN_POSITION + ','" & vbCrLf &  _
"                                       + TAB_SET_INDEX + ',' + DATA_MODEL_TYPE + ') '" & vbCrLf &  _
"                                       + CHAR(13)" & vbCrLf &  _
"                                FROM   #GISSCREENDETAIL" & vbCrLf &  _
"                                WHERE  ID = @INDEX" & vbCrLf &  _
"                            END --C3BA" & vbCrLf &  _
"                          IF @INSERTTYPE = 1" & vbCrLf &  _
"                            BEGIN --C3BB" & vbCrLf &  _
"                                INSERT INTO #QUERIES" & vbCrLf &  _
"                                SELECT 'SELECT TOP 1 @GISSDOBJECTID = GIS_OBJECT_ID FROM  GIS_OBJECT WHERE TABLE_NAME = '''" & vbCrLf &  _
"                                       + GIS_OBJECT_TABLE_NAME + ''' ' + CHAR(13)" & vbCrLf &  _
"                                       + 'SELECT @GISSDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''" & vbCrLf &  _
"                                       + GIS_SCREEN_CODE" & vbCrLf &  _
"                                       + '''  AND GIS_DATA_MODEL_ID = (SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''" & vbCrLf &  _
"                                       + @sDataModelCode + ''') ' + CHAR(13)" & vbCrLf &  _
"                                       + 'INSERT INTO GIS_SCREEN_DETAIL (GIS_SCREEN_ID," & vbCrLf &  _
"										SCREEN_DETAIL_CNT," & vbCrLf &  _
"										GIS_OBJECT_ID," & vbCrLf &  _
"										GIS_PROPERTY_ID," & vbCrLf &  _
"										IS_FRAME," & vbCrLf &  _
"										TAB_NUMBER," & vbCrLf &  _
"										CAPTION," & vbCrLf &  _
"										ITEM_TOP," & vbCrLf &  _
"										ITEM_LEFT," & vbCrLf &  _
"										ITEM_HEIGHT," & vbCrLf &  _
"										ITEM_WIDTH," & vbCrLf &  _
"										COLUMN_WIDTH," & vbCrLf &  _
"										PRE_QUOTE_REQUIREMENT," & vbCrLf &  _
"										POST_QUOTE_REQUIREMENT," & vbCrLf &  _
"										PURCHASE_REQUIREMENT," & vbCrLf &  _
"										PARENT_ID," & vbCrLf &  _
"										HELP_TEXT," & vbCrLf &  _
"										DEFAULT_OBJECT_ID," & vbCrLf &  _
"										DEFAULT_PROPERTY_ID," & vbCrLf &  _
"										IS_VALUATION," & vbCrLf &  _
"										IS_RATE_AND_PREMIUM," & vbCrLf &  _
"										CHILD_SCREEN_ID," & vbCrLf &  _
"										PMFORMAT," & vbCrLf &  _
"										COLUMN_POSITION," & vbCrLf &  _
"										TAB_SET_INDEX," & vbCrLf &  _
"										DATA_MODEL_TYPE) VALUES(@GISSDSCREENID,'" & vbCrLf &  _
"                                       + SCREEN_DETAIL_CNT + ',@GISSDOBJECTID,NULL,'" & vbCrLf &  _
"                                       + IS_FRAME + ',' + TAB_NUMBER + ','''" & vbCrLf &  _
"                                       + REPLACE(CAPTION, '''', '^') + ''',' + ITEM_TOP" & vbCrLf &  _
"                                       + ',' + ITEM_LEFT + ',' + ITEM_HEIGHT + ',' + ITEM_WIDTH" & vbCrLf &  _
"                                       + ',' + COLUMN_WIDTH + ',' + PRE_QUOTE_REQUIREMENT" & vbCrLf &  _
"                                       + ',' + POST_QUOTE_REQUIREMENT + ','" & vbCrLf &  _
"                                       + PURCHASE_REQUIREMENT + ',' + PARENT_ID + ','''" & vbCrLf &  _
"                                       + HELP_TEXT + ''',' + DEFAULT_OBJECT_ID + ','" & vbCrLf &  _
"                                       + DEFAULT_PROPERTY_ID + ',' + IS_VALUATION + ','" & vbCrLf &  _
"                                       + IS_RATE_AND_PREMIUM + ',@CHILDSCREENID,'" & vbCrLf &  _
"                                       + PMFORMAT + ',' + COLUMN_POSITION + ','" & vbCrLf &  _
"                                       + TAB_SET_INDEX + ',' + DATA_MODEL_TYPE + ') '" & vbCrLf &  _
"                                       + CHAR(13)" & vbCrLf &  _
"                                FROM   #GISSCREENDETAIL" & vbCrLf &  _
"                                WHERE  ID = @INDEX" & vbCrLf &  _
"                            END ----C3BB" & vbCrLf &  _
"                          IF @INSERTTYPE = 2" & vbCrLf &  _
"                            BEGIN ----C3BC" & vbCrLf &  _
"                                INSERT INTO #QUERIES" & vbCrLf &  _
"                                SELECT 'SELECT @GISSDPROPERTYID = GIP.GIS_PROPERTY_ID ,@GISSDOBJECTID =GIO.GIS_OBJECT_ID FROM GIS_PROPERTY GIP JOIN GIS_OBJECT GIO ON GIO.GIS_OBJECT_ID=GIP.GIS_OBJECT_ID WHERE '" & vbCrLf &  _
"                                       + CHAR(13) + 'GIP.PROPERTY_NAME = '''" & vbCrLf &  _
"                                       + GIS_PROPERTY_COLUMN_NAME" & vbCrLf &  _
"                                       + ''' AND GIO.TABLE_NAME= '''" & vbCrLf &  _
"                                       + GIS_OBJECT_TABLE_NAME + ''' ' + CHAR(13)" & vbCrLf &  _
"                                       + 'SELECT @GISSDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''" & vbCrLf &  _
"                                       + GIS_SCREEN_CODE" & vbCrLf &  _
"                                       + '''  AND GIS_DATA_MODEL_ID = (SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''" & vbCrLf &  _
"                                       + @sDataModelCode + ''') ' + CHAR(13)" & vbCrLf &  _
"                                       + 'INSERT INTO GIS_SCREEN_DETAIL (GIS_SCREEN_ID," & vbCrLf &  _
"										SCREEN_DETAIL_CNT," & vbCrLf &  _
"										GIS_OBJECT_ID," & vbCrLf &  _
"										GIS_PROPERTY_ID," & vbCrLf &  _
"										IS_FRAME," & vbCrLf &  _
"										TAB_NUMBER," & vbCrLf &  _
"										CAPTION," & vbCrLf &  _
"										ITEM_TOP," & vbCrLf &  _
"										ITEM_LEFT," & vbCrLf &  _
"										ITEM_HEIGHT," & vbCrLf &  _
"										ITEM_WIDTH," & vbCrLf &  _
"										COLUMN_WIDTH," & vbCrLf &  _
"										PRE_QUOTE_REQUIREMENT," & vbCrLf &  _
"										POST_QUOTE_REQUIREMENT," & vbCrLf &  _
"										PURCHASE_REQUIREMENT," & vbCrLf &  _
"										PARENT_ID," & vbCrLf &  _
"										HELP_TEXT," & vbCrLf &  _
"										DEFAULT_OBJECT_ID," & vbCrLf &  _
"										DEFAULT_PROPERTY_ID," & vbCrLf &  _
"										IS_VALUATION," & vbCrLf &  _
"										IS_RATE_AND_PREMIUM," & vbCrLf &  _
"										CHILD_SCREEN_ID," & vbCrLf &  _
"										PMFORMAT," & vbCrLf &  _
"										COLUMN_POSITION," & vbCrLf &  _
"										TAB_SET_INDEX," & vbCrLf &  _
"										DATA_MODEL_TYPE) VALUES(@GISSDSCREENID,'" & vbCrLf &  _
"                                       + SCREEN_DETAIL_CNT" & vbCrLf &  _
"                                       + ',@GISSDOBJECTID,@GISSDPROPERTYID,'" & vbCrLf &  _
"                                       + IS_FRAME + ',' + TAB_NUMBER + ','''" & vbCrLf &  _
"                                       + REPLACE(CAPTION, '''', '^') + ''',' + ITEM_TOP" & vbCrLf &  _
"                                       + ',' + ITEM_LEFT + ',' + ITEM_HEIGHT + ',' + ITEM_WIDTH" & vbCrLf &  _
"                                       + ',' + COLUMN_WIDTH + ',' + PRE_QUOTE_REQUIREMENT" & vbCrLf &  _
"                                       + ',' + POST_QUOTE_REQUIREMENT + ','" & vbCrLf &  _
"                                       + PURCHASE_REQUIREMENT + ',' + PARENT_ID + ','''" & vbCrLf &  _
"                                       + HELP_TEXT + ''',' + DEFAULT_OBJECT_ID + ','" & vbCrLf &  _
"                                       + DEFAULT_PROPERTY_ID + ',' + IS_VALUATION + ','" & vbCrLf &  _
"                                       + IS_RATE_AND_PREMIUM + ',@CHILDSCREENID,'" & vbCrLf &  _
"                                       + PMFORMAT + ',' + COLUMN_POSITION + ','" & vbCrLf &  _
"                                       + TAB_SET_INDEX + ',' + DATA_MODEL_TYPE + ') '" & vbCrLf &  _
"                                       + CHAR(13)" & vbCrLf &  _
"                                FROM   #GISSCREENDETAIL" & vbCrLf &  _
"                                WHERE  ID = @INDEX" & vbCrLf &  _
"                            END --C3BC" & vbCrLf &  _
"                          SET @INDEX=@INDEX + 1" & vbCrLf &  _
"                      END --C3B" & vbCrLf &  _
"                END --C3A" & vbCrLf &  _
"            END --C3" & vbCrLf &  _
"      END --A3" & vbCrLf &  _
"    ----@MANAGE UMLS-----" & vbCrLf &  _
"    SELECT @INDEX = 1," & vbCrLf &  _
"           @MAXCNT = MAX(ID)" & vbCrLf &  _
"    FROM   #UML" & vbCrLf &  _
"    IF @MAXCNT > 0" & vbCrLf &  _
"      BEGIN --A4 " & vbCrLf &  _
"          DECLARE @UMLCODE VARCHAR(10)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT ' '" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '--*********************************** THIS WILL INSERT UMLS IF NEEDED FOR DATAMODEL ( '" & vbCrLf &  _
"                 + @sDataModelCode" & vbCrLf &  _
"                 + ') ***********************************'" & vbCrLf &  _
"          WHILE ( @INDEX <= @MAXCNT )" & vbCrLf &  _
"            BEGIN--A4A" & vbCrLf &  _
"                SELECT @UMLCODE = #UML.UMLCODE" & vbCrLf &  _
"                FROM   #UML" & vbCrLf &  _
"                WHERE  ID = @INDEX" & vbCrLf &  _
"                EXECUTE spu_UML_Script" & vbCrLf &  _
"                  @UMLCODE" & vbCrLf &  _
"                SET @INDEX=@INDEX + 1" & vbCrLf &  _
"            END --A4A" & vbCrLf &  _
"      END --A4" & vbCrLf &  _
"    ----@MANAGE UDLS-----" & vbCrLf &  _
"    SELECT @INDEX = 1," & vbCrLf &  _
"           @MAXCNT = MAX(ID)" & vbCrLf &  _
"    FROM   #UDL" & vbCrLf &  _
"    IF @MAXCNT > 0" & vbCrLf &  _
"      BEGIN --A5" & vbCrLf &  _
"          DECLARE @UDLCODE VARCHAR(255)" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT ' '" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '--*********************************** THIS WILL INSERT UDLS/PMLOOKUPS IF NEEDED FOR DATAMODEL ( '" & vbCrLf &  _
"                 + @sDataModelCode" & vbCrLf &  _
"                 + ') ***********************************'" & vbCrLf &  _
"          WHILE ( @INDEX <= @MAXCNT )" & vbCrLf &  _
"            BEGIN--A5A" & vbCrLf &  _
"                SELECT @UDLCODE = #UDL.UDLNAME" & vbCrLf &  _
"                FROM   #UDL" & vbCrLf &  _
"                WHERE  ID = @INDEX" & vbCrLf &  _
"                EXECUTE spu_UDL_Script" & vbCrLf &  _
"                  @UDLCODE" & vbCrLf &  _
"                SET @INDEX=@INDEX + 1" & vbCrLf &  _
"            END --A5A" & vbCrLf &  _
"      END --A5" & vbCrLf &  _
"    IF @COPYPRODUCTANDRISK = 1" & vbCrLf &  _
"      BEGIN --B1" & vbCrLf &  _
"          EXECUTE spu_GetProductAndRisk" & vbCrLf &  _
"            @sDataModelCode" & vbCrLf &  _
"      END --B1" & vbCrLf &  _
"    --***********************************=====================FINAL=========================***********************************---" & vbCrLf &  _
"    SELECT @MAXCNT = CONVERT(INT, MAX(ID))" & vbCrLf &  _
"    FROM   #QUERIES" & vbCrLf &  _
"    IF @MAXCNT > 0" & vbCrLf &  _
"      BEGIN--Y		" & vbCrLf &  _
"          IF @CAPTURESCREENS <> 0" & vbCrLf &  _
"            BEGIN--YA1" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'UPDATE GIS_SCREEN 	' + CHAR(13)" & vbCrLf &  _
"                       + 'SET SCRIPT_DEFAULTS = REPLACE(CONVERT(VARCHAR(MAX),SCRIPT_DEFAULTS),''^'',''''''''), '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'SCRIPT_DYNAMIC_LOGIC = REPLACE(CONVERT(VARCHAR(MAX),SCRIPT_DYNAMIC_LOGIC),''^'','''''''') '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'WHERE SCRIPT_DEFAULTS IS NOT NULL AND GIS_DATA_MODEL_ID =@DATAMODELID'" & vbCrLf &  _
"                INSERT INTO #QUERIES" & vbCrLf &  _
"                SELECT 'UPDATE GIS_SCREEN_DETAIL ' + CHAR(13)" & vbCrLf &  _
"                       + 'SET CAPTION = REPLACE(CAPTION,''^'','''''''') '" & vbCrLf &  _
"                       + CHAR(13)" & vbCrLf &  _
"                       + 'WHERE CAPTION IS NOT NULL AND GIS_SCREEN_ID IN( SELECT GIS_SCREEN_ID FROM GIS_SCREEN  WHERE GIS_DATA_MODEL_ID =@DATAMODELID)'" & vbCrLf &  _
"            END --YA1" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT 'UPDATE GIS_PROPERTY ' + CHAR(13)" & vbCrLf &  _
"                 + 'SET SPECIALS_TYPE_REFERENCE=CONVERT(VARCHAR(10),GISUDH.GIS_USER_DEF_HEADER_ID) '" & vbCrLf &  _
"                 + CHAR(13) + 'FROM GIS_PROPERTY GISP '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'JOIN GIS_USER_DEF_HEADER GISUDH '" & vbCrLf &  _
"                 + CHAR(13)" & vbCrLf &  _
"                 + 'ON GISUDH.CODE=GISP.SPECIALS_TYPE_REFERENCE '" & vbCrLf &  _
"                 + CHAR(13) + 'WHERE GISP.SPECIALS_TYPE=6'" & vbCrLf &  _
"				  --INSERT INTO #QUERIES" & vbCrLf &  _
"				  --SELECT" & vbCrLf &  _
"				  --'UPDATE GIS_USER_DEF_HEADER ' + CHAR(13) +" & vbCrLf &  _
"				  --'SET DESCRIPTION = REPLACE(DESCRIPTION,''^'','''''''')' " & vbCrLf &  _
"				  --INSERT INTO #QUERIES" & vbCrLf &  _
"				  --SELECT" & vbCrLf &  _
"				  --'UPDATE GIS_USER_DEF_DETAIL ' + CHAR(13) +" & vbCrLf &  _
"				  --'SET DESCRIPTION = REPLACE(DESCRIPTION,''^'','''''''')' " & vbCrLf &  _
"      END --Y" & vbCrLf &  _
"    ELSE" & vbCrLf &  _
"      BEGIN--Z" & vbCrLf &  _
"          INSERT INTO #QUERIES" & vbCrLf &  _
"          SELECT '--NO SCRIPT AVAILABLE FOR DATAMODEL ( '" & vbCrLf &  _
"                 + @sDataModelCode + ')'" & vbCrLf &  _
"      END --Z	" & vbCrLf &  _
"    SELECT QUERY" & vbCrLf &  _
"    FROM   #QUERIES" & vbCrLf &  _
"    ORDER  BY ID ASC" & vbCrLf &  _
"    DROP TABLE #GISOBJECTS" & vbCrLf &  _
"    DROP TABLE #GISPROPERTIES" & vbCrLf &  _
"    DROP TABLE #GISSCREEN" & vbCrLf &  _
"    DROP TABLE #GISSCREENDETAIL" & vbCrLf &  _
"    DROP TABLE #UML" & vbCrLf &  _
"    DROP TABLE #UDL" & vbCrLf &  _
"    DROP TABLE #QUERIES "
End Module

