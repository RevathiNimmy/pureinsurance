DDLDropProcedure 'spu_GetProductAndRisk'
GO

CREATE PROCEDURE spu_GetProductAndRisk @sDataModelCode VARCHAR(10)
AS
    SELECT RT.RISK_TYPE_ID,
           RT.CODE
    INTO   #RISKCODES
    FROM   RISK_TYPE RT
           JOIN GIS_SCREEN GSCR
             ON RT.GIS_SCREEN_ID = GSCR.GIS_SCREEN_ID
           JOIN GIS_DATA_MODEL GDM
             ON GDM.GIS_DATA_MODEL_ID = GSCR.GIS_DATA_MODEL_ID
    WHERE  GDM.GIS_DATA_MODEL_TYPE_ID = 1
           AND GDM.CODE = @sDataModelCode

    SELECT DISTINCT RTG.*
    INTO   #RISKTYPEGROUP
    FROM   RISK_TYPE_GROUP RTG
           JOIN RISK_TYPE_USAGE RTU
             ON RTU.RISK_TYPE_GROUP_ID = RTG.RISK_TYPE_GROUP_ID
           JOIN #RISKCODES
             ON #RISKCODES.RISK_TYPE_ID = RTU.RISK_TYPE_ID

    SELECT PROD.*
    INTO   #PRODUCTS
    FROM   PRODUCT PROD
           JOIN PRODUCT_RISK_TYPE_GROUP PRTG
             ON PROD.PRODUCT_ID = PRTG.PRODUCT_ID
           JOIN #RISKTYPEGROUP RTG
             ON RTG.RISK_TYPE_GROUP_ID = PRTG.RISK_TYPE_GROUP_ID

    IF EXISTS(SELECT *
              FROM   #RISKCODES)
      BEGIN
          INSERT INTO #QUERIES
          SELECT '---THIS BIT OF SQL WILL COPY RISK_TYPE_GROUP------- '
                 + CHAR(13) + 'DECLARE @MAXGROUPID INT '
                 + CHAR(13) + 'DECLARE @GROUPCAPTIONID INT '
                 + CHAR(13)
                 + 'SELECT @MAXGROUPID =MAX(RISK_TYPE_GROUP_ID) FROM RISK_TYPE_GROUP '
                 + CHAR(13)

          INSERT INTO #QUERIES
          SELECT 'IF NOT EXISTS(SELECT RISK_TYPE_GROUP_ID FROM RISK_TYPE_GROUP WHERE CODE ='''
                 + RTRIM(LTRIM(CODE)) + ''') ' + CHAR(13) + 'BEGIN '
                 + CHAR(13)
                 + 'SET @MAXGROUPID = @MAXGROUPID + 1 '
                 + CHAR(13)
                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
                 + RTRIM(LTRIM(DESCRIPTION))
                 + ''', @GROUPCAPTIONID OUTPUT ' + CHAR(13)
                 + 'INSERT INTO RISK_TYPE_GROUP (
					RISK_TYPE_GROUP_ID,
					CAPTION_ID,
					CODE,
					DESCRIPTION,
					IS_DELETED,
					EFFECTIVE_DATE) VALUES(
					@MAXGROUPID,
					@GROUPCAPTIONID,'''
                 + ISNULL(CONVERT(CHAR(10), CODE), 'NULL')
                 + ''','''
                 + ISNULL(CONVERT(VARCHAR(255), DESCRIPTION), 'NULL')
                 + ''','
                 + ISNULL(CONVERT(VARCHAR(10), IS_DELETED), 'NULL')
                 + ','''
                 + ISNULL(CONVERT(VARCHAR(20), EFFECTIVE_DATE), 'NULL')
                 + ''')' + CHAR(13) + ' END ' + CHAR(13)
          FROM   #RISKTYPEGROUP

          INSERT INTO #QUERIES
          SELECT '---THIS BIT OF SQL WILL COPY RISK_TYPE-------'
                 + CHAR(13) + 'DECLARE @MAXRISKTYPEID INT '
                 + CHAR(13)
                 + 'DECLARE @RISKTYPECAPTIONID INT '
                 + CHAR(13)
                 + 'SELECT @MAXRISKTYPEID =MAX(RISK_TYPE_ID) FROM RISK_TYPE '
                 + CHAR(13)

          INSERT INTO #QUERIES
          SELECT 'IF NOT EXISTS(SELECT RISK_TYPE_ID FROM RISK_TYPE WHERE CODE ='''
                 + RTRIM(LTRIM(RT.CODE)) + ''') ' + CHAR(13)
                 + 'BEGIN ' + CHAR(13)
                 + 'SET @MAXRISKTYPEID = @MAXRISKTYPEID + 1 '
                 + CHAR(13)
                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
                 + RTRIM(LTRIM(DESCRIPTION))
                 + ''', @RISKTYPECAPTIONID OUTPUT ' + CHAR(13)
                 + 'INSERT INTO RISK_TYPE ( RISK_TYPE_ID,RISK_FOLDER_TYPE_ID,CAPTION_ID,CODE,DESCRIPTION,EFFECTIVE_DATE,IS_DELETED,ACCUMULATION_LEVEL,
					GIS_SCREEN_ID,IS_DEFERRED_RI_PERMITTED,CLAIMS_GIS_SCREEN_ID,CLAIMS_IS_POST_TAXES,DISPLAY_REINSURANCE_SCREEN,SHOW_INFORMATION_CHECKLIST) VALUES ( 
					@MAXRISKTYPEID,NULL,@RISKTYPECAPTIONID,'''
                 + ISNULL(CONVERT(VARCHAR(20), RT.CODE), 'NULL')
                 + ''','''
                 + ISNULL(CONVERT(VARCHAR(20), DESCRIPTION), 'NULL')
                 + ''','''
                 + ISNULL(CONVERT(VARCHAR(20), EFFECTIVE_DATE), 'NULL')
                 + ''','
                 + ISNULL(CONVERT(VARCHAR(20), IS_DELETED), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), ACCUMULATION_LEVEL), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), GIS_SCREEN_ID), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), IS_DEFERRED_RI_PERMITTED), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), CLAIMS_GIS_SCREEN_ID), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), CLAIMS_IS_POST_TAXES), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), DISPLAY_REINSURANCE_SCREEN), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), SHOW_INFORMATION_CHECKLIST), 'NULL')
                 + ')' + CHAR(13) + ' END ' + CHAR(13)
          FROM   RISK_TYPE RT
                 JOIN #RISKCODES
                   ON #RISKCODES.RISK_TYPE_ID = RT.RISK_TYPE_ID

          INSERT INTO #QUERIES
          SELECT '---THIS BIT OF SQL WILL CREATE RISK_TYPE_USUAGE ENTRIES-------'
                 + CHAR(13) + 'DECLARE @TEMPRISK_TYPE_ID INT '
                 + CHAR(13)
                 + 'DECLARE @TEMPRISK_TYPE_GROUP_ID INT '
                 + CHAR(13)

          INSERT INTO #QUERIES
          SELECT 'SELECT @TEMPRISK_TYPE_ID = RISK_TYPE_ID FROM RISK_TYPE FROM WHERE CODE ='''
                 + RTRIM(LTRIM(RC.CODE)) + '''' + CHAR(13)
                 + 'SELECT @TEMPRISK_TYPE_GROUP_ID = RISK_TYPE_GROUP_ID FROM RISK_TYPE_GROUP FROM WHERE CODE ='''
                 + RTRIM(LTRIM(RTG.CODE)) + '''' + CHAR(13)
                 + 'IF NOT EXISTS(SELECT * FROM RISK_TYPE_USAGE WHERE RISK_TYPE_ID =@TEMPRISK_TYPE_ID AND RISK_TYPE_GROUP_ID =@TEMPRISK_TYPE_GROUP_ID) '
                 + CHAR(13) + 'BEGIN ' + CHAR(13)
                 + 'INSERT INTO RISK_TYPE_USAGE (RISK_TYPE_ID,RISK_TYPE_GROUP_ID) VALUES (@TEMPRISK_TYPE_ID,@TEMPRISK_TYPE_GROUP_ID) '
                 + CHAR(13) + ' END ' + CHAR(13)
          FROM   RISK_TYPE_USAGE RTU
                 JOIN #RISKTYPEGROUP RTG
                   ON RTG.RISK_TYPE_GROUP_ID = RTU.RISK_TYPE_GROUP_ID
                 JOIN #RISKCODES RC
                   ON RC.RISK_TYPE_ID = RTU.RISK_TYPE_ID

          INSERT INTO #QUERIES
          SELECT '---THIS BIT OF SQL WILL COPY PRODUCT-------'
                 + CHAR(13) + 'DECLARE @MAXPRODUCTID INT '
                 + CHAR(13) + 'DECLARE @PRODUCTCAPTIONID INT '
                 + CHAR(13)
                 + 'SELECT @MAXPRODUCTID =MAX(PRODUCT_ID) FROM PRODUCT '
                 + CHAR(13)

          INSERT INTO #QUERIES
          SELECT 'IF NOT EXISTS(SELECT PRODUCT_ID FROM PRODUCT WHERE CODE ='''
                 + RTRIM(LTRIM(CODE)) + ''')' + CHAR(13) + 'BEGIN '
                 + CHAR(13)
                 + 'SET @MAXPRODUCTID = @MAXPRODUCTID + 1 '
                 + CHAR(13)
                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
                 + RTRIM(LTRIM(DESCRIPTION))
                 + ''', @PRODUCTCAPTIONID OUTPUT ' + CHAR(13)
                 + 'INSERT INTO PRODUCT (
					PRODUCT_ID,
					CAPTION_ID,
					CODE,
					DESCRIPTION,
					EFFECTIVE_DATE,
					IS_DELETED,
					CAN_MAKE_LIVE_INVOICE,
					CAN_MAKE_LIVE_INSTALMENTS,
					CAN_MAKE_LIVE_PAYNOW,
					PRODUCE_SCHEDULE,
					PRODUCE_CERTIFICATE,
					PRODUCE_DEBIT_NOTE) VALUES (@MAXPRODUCTID,@PRODUCTCAPTIONID,'
                 + ISNULL(CONVERT(VARCHAR(20), CODE), 'NULL')
                 + ''','''
                 + ISNULL(CONVERT(VARCHAR(20), DESCRIPTION), 'NULL')
                 + ''','''
                 + ISNULL(CONVERT(VARCHAR(20), EFFECTIVE_DATE), 'NULL')
                 + ''','
                 + ISNULL(CONVERT(VARCHAR(20), IS_DELETED), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), CAN_MAKE_LIVE_INVOICE), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), CAN_MAKE_LIVE_INSTALMENTS), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), CAN_MAKE_LIVE_PAYNOW), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), PRODUCE_SCHEDULE), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), PRODUCE_CERTIFICATE), 'NULL')
                 + ','
                 + ISNULL(CONVERT(VARCHAR(20), PRODUCE_DEBIT_NOTE), 'NULL')
                 + ') ' + CHAR(13) + ' END ' + CHAR(13)
          FROM   #PRODUCTS

          INSERT INTO #QUERIES
          SELECT '---THIS BIT OF SQL WILL CREATE PRODUCT_RISK_TYPE_GROUP ENTRIES-------'
                 + CHAR(13) + 'DECLARE @PPRODUCT_ID INT '
                 + CHAR(13)
                 + 'DECLARE @PRISK_TYPE_GROUP_ID INT '
                 + CHAR(13)

          INSERT INTO #QUERIES
          SELECT 'SELECT @PPRODUCT_ID = RISK_TYPE_ID FROM RISK_TYPE FROM WHERE CODE ='''
                 + RTRIM(LTRIM(P.CODE)) + '''' + CHAR(13)
                 + 'SELECT @PRISK_TYPE_GROUP_ID = RISK_TYPE_GROUP_ID FROM RISK_TYPE_GROUP FROM WHERE CODE ='''
                 + RTRIM(LTRIM(RTG.CODE)) + '''' + CHAR(13)
                 + 'IF NOT EXISTS(SELECT * FROM PRODUCT_RISK_TYPE_GROUP WHERE PRODUCT_ID =@PPRODUCT_ID AND RISK_TYPE_GROUP_ID =@PRISK_TYPE_GROUP_ID) '
                 + CHAR(13) + 'BEGIN ' + CHAR(13)
                 + 'INSERT INTO RISK_TYPE_USAGE (PRODUCT_ID,RISK_TYPE_GROUP_ID) VALUES (@PPRODUCT_ID,@PRISK_TYPE_GROUP_ID) '
                 + CHAR(13) + ' END ' + CHAR(13)
          FROM   PRODUCT_RISK_TYPE_GROUP PRTG
                 JOIN #PRODUCTS P
                   ON PRTG.PRODUCT_ID = P.PRODUCT_ID
                 JOIN #RISKTYPEGROUP RTG
                   ON PRTG.RISK_TYPE_GROUP_ID = RTG.RISK_TYPE_GROUP_ID
      END

    DROP TABLE #RISKCODES
    DROP TABLE #RISKTYPEGROUP
    DROP TABLE #PRODUCTS

GO

--USE @DATAMODELID AS DATAMODEL ID
DDLDropProcedure 'spu_CreatePolicyBinder'
GO

CREATE PROCEDURE spu_CreatePolicyBinder @sDataModelCode VARCHAR(10),
                                        @DATA           VARCHAR(MAX) OUTPUT
AS
    SET @sDataModelCode =RTRIM(@sDataModelCode)

    SELECT @DATA = 'DECLARE  @MAXPBOID INT ' + CHAR(13)
                   + 'DECLARE  @MAXPBPID INT ' + CHAR(13)
                   + 'SELECT @MAXPBOID = MAX(GIS_OBJECT_ID)+1 FROM GIS_OBJECT WITH(NOLOCK) '
                   + CHAR(13)
                   + 'SELECT @MAXPBPID = MAX(GIS_PROPERTY_ID)+1 FROM GIS_PROPERTY WITH(NOLOCK) '
                   + CHAR(13) + 'INSERT INTO GIS_OBJECT (GIS_OBJECT_ID,
					GIS_DATA_MODEL_ID,
					OBJECT_NAME,
					TABLE_NAME,
					MAX_INSTANCES,
					IS_QUOTE_OBJECT,
					PARENT_OBJECT_ID,
					POLARIS_OBJECT_ID,
					IS_SELECTABLE_FOR_SCREEN,
					IS_NON_GIS,
					EDIT_FLAGS) ' + CHAR(13) + 'VALUES ('
                   + '@MAXPBOID,@DATAMODELID,'''
                   + @sDataModelCode + '_POLICY_BINDER'','''
                   + @sDataModelCode
                   + '_POLICY_BINDER'',1,0,NULL,NULL,0,0,0) '
                   + CHAR(13) + 'INSERT INTO GIS_PROPERTY (GIS_PROPERTY_ID,
					GIS_OBJECT_ID,
					PROPERTY_NAME,
					COLUMN_NAME,
					DATA_TYPE,
					IS_INPUT_PROPERTY,
					IS_IDENTIFYING_PROPERTY,
					IS_PRIMARY_KEY,
					POLARIS_PROPERTY_ID,
					IS_DELETED,
					IS_SEARCH_PROPERTY,
					INDEX_LINKING_ID,
					EDIT_FLAGS,
					SPECIALS_TYPE,
					SPECIALS_TYPE_REFERENCE)  '
                   + CHAR(13) + 'VALUES ('
                   + '@MAXPBPID,@MAXPBOID,''' + @sDataModelCode
                   + '_POLICY_BINDER_ID'',''' + @sDataModelCode
                   + '_POLICY_BINDER_ID'',2,0,1,1,NULL,0,0,NULL,0,0,NULL)'
                   + CHAR(13) + 'SET @MAXPBPID= @MAXPBPID+1 '
                   + CHAR(13) + 'INSERT INTO GIS_PROPERTY (GIS_PROPERTY_ID,
					GIS_OBJECT_ID,
					PROPERTY_NAME,
					COLUMN_NAME,
					DATA_TYPE,
					IS_INPUT_PROPERTY,
					IS_IDENTIFYING_PROPERTY,
					IS_PRIMARY_KEY,
					POLARIS_PROPERTY_ID,
					IS_DELETED,
					IS_SEARCH_PROPERTY,
					INDEX_LINKING_ID,
					EDIT_FLAGS,
					SPECIALS_TYPE,
					SPECIALS_TYPE_REFERENCE)  '
                   + CHAR(13) + 'VALUES ('
                   + '@MAXPBPID,@MAXPBOID,''GIS_POLICY_LINK_ID'',''GIS_POLICY_LINK_ID'',2,0,0,0,NULL,0,0,NULL,0,0,NULL)'
                   + CHAR(13) + 'CREATE TABLE ' + @sDataModelCode
                   + '_POLICY_BINDER(' + @sDataModelCode
                   + '_POLICY_BINDER_ID INT NOT NULL,
					GIS_POLICY_LINK_ID INT NULL)' + CHAR(13)
                   + ' ALTER TABLE ' + @sDataModelCode
                   + '_POLICY_BINDER ADD PRIMARY KEY NONCLUSTERED ('
                   + @sDataModelCode + '_POLICY_BINDER_ID ASC '
                   + ')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY] '
                   + CHAR(13) + ' ALTER TABLE ' + @sDataModelCode
                   + '_POLICY_BINDER  WITH CHECK ADD FOREIGN KEY(GIS_POLICY_LINK_ID) '
                   + 'REFERENCES GIS_POLICY_LINK (GIS_POLICY_LINK_ID) ON DELETE CASCADE '
                   + CHAR(13)

GO

DDLDropProcedure 'spu_GetUDLFieldNames'
GO

CREATE PROCEDURE spu_GetUDLFieldNames @TABLECODE          VARCHAR(255),
                                      @FIELDS             VARCHAR(255) OUTPUT,
                                      @FIELDSWITHDATATYPE VARCHAR(300) OUTPUT
AS
    DECLARE @TABLENAMEID VARCHAR(255)
    DECLARE @TABLENAME VARCHAR(255)

    SET @TABLENAME = 'UDL_' + LTRIM(RTRIM(@TABLECODE))
    SET @TABLENAMEID=RTRIM(@TABLENAME) + '_ID'
    SET @FIELDS=''
    SET @FIELDSWITHDATATYPE=''

    SELECT @FIELDS = @FIELDS + ',' + RTRIM(LTRIM(NAME)),
           @FIELDSWITHDATATYPE = @FIELDSWITHDATATYPE + ',' + RTRIM(LTRIM(NAME))
                                 + ' VARCHAR(255)'
    FROM   SYS.ALL_COLUMNS
    WHERE  OBJECT_ID IN
           --SELECT  NAME FROM SYS.ALL_COLUMNS WHERE OBJECT_ID IN 
           (SELECT OBJECT_ID
            FROM   SYS.OBJECTS
            WHERE  NAME = @TABLENAME)
           AND NAME NOT IN ( @TABLENAMEID, 'CAPTION_ID', 'CODE', 'DESCRIPTION',
                             'IS_DELETED', 'EFFECTIVE_DATE', 'UDL_VERSION' )

    SELECT @FIELDS = SUBSTRING(@FIELDS, 2, LEN(@FIELDS))

    SELECT @FIELDSWITHDATATYPE = SUBSTRING(@FIELDSWITHDATATYPE, 2, LEN(@FIELDSWITHDATATYPE))

GO

DDLDropProcedure 'spu_GetUDLValue'
GO

CREATE PROCEDURE spu_GetUDLValue @TABLECODE VARCHAR(255),
                                 @FIELDS    VARCHAR(MAX),
                                 @FIELDCODE VARCHAR(MAX),
                                 @DATA      VARCHAR(MAX) OUTPUT
AS
    DECLARE @SQL VARCHAR(MAX)
    DECLARE @MODIFIEDFILEDS VARCHAR(MAX)

    IF @FIELDS <> ''
--      SET @FIELDS = ',' + @FIELDS

    SET @MODIFIEDFILEDS = 'CODE,DESCRIPTION,' + @FIELDS
    SET @MODIFIEDFILEDS = '''#''+ISNULL('
                          + REPLACE(@MODIFIEDFILEDS, ',', ',''NULL'')+''#,#''+ISNULL(')
                          + ',''NULL'')+''#'''

    CREATE TABLE #TEMPUDL
      (
         SQLDATA VARCHAR(500)
      )

    SELECT @SQL = 'INSERT INTO #TEMPUDL  SELECT '
                  + @MODIFIEDFILEDS + ' FROM UDL_'
                  + LTRIM(@TABLECODE) + ' WHERE CODE = '''
                  + @FIELDCODE + ''''

    EXECUTE(@SQL)

    SELECT TOP 1 @DATA = REPLACE(#TEMPUDL.SQLDATA, '#', '''''')
    FROM   #TEMPUDL

    DROP TABLE #TEMPUDL

GO

--THIS SP IS CALLED VIA OTHER SP SO EXPECTS TEMPTABLE TO BE THERE WITH FOLLOWING STRUCTURE
--CREATE TABLE #QUERIES(ID INT IDENTITY,QUERY TEXT)
DDLDropProcedure 'spu_UDL_Script'
GO

CREATE PROCEDURE spu_UDL_Script @UDLCODE VARCHAR(255)
AS
    SET @UDLCODE= LTRIM(@UDLCODE)

    DECLARE @UDLDESCRIPTION VARCHAR(255)
    DECLARE @FIELDS VARCHAR(200)
    DECLARE @FIELDSWITHDATATYPE VARCHAR(400)

    EXEC spu_GetUDLFieldNames
      @UDLCODE,
      @FIELDS OUTPUT,
      @FIELDSWITHDATATYPE OUTPUT

    DECLARE @COMMAND VARCHAR(200)

    EXECUTE DDLDropTable
      'TRSCURRENTTABLE'

    SELECT @UDLDESCRIPTION = RTRIM(LTRIM(DESCRIPTION))
    FROM   GIS_LIST_TYPE
    WHERE  CODE = @UDLCODE

    INSERT INTO #QUERIES
    SELECT '----**THIS SCRIPT IS GENERATED FOR UDL '
           + RTRIM(LTRIM(@UDLCODE)) + ' **------'

    INSERT INTO #QUERIES
    SELECT 'IF  NOT EXISTS(SELECT GIS_LIST_TYPE_ID FROM GIS_LIST_TYPE WHERE CODE= '''
           + @UDLCODE + ''' AND DESCRIPTION= '''
           + @UDLDESCRIPTION + ''') ' + CHAR(13) + 'BEGIN '
           + CHAR(13) + 'EXEC SPU_GIS_LIST_TYPE_ADD '''
           + @UDLCODE + ''',''' + @UDLDESCRIPTION + ''' '
           + CHAR(13)
           + 'EXEC SPU_GIS_LIST_PM_CAPTION ''UDL_'
           + @UDLCODE + ''',''' + @FIELDSWITHDATATYPE + ''' '
           + CHAR(13) + ' END ' + CHAR(13)

    IF EXISTS(SELECT NULL
              FROM   SYSOBJECTS
              WHERE  NAME = 'UDL_' + RTRIM(@UDLCODE)
                     AND XTYPE = 'U')
      BEGIN
          SELECT @COMMAND = 'SELECT UDL_' + RTRIM(@UDLCODE)
                            + '_ID AS UDLID,* INTO TRSCURRENTTABLE FROM UDL_'
                            + @UDLCODE

          EXECUTE(@COMMAND)

          DECLARE @MAXCNT INT
          DECLARE @INDEX INT
          DECLARE @DATA VARCHAR(500)
          DECLARE @FIELDCODE VARCHAR(25)

          SELECT @INDEX = 1,
                 @MAXCNT = MAX(UDLID)
          FROM   TRSCURRENTTABLE

          IF @MAXCNT > 0
            BEGIN
                WHILE( @INDEX <= @MAXCNT )
                  BEGIN
                      SELECT @FIELDCODE = CODE
                      FROM   TRSCURRENTTABLE
                      WHERE  UDLID = @INDEX

                      EXEC spu_GetUDLValue
                        @UDLCODE,
                        @FIELDS,
                        @FIELDCODE,
                        @DATA OUTPUT

                      INSERT INTO #QUERIES
                      SELECT 'IF NOT EXISTS(SELECT * FROM UDL_'
                             + @UDLCODE + ' WHERE CODE = '''
                             + TRSCURRENTTABLE.CODE + ''') ' + CHAR(13)
                             + 'BEGIN ' + CHAR(13)
                             + 'EXEC SPU_GIS_LISTENTRY_ADD @TABLE=N''UDL_'
                             + @UDLCODE + ''', ' + '@FIELDS=N'',' + @FIELDS + ''', '
                             + '@DATA=N'',' + @DATA + ''',@EFFDATE=N'''
                             + CONVERT(VARCHAR(30), TRSCURRENTTABLE.EFFECTIVE_DATE)
                             + ''',@LANGUAGE_ID=1,@CAPTION=N'''
                             + TRSCURRENTTABLE.DESCRIPTION + ''' ' + CHAR(13)
                             + 'EXEC SPU_GIS_LIST_ADD_USAGE @LISTTYPE=N'''
                             + @UDLCODE + ''',@CODE=N'''
                             + TRSCURRENTTABLE.CODE
                             + ''',@VERSION=1,@EFFDATE='''
                             + CONVERT(VARCHAR(30), TRSCURRENTTABLE.EFFECTIVE_DATE)
                             + ''' ' + CHAR(13) + ' END ' + CHAR(13)
                      FROM   TRSCURRENTTABLE
                      WHERE  TRSCURRENTTABLE.UDLID = @INDEX

                      SET @INDEX=@INDEX + 1
                  END
            END

          DROP TABLE TRSCURRENTTABLE
      END
 
GO

DDLDropProcedure 'spu_CreateGISProperty'
GO

CREATE PROCEDURE spu_CreateGISProperty @TABLENAME  VARCHAR(100),
                                       @COLUMNNAME VARCHAR(100),
                                       @RSQL       VARCHAR(300) OUTPUT
AS
    SELECT @RSQL = 'EXECUTE DDLADDCOLUMN ''' + @TABLENAME
                   + ''',''' + @COLUMNNAME + ''' ,'' ' + CASE WHEN DATA_TYPE = 1 THEN 'DATETIME' WHEN DATA_TYPE = 2 THEN 'INT' WHEN DATA_TYPE = 5 THEN 'VARCHAR(255)' WHEN DATA_TYPE = 7 THEN 'VARCHAR(4000)' WHEN DATA_TYPE = 20 THEN 'TINYINT' WHEN DATA_TYPE = 21 THEN 'NUMERIC(19,4)' WHEN DATA_TYPE = 22 THEN 'NUMERIC(7,4)' WHEN DATA_TYPE = 23 THEN 'INT' ELSE 'VARCHAR(255)' END + ''',0'
    FROM   GIS_PROPERTY GISP
           JOIN GIS_OBJECT GISO
             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID
    WHERE  GISO.TABLE_NAME = @TABLENAME
           AND GISP.COLUMN_NAME = @COLUMNNAME

GO

DDLDropProcedure 'spu_CreateGISObject'
GO

CREATE PROCEDURE spu_CreateGISObject @TABLENAME VARCHAR(100),
                                     @RSQL      VARCHAR(MAX) OUTPUT
AS
    DECLARE @CLMMAX INT
    DECLARE @CLMINDEX INT
    DECLARE @sDataModelCode VARCHAR(10)
    DECLARE @OTHERKEYS VARCHAR(500)
    DECLARE @COLUMNNAME VARCHAR(60)
    DECLARE @TABLEKEY VARCHAR(60)
    DECLARE @PARENTOBJECTNAME VARCHAR(60)
    DECLARE @OTHERFOREIGNKEYS VARCHAR(500)

    SELECT @sDataModelCode = LTRIM(RTRIM(GISDM.CODE))
    FROM   GIS_OBJECT GISO
           JOIN GIS_DATA_MODEL GISDM
             ON GISDM.GIS_DATA_MODEL_ID = GISO.GIS_DATA_MODEL_ID
    WHERE  GISO.TABLE_NAME = @TABLENAME

    SET @TABLEKEY = @TABLENAME + '_ID'

    SELECT @PARENTOBJECTNAME = TABLE_NAME
    FROM   GIS_OBJECT
    WHERE  GIS_OBJECT_ID IN (SELECT PARENT_OBJECT_ID
                             FROM   GIS_OBJECT
                             WHERE  TABLE_NAME = @TABLENAME)

    CREATE TABLE #COLDETAILS
      (
         ID          INT IDENTITY,
         COLUMN_NAME VARCHAR(100),
         DATA_TYPE   VARCHAR(100)
      )

    INSERT INTO #COLDETAILS
    SELECT COLUMN_NAME,
           CASE
             WHEN DATA_TYPE = 1 THEN 'DATETIME'
             WHEN DATA_TYPE = 2 THEN 'INT'
             WHEN DATA_TYPE = 5 THEN 'VARCHAR(255)'
             WHEN DATA_TYPE = 7 THEN 'VARCHAR(4000)'
             WHEN DATA_TYPE = 20 THEN 'TINYINT'
             WHEN DATA_TYPE = 21 THEN 'NUMERIC(19,4)'
             WHEN DATA_TYPE = 22 THEN 'NUMERIC(7,4)'
             WHEN DATA_TYPE = 23 THEN 'INT'
             ELSE 'VARCHAR(255)'
           END
    FROM   GIS_PROPERTY GISP
           JOIN GIS_OBJECT GISO
             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID
    WHERE  GISO.TABLE_NAME = @TABLENAME
    ORDER  BY GISP.GIS_PROPERTY_ID ASC

    SELECT @CLMINDEX = 1,
           @CLMMAX = MAX(ID)
    FROM   #COLDETAILS

    SELECT @OTHERKEYS = ''

    DECLARE @SETNN CHAR(10)

    IF @CLMMAX > 0
      BEGIN
          --SELECT @RSQL = 'DROP TABLE ' + @TABLENAME + CHAR(13) 
          SELECT @RSQL = ' EXECUTE DDLDROPTABLE ' + @TABLENAME
                         + CHAR(13)

          SELECT @RSQL = @RSQL + ' CREATE TABLE ' + @TABLENAME + '( '

          WHILE( @CLMINDEX <= @CLMMAX )
            BEGIN
                SELECT @COLUMNNAME = COLUMN_NAME
                FROM   #COLDETAILS
                WHERE  ID = @CLMINDEX

                SET @SETNN=''

                IF SUBSTRING(@COLUMNNAME, 1, LEN(@sDataModelCode)) = @sDataModelCode
                   AND @COLUMNNAME <> @TABLEKEY
                  BEGIN
                      SELECT @OTHERKEYS = @OTHERKEYS + @COLUMNNAME + '###'
                  END

                IF SUBSTRING(@COLUMNNAME, 1, LEN(@sDataModelCode)) = @sDataModelCode
                  BEGIN
                      SET @SETNN=' NOT NULL '
                  END

                SELECT @RSQL = CASE
                                 WHEN @CLMINDEX < @CLMMAX THEN @RSQL + RTRIM(LTRIM(COLUMN_NAME)) + ' '
                                                               + RTRIM(LTRIM(DATA_TYPE)) + @SETNN + ', '
                                 ELSE @RSQL + RTRIM(LTRIM(COLUMN_NAME)) + ' '
                                      + RTRIM(LTRIM(DATA_TYPE)) + @SETNN + ')'
                               END
                FROM   #COLDETAILS
                WHERE  ID = @CLMINDEX

                SET @CLMINDEX = @CLMINDEX + 1
            
            END
      END

    --PRIMARY KEY ADD
    SELECT @RSQL = @RSQL + CHAR(13) + ' ALTER TABLE ' + @TABLENAME
                   + ' ADD PRIMARY KEY CLUSTERED ( ' + CHAR(13)

    SELECT @RSQL = @RSQL + REPLACE(@OTHERKEYS, '###', ' ASC,')
                   + @TABLEKEY + ' ASC ' + CHAR(13)

    SELECT @RSQL = @RSQL
                   + ')WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]'
                   + CHAR(13)

    -- FOREIGN KEY	
    SELECT @OTHERFOREIGNKEYS = LEFT(@OTHERKEYS, LEN(@OTHERKEYS) - 3)

    SELECT @RSQL = @RSQL + ' ALTER TABLE ' + @TABLENAME
                   + '  WITH CHECK ADD FOREIGN KEY('
                   + REPLACE(@OTHERFOREIGNKEYS, '###', ',') + ')'
                   + CHAR(13)

    SELECT @RSQL = @RSQL + 'REFERENCES ' + @PARENTOBJECTNAME + ' ( '
                   + REPLACE(@OTHERFOREIGNKEYS, '###', ',')
                   + ') ON DELETE CASCADE ' + CHAR(13)

    --PRINT @RSQL
    --SELECT @RSQL
    DROP TABLE #COLDETAILS

GO

--THIS SP IS CALLED VIA OTHER SP SO EXPECTS TEMPTABLE TO BE THERE WITH FOLLOWING STRUCTURE
--CREATE TABLE #QUERIES(ID INT IDENTITY,QUERY TEXT)
DDLDropProcedure 'spu_UML_Script'
GO

CREATE PROCEDURE spu_UML_Script @UMLCODE     VARCHAR(20),
                                @CHILDCODE   VARCHAR(20)=NULL,
                                @NEWPARENTID VARCHAR(20)=NULL OUTPUT
AS
    DECLARE @PARENTID INT
    DECLARE @PARENTCODE VARCHAR(20)
    DECLARE @CURRENTHEADERID INT
    DECLARE @PARENTOFCAPTION VARCHAR(10)

    SELECT @CURRENTHEADERID = GIS_USER_DEF_HEADER_ID,
           @PARENTID = ISNULL(PARENT, -1)
    FROM   GIS_USER_DEF_HEADER
    WHERE  CODE = @UMLCODE

    IF @PARENTID > 0
       AND @NEWPARENTID IS NULL
      BEGIN
          SET @PARENTOFCAPTION=CONVERT(VARCHAR(10), @PARENTID)

          SELECT @PARENTCODE = CODE
          FROM   GIS_USER_DEF_HEADER
          WHERE  GIS_USER_DEF_HEADER_ID = @PARENTID

          EXECUTE spu_UML_Script
            @PARENTCODE,
            @UMLCODE,
            @NEWPARENTID OUTPUT
      END

    SET @PARENTOFCAPTION= ISNULL(@CHILDCODE, '')

    INSERT INTO #QUERIES
    SELECT '----**THIS SCRIPT IS GENERATED FOR UML '
           + RTRIM(LTRIM(@UMLCODE)) + ' **------'

    INSERT INTO #QUERIES
    SELECT 'DECLARE @CAPTION'
           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))
           + @PARENTOFCAPTION + ' INT ' + CHAR(13)
           + 'DECLARE @ID'
           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))
           + @PARENTOFCAPTION + ' INT ' + CHAR(13)
           + 'IF NOT EXISTS(SELECT GIS_USER_DEF_HEADER_ID FROM GIS_USER_DEF_HEADER WHERE CODE='''
           + RTRIM(LTRIM(@UMLCODE)) + ''') ' + CHAR(13)
           + 'BEGIN ' + CHAR(13)
           + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')
           + ''', @CAPTION'
           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))
           + @PARENTOFCAPTION + ' OUTPUT ' + CHAR(13)
           + 'INSERT INTO GIS_USER_DEF_HEADER '
           + CHAR(13) + 'VALUES(@CAPTION'
           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))
           + @PARENTOFCAPTION + ','''
           + ISNULL(CONVERT(VARCHAR(50), CODE), 'NULL')
           + ''','''
           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')
           + ''','
           + ISNULL(CONVERT(VARCHAR(50), IS_DELETED), 'NULL')
           + ','''
           + ISNULL(CONVERT(VARCHAR(50), EFFECTIVE_DATE), 'NULL')
           + ''','
           + ISNULL(CONVERT(VARCHAR(50), @NEWPARENTID), '-1')
           + ','
           + ISNULL(CONVERT(VARCHAR(50), SYSTEM_GENERATED), '0')
           + ') ' + CHAR(13) + ' END ' + CHAR(13) + 'SELECT @ID'
           + RTRIM(LTRIM(@CURRENTHEADERID))
           + @PARENTOFCAPTION
           + ' = GIS_USER_DEF_HEADER_ID FROM GIS_USER_DEF_HEADER WHERE CODE='''
           + @UMLCODE + ''''
    FROM   GIS_USER_DEF_HEADER
    WHERE  GIS_USER_DEF_HEADER_ID = @CURRENTHEADERID

    INSERT INTO #QUERIES
    SELECT 'DECLARE @DETAILCAPTION'
           + RTRIM(LTRIM(GIS_USER_DEF_DETAIL_ID))
           + @PARENTOFCAPTION + ' INT ' + CHAR(13)
           + 'IF NOT EXISTS(SELECT GIS_USER_DEF_DETAIL_ID FROM GIS_USER_DEF_DETAIL WHERE GIS_USER_DEF_HEADER_ID = @ID'
           + RTRIM(LTRIM(GIS_USER_DEF_HEADER_ID))
           + @PARENTOFCAPTION + ' AND  CODE = ''' + CODE
           + ''') ' + CHAR(13) + 'BEGIN ' + CHAR(13)
           + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')
           + ''', @DETAILCAPTION'
           + RTRIM(LTRIM(GIS_USER_DEF_DETAIL_ID))
           + @PARENTOFCAPTION + ' OUTPUT ' + CHAR(13)
           + 'INSERT INTO GIS_USER_DEF_DETAIL VALUES(@ID'
           + RTRIM(LTRIM(@CURRENTHEADERID))
           + ',@DETAILCAPTION'
           + RTRIM(LTRIM(GIS_USER_DEF_DETAIL_ID))
           + @PARENTOFCAPTION + ','''
           + ISNULL(CONVERT(VARCHAR(50), CODE), 'NULL')
           + ''','''
           + REPLACE(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL'), '''', '')
           + ''','
           + ISNULL(CONVERT(VARCHAR(50), IS_DELETED), 'NULL')
           + ','''
           + ISNULL(CONVERT(VARCHAR(50), EFFECTIVE_DATE), 'NULL')
           + ''','
           + ISNULL(CONVERT(VARCHAR(50), -1), 'NULL')
           + ',NULL,'
           + ISNULL(CONVERT(VARCHAR(50), SYSTEM_GENERATED), '0')
           + ') ' + CHAR(13) + --PUTTING LAST COLUMN NULL AS OF NOW 
           ' END ' + CHAR(13)
    FROM   GIS_USER_DEF_DETAIL
    WHERE  GIS_USER_DEF_HEADER_ID = @CURRENTHEADERID

    SELECT @NEWPARENTID = '@ID' + RTRIM(LTRIM(@CURRENTHEADERID))
                          + @PARENTOFCAPTION

    RETURN

GO

DDLDropProcedure 'spu_GIS_ExportMPDataModel'
GO

CREATE PROCEDURE spu_GIS_ExportMPDataModel @sDataModelCode     VARCHAR(10),
                                           @COPYPRODUCTANDRISK INT=0,
                                           @CAPTURESCREENS     INT=1
AS
    
    CREATE TABLE #QUERIES
      (
         ID    INT IDENTITY,
         QUERY TEXT
      )

    INSERT INTO #QUERIES
    SELECT '--' + @sDataModelCode

    IF @CAPTURESCREENS = 1
      BEGIN
          DECLARE @sScreenCodes VARCHAR(MAX)

          SELECT @sScreenCodes = COALESCE(@sScreenCodes + ''',''', '')
                                 + RTRIM(GISS.CODE)
          FROM   GIS_SCREEN GISS
                 JOIN GIS_DATA_MODEL GISDM
                   ON GISS.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
          WHERE  GISDM.CODE = @sDataModelCode

          SET @sScreenCodes = '''' + @sScreenCodes + ''''

          INSERT INTO #QUERIES
          SELECT 'DECLARE @sScreenCodes VARCHAR(MAX) '
                 + CHAR(13)
                 + 'DECLARE @sErrorMessage VARCHAR(MAX) '
                 + CHAR(13)
                 + 'IF EXISTS(SELECT NULL FROM GIS_SCREEN GISS JOIN GIS_DATA_MODEL GISDM ON GISS.GIS_DATA_MODEL_ID =GISDM.GIS_DATA_MODEL_ID WHERE GISDM.CODE<>'''
                 + @sDataModelCode
                 + ''' AND RTRIM(GISS.CODE) IN ('
                 + @sScreenCodes + ')) ' + CHAR(13) + 'BEGIN '
                 + CHAR(13)
                 + 'SELECT @sScreenCodes = COALESCE(@sScreenCodes + '','','''') + RTRIM(GISS.code) FROM GIS_SCREEN GISS JOIN GIS_DATA_MODEL GISDM ON GISS.GIS_DATA_MODEL_ID =GISDM.GIS_DATA_MODEL_ID WHERE GISDM.CODE<>'''
                 + @sDataModelCode
                 + ''' AND RTRIM(GISS.CODE) IN ('
                 + @sScreenCodes + ') ' + CHAR(13)
                 + 'SET @sErrorMessage = @sScreenCodes + '' Screen codes already exists, Please rename first and then import again.'' '
                 + CHAR(13)
                 + 'RAISERROR (@sErrorMessage,16,1); '
                 + CHAR(13) + 'RETURN; ' + CHAR(13) + 'END '
      END

    DECLARE @CAPTUREOBJECTS INT

    SET @CAPTUREOBJECTS=1 --ALWAYS CAPTURE OBJECTS
    SET @sDataModelCode=RTRIM(LTRIM(@sDataModelCode))

    DECLARE @MAXCNT INT
    DECLARE @INDEX INT

    --**GET DETAILS OF ASSOCIATED UMLS***--
    CREATE TABLE #UML
      (
         ID      INT IDENTITY,
         UMLID   INT,
         UMLCODE VARCHAR(10)
      )

    INSERT INTO #UML
    SELECT DISTINCT GISUDH.GIS_USER_DEF_HEADER_ID,
                    GISUDH.CODE
    FROM   GIS_USER_DEF_HEADER GISUDH
           JOIN GIS_PROPERTY GISP
             ON GISUDH.GIS_USER_DEF_HEADER_ID = CONVERT(INT, GISP.SPECIALS_TYPE_REFERENCE)
           JOIN GIS_OBJECT GISO
             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
    WHERE  GISDM.CODE = @sDataModelCode           
           AND GISP.SPECIALS_TYPE = 6
           AND ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 'NULL') <> 'NULL'
    ORDER  BY GISUDH.GIS_USER_DEF_HEADER_ID ASC

    --**GET DETAILS OF ASSOCIATED UDLS***--
    CREATE TABLE #UDL
      (
         ID      INT IDENTITY,
         UDLNAME VARCHAR(255)
      )

    INSERT INTO #UDL
    SELECT DISTINCT GISP.SPECIALS_TYPE_REFERENCE
    FROM   GIS_PROPERTY GISP
           JOIN GIS_OBJECT GISO
             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
    WHERE  GISDM.CODE = @sDataModelCode
           AND GISP.SPECIALS_TYPE = 2
           AND ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 'NULL') <> 'NULL'
           
    INSERT INTO #UDL
    SELECT DISTINCT REPLACE(GFM.ViewName,'UDL_','')
    FROM   GIS_FIND_MAPPING GFM
		   JOIN GIS_PROPERTY GISP
			 ON GISP.GIS_PROPERTY_ID = GFM.GIS_PROPERTY_ID
           JOIN GIS_OBJECT GISO
             ON GISO.GIS_OBJECT_ID = GFM.GIS_OBJECT_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
    WHERE  GISDM.CODE = @sDataModelCode
           

    -------------**************** GET DETAILS OF OBJECTS ************------------------------
    CREATE TABLE #GISOBJECTS
      (
         ID                       INT IDENTITY,
         OBJECT_NAME              VARCHAR(100),
         TABLE_NAME               VARCHAR(100),
         MAX_INSTANCES            VARCHAR (10),
         IS_QUOTE_OBJECT          VARCHAR (10),
         PARENT_TABLE_NAME        VARCHAR(100),
         POLARIS_OBJECT_ID        VARCHAR (10),
         IS_SELECTABLE_FOR_SCREEN VARCHAR (10),
         IS_NON_GIS               VARCHAR (10),
         EDIT_FLAGS               VARCHAR (10)
      )

    INSERT INTO #GISOBJECTS
    SELECT GISO.OBJECT_NAME,
           GISO.TABLE_NAME,
           ISNULL(CONVERT(VARCHAR(10), GISO.MAX_INSTANCES), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISO.IS_QUOTE_OBJECT), 'NULL'),
           GISPO.TABLE_NAME,
           ISNULL(CONVERT(VARCHAR(10), GISO.POLARIS_OBJECT_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISO.IS_SELECTABLE_FOR_SCREEN), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISO.IS_NON_GIS), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISO.EDIT_FLAGS), 'NULL')
    FROM   GIS_OBJECT GISO
           JOIN GIS_DATA_MODEL GISDM
             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
           JOIN GIS_OBJECT GISPO
             ON GISO.PARENT_OBJECT_ID = GISPO.GIS_OBJECT_ID
    WHERE  GISDM.CODE = @sDataModelCode
    ORDER  BY GISO.PARENT_OBJECT_ID ASC

    -------------************* GET DETAILS OF PROPERTIES ************-----------------------
    CREATE TABLE #GISPROPERTIES
      (
         ID                      INT IDENTITY,
         GIS_OBJECT_TABLE_NAME   VARCHAR(100),
         PROPERTY_NAME           VARCHAR(100),
         COLUMN_NAME             VARCHAR(100),
         DATA_TYPE               VARCHAR(10),--INT,
         IS_INPUT_PROPERTY       VARCHAR(10),-- INT,
         IS_IDENTIFYING_PROPERTY VARCHAR(10),--INT,
         IS_PRIMARY_KEY          VARCHAR(10),--INT,
         POLARIS_PROPERTY_ID     VARCHAR(10),-- IT IS ALWAYS NULL
         IS_DELETED              VARCHAR(10),--INT,
         IS_SEARCH_PROPERTY      VARCHAR(10),--INT,
         INDEX_LINKING_ID        VARCHAR(10),--INT,
         EDIT_FLAGS              VARCHAR(10),--INT,
         SPECIALS_TYPE           VARCHAR(10),--INT,
         SPECIALS_TYPE_REFERENCE VARCHAR(100)
      )

    INSERT INTO #GISPROPERTIES
    SELECT GISO.TABLE_NAME,
           GISP.PROPERTY_NAME,
           GISP.COLUMN_NAME,
           ISNULL(CONVERT(VARCHAR(10), GISP.DATA_TYPE), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.IS_INPUT_PROPERTY), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.IS_IDENTIFYING_PROPERTY), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.IS_PRIMARY_KEY), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.POLARIS_PROPERTY_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.IS_DELETED), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.IS_SEARCH_PROPERTY), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.INDEX_LINKING_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.EDIT_FLAGS), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISP.SPECIALS_TYPE), 'NULL'),
           ISNULL(CONVERT(VARCHAR(100), SPECIALS_TYPE_REFERENCE), 'NULL')
    FROM   GIS_PROPERTY GISP
           JOIN GIS_OBJECT GISO
             ON GISP.GIS_OBJECT_ID = GISO.GIS_OBJECT_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISO.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
    WHERE  GISDM.CODE = @sDataModelCode
    ORDER  BY GISO.GIS_OBJECT_ID ASC

    ------------*********** UPDATE PROPERTY TABLE BASED ON UML ***********------------------ 
    --THIS WILL REPLACE THE ID OF UML IN UPDATE WITH CODE INSTEAD WHICH WILL BE LATER USED TO UPDATE 
    UPDATE #GISPROPERTIES
    SET    SPECIALS_TYPE_REFERENCE = #UML.UMLCODE
    FROM   #GISPROPERTIES GISP
           JOIN #UML
             ON #UML.UMLID = CONVERT(INT, ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 0))
    WHERE  GISP.SPECIALS_TYPE = 6
           AND ISNULL(GISP.SPECIALS_TYPE_REFERENCE, 'NULL') <> 'NULL'

    --------------************* GET DETAILS OF SCREENS ************------------------------
    CREATE TABLE #GISSCREEN
      (
         ID                   INT IDENTITY,         
         CODE                 VARCHAR(20),
         DESCRIPTION          VARCHAR(100),
         IS_DELETED           VARCHAR(10),
         EFFECTIVE_DATE       VARCHAR(50),
         PARENT_SCREEN_CODE   VARCHAR(20),
         IS_MAINTAINABLE      VARCHAR(10),
         GIS_DATA_MODEL_ID    VARCHAR(10),
         SCRIPT_DEFAULTS      VARCHAR(MAX),
         SCRIPT_DYNAMIC_LOGIC VARCHAR(MAX),
         SCREEN_TYPE          VARCHAR(10),
         SCREEN_HEIGHT        VARCHAR(10),
         SCREEN_WIDTH         VARCHAR(10),
         PRODUCT_OPTION       VARCHAR(10)
      )

    --'INSERT PARENT SCREENS FIRST
    INSERT INTO #GISSCREEN
    SELECT GISS.CODE,
           GISS.DESCRIPTION,
           ISNULL(CONVERT(VARCHAR(10), GISS.IS_DELETED), 'NULL'),
           ISNULL(CONVERT(VARCHAR(50), GISS.EFFECTIVE_DATE), 'NULL'),
           'NULL',--NO PARENT CODE HERE
           ISNULL(CONVERT(VARCHAR(10), GISS.IS_MAINTAINABLE), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.GIS_DATA_MODEL_ID), 'NULL'),
           ISNULL(GISS.SCRIPT_DEFAULTS, 'NULL'),
           ISNULL(GISS.SCRIPT_DYNAMIC_LOGIC, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_TYPE), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_HEIGHT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.PRODUCT_OPTION), 'NULL')
    FROM   GIS_SCREEN GISS
           JOIN GIS_DATA_MODEL GISDM
             ON GISS.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
    WHERE  GISDM.CODE = @sDataModelCode
           AND GISS.PARENT_ID IS NULL
    ORDER  BY GISS.GIS_SCREEN_ID ASC

    --'INSERT CHILD SCREENS 
    INSERT INTO #GISSCREEN
    SELECT GISS.CODE,
           GISS.DESCRIPTION,
           ISNULL(CONVERT(VARCHAR(10), GISS.IS_DELETED), 'NULL'),
           ISNULL(CONVERT(VARCHAR(50), GISS.EFFECTIVE_DATE), 'NULL'),
           GISSP.CODE,
           ISNULL(CONVERT(VARCHAR(10), GISS.IS_MAINTAINABLE), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.GIS_DATA_MODEL_ID), 'NULL'),
           ISNULL(GISS.SCRIPT_DEFAULTS, 'NULL'),
           ISNULL(GISS.SCRIPT_DYNAMIC_LOGIC, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_TYPE), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_HEIGHT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.SCREEN_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISS.PRODUCT_OPTION), 'NULL')
    FROM   GIS_SCREEN GISS
           JOIN GIS_DATA_MODEL GISDM
             ON GISS.GIS_DATA_MODEL_ID = GISDM.GIS_DATA_MODEL_ID
           JOIN GIS_SCREEN GISSP
             ON GISS.PARENT_ID = GISSP.GIS_SCREEN_ID
    WHERE  GISDM.CODE = @sDataModelCode
    ORDER  BY GISS.GIS_SCREEN_ID ASC

    UPDATE #GISSCREEN
    SET    SCRIPT_DEFAULTS = REPLACE(SCRIPT_DEFAULTS, '''', '^')
    WHERE  SCRIPT_DEFAULTS <> 'NULL'

    UPDATE #GISSCREEN
    SET    SCRIPT_DYNAMIC_LOGIC = REPLACE(SCRIPT_DYNAMIC_LOGIC, '''', '^')
    WHERE  SCRIPT_DYNAMIC_LOGIC <> 'NULL'

    -------------******** GET DETAILS OF SCREEN DETAILS *****------------------------------
    CREATE TABLE #GISSCREENDETAIL
      (
         ID                       INT IDENTITY,
         GIS_SCREEN_CODE          VARCHAR(10),
         SCREEN_DETAIL_CNT        VARCHAR(10),
         GIS_OBJECT_TABLE_NAME    VARCHAR(200),
         GIS_PROPERTY_COLUMN_NAME VARCHAR(200),
         IS_FRAME                 VARCHAR(10),
         TAB_NUMBER               VARCHAR(10),
         CAPTION                  VARCHAR(255),
         ITEM_TOP                 VARCHAR(10),
         ITEM_LEFT                VARCHAR(10),
         ITEM_HEIGHT              VARCHAR(10),
         ITEM_WIDTH               VARCHAR(10),
         COLUMN_WIDTH             VARCHAR(10),
         PRE_QUOTE_REQUIREMENT    VARCHAR(10),
         POST_QUOTE_REQUIREMENT   VARCHAR(10),
         PURCHASE_REQUIREMENT     VARCHAR(10),
         PARENT_ID                VARCHAR(10),
         HELP_TEXT                VARCHAR(255),
         DEFAULT_OBJECT_ID        VARCHAR(10),
         DEFAULT_PROPERTY_ID      VARCHAR(10),
         IS_VALUATION             VARCHAR(10),
         IS_RATE_AND_PREMIUM      VARCHAR(10),
         CHILD_SCREEN_ID          VARCHAR(10),
         PMFORMAT                 VARCHAR(10),
         COLUMN_POSITION          VARCHAR(10),
         TAB_SET_INDEX            VARCHAR(10),
         DATA_MODEL_TYPE          VARCHAR(10),
         INSERT_TYPE              VARCHAR(10)-- 0 = PROPERTY NULL & OBJECT NULL 1= ONLY PROPERTY NULL 2 = NEITHER NULL
      )

    -- INSERT DETIALS WITH PROPERTY NULL & OBJECT NULL
    INSERT INTO #GISSCREENDETAIL
    SELECT GISS.CODE,
           ISNULL(CONVERT(VARCHAR(10), GISSD.SCREEN_DETAIL_CNT), 'NULL'),
           'NULL',
           ISNULL(CONVERT(VARCHAR(10), GISSD.GIS_PROPERTY_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_FRAME), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_NUMBER), 'NULL'),
           ISNULL(GISSD.CAPTION, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_TOP), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_LEFT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_HEIGHT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PRE_QUOTE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.POST_QUOTE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PURCHASE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PARENT_ID), 'NULL'),
           ISNULL(GISSD.HELP_TEXT, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_OBJECT_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_PROPERTY_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_VALUATION), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_RATE_AND_PREMIUM), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.CHILD_SCREEN_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PMFORMAT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_POSITION), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_SET_INDEX), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DATA_MODEL_TYPE), 'NULL'),
           0
    FROM   GIS_SCREEN_DETAIL GISSD
           JOIN GIS_SCREEN GISS
             ON GISSD.GIS_SCREEN_ID = GISS.GIS_SCREEN_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISDM.GIS_DATA_MODEL_ID = GISS.GIS_DATA_MODEL_ID
    WHERE  GISDM.CODE = @sDataModelCode
           AND ISNULL(GISSD.GIS_PROPERTY_ID, 0) < 1
           AND GISSD.GIS_OBJECT_ID IS NULL
    ORDER  BY GISSD.GIS_SCREEN_ID ASC

    -- INSERT DETIALS WITH ONLY PROPERTY NULL
    INSERT INTO #GISSCREENDETAIL
    SELECT GISS.CODE,
           ISNULL(CONVERT(VARCHAR(10), GISSD.SCREEN_DETAIL_CNT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(200), GISO.TABLE_NAME), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.GIS_PROPERTY_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_FRAME), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_NUMBER), 'NULL'),
           ISNULL(GISSD.CAPTION, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_TOP), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_LEFT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_HEIGHT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PRE_QUOTE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.POST_QUOTE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PURCHASE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PARENT_ID), 'NULL'),
           ISNULL(GISSD.HELP_TEXT, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_OBJECT_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_PROPERTY_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_VALUATION), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_RATE_AND_PREMIUM), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.CHILD_SCREEN_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PMFORMAT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_POSITION), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_SET_INDEX), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DATA_MODEL_TYPE), 'NULL'),
           1
    FROM   GIS_SCREEN_DETAIL GISSD
           JOIN GIS_SCREEN GISS
             ON GISSD.GIS_SCREEN_ID = GISS.GIS_SCREEN_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISDM.GIS_DATA_MODEL_ID = GISS.GIS_DATA_MODEL_ID
           JOIN GIS_OBJECT GISO
             ON GISO.GIS_OBJECT_ID = GISSD.GIS_OBJECT_ID
    WHERE  GISDM.CODE = @sDataModelCode
           AND ISNULL(GISSD.GIS_PROPERTY_ID, 0) < 1
    ORDER  BY GISSD.GIS_SCREEN_ID ASC

    -- INSERT DETIALS WITH  2 = NEITHER NULL
    INSERT INTO #GISSCREENDETAIL
    SELECT GISS.CODE,
           ISNULL(CONVERT(VARCHAR(10), GISSD.SCREEN_DETAIL_CNT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(200), GISO.TABLE_NAME), 'NULL'),
           ISNULL(CONVERT(VARCHAR(200), GISP.COLUMN_NAME), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_FRAME), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_NUMBER), 'NULL'),
           ISNULL(GISSD.CAPTION, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_TOP), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_LEFT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_HEIGHT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.ITEM_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_WIDTH), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PRE_QUOTE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.POST_QUOTE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PURCHASE_REQUIREMENT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PARENT_ID), 'NULL'),
           ISNULL(GISSD.HELP_TEXT, 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_OBJECT_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DEFAULT_PROPERTY_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_VALUATION), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.IS_RATE_AND_PREMIUM), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.CHILD_SCREEN_ID), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.PMFORMAT), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.COLUMN_POSITION), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.TAB_SET_INDEX), 'NULL'),
           ISNULL(CONVERT(VARCHAR(10), GISSD.DATA_MODEL_TYPE), 'NULL'),
           2
    FROM   GIS_SCREEN_DETAIL GISSD
           JOIN GIS_SCREEN GISS
             ON GISSD.GIS_SCREEN_ID = GISS.GIS_SCREEN_ID
           JOIN GIS_DATA_MODEL GISDM
             ON GISDM.GIS_DATA_MODEL_ID = GISS.GIS_DATA_MODEL_ID
           JOIN GIS_OBJECT GISO
             ON GISO.GIS_OBJECT_ID = GISSD.GIS_OBJECT_ID
           JOIN GIS_PROPERTY GISP
             ON GISP.GIS_PROPERTY_ID = GISSD.GIS_PROPERTY_ID
    WHERE  GISDM.CODE = @sDataModelCode
    ORDER  BY GISSD.GIS_SCREEN_ID ASC

    -------------************************************************------------------------
    IF @CAPTUREOBJECTS <> 0
      BEGIN --A
          --**********NOW MAKE QUERY*****************	
          SELECT @INDEX = 1,
                 @MAXCNT = MAX(ID)
          FROM   #GISOBJECTS

          IF @MAXCNT > 0
            BEGIN --B
                DECLARE @ADDDATAMODELSQL VARCHAR(MAX)

                EXECUTE spu_CreatePolicyBinder
                  @sDataModelCode,
                  @ADDDATAMODELSQL OUTPUT

                INSERT INTO #QUERIES
                SELECT 'DECLARE @DATAMODELID INT ' + CHAR(13)
                       + 'DECLARE @OBJECTCNT INT ' + CHAR(13)
                       + 'DECLARE @PARENTOBJECTID INT ' + CHAR(13)
                       + 'DECLARE @DMCAPTIONID INT ' + CHAR(13)

                INSERT INTO #QUERIES
                SELECT 'IF NOT EXISTS(SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''
                       + @sDataModelCode + ''') ' + CHAR(13) + 'BEGIN '
                       + CHAR(13)
                       + 'SELECT @DATAMODELID = MAX(GIS_DATA_MODEL_ID)+1 FROM GIS_DATA_MODEL '
                       + CHAR(13)
                       + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
					   + RTRIM(LTRIM(@sDataModelCode))
					   + ''', @DMCAPTIONID OUTPUT ' + CHAR(13)
                       + 'INSERT INTO GIS_DATA_MODEL (GIS_DATA_MODEL_ID,
						CODE,
						CAPTION_ID,
						DESCRIPTION,
						IS_DELETED,
						EFFECTIVE_DATE,
						GIS_DATA_MODEL_TYPE_ID,
						PRODUCT_OPTION)	VALUES(@DATAMODELID,'''
                       + RTRIM(LTRIM(ISNULL(CONVERT(VARCHAR(50), CODE), 'NULL')))
                       + ''',@DMCAPTIONID,'''
                       + RTRIM(LTRIM(ISNULL(CONVERT(VARCHAR(50), DESCRIPTION), 'NULL')))
                       + ''','
                       + ISNULL(CONVERT(VARCHAR(50), IS_DELETED), 'NULL')
                       + ','''
                       + ISNULL(CONVERT(VARCHAR(50), EFFECTIVE_DATE), 'NULL')
                       + ''','
                       + ISNULL(CONVERT(VARCHAR(50), GIS_DATA_MODEL_TYPE_ID), 'NULL')
                       + ','
                       + ISNULL(CONVERT(VARCHAR(50), PRODUCT_OPTION), 'NULL')
                       + ') ' + CHAR(13) + @ADDDATAMODELSQL + CHAR(13)
                       + ' END ' + CHAR(13)
                FROM   GIS_DATA_MODEL
                WHERE  CODE = @sDataModelCode

                INSERT INTO #QUERIES
                SELECT 'SELECT @DATAMODELID = GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WITH(NOLOCK) WHERE CODE = '''
                       + @sDataModelCode + ''' ' + CHAR(13)
                       + 'SELECT @OBJECTCNT = MAX(GIS_OBJECT_ID) FROM GIS_OBJECT WITH(NOLOCK) '
                       + CHAR(13)

                WHILE( @INDEX <= @MAXCNT )
                  BEGIN --C		
                      DECLARE @TABLENAME VARCHAR(100)
                      DECLARE @ADDOBJECTSQL VARCHAR(MAX)

                      SELECT @TABLENAME = TABLE_NAME
                      FROM   #GISOBJECTS
                      WHERE  ID = @INDEX

                      EXECUTE spu_CreateGISObject
                        @TABLENAME,
                        @ADDOBJECTSQL OUTPUT

                      INSERT INTO #QUERIES
                      SELECT 'IF (SELECT GIS_OBJECT_ID FROM GIS_OBJECT WITH(NOLOCK) WHERE TABLE_NAME = '''
                             + TABLE_NAME + ''') IS NULL ' + CHAR(13) + 'BEGIN '
                             + CHAR(13)
                             + 'SET @OBJECTCNT = @OBJECTCNT + 1 '
                             + CHAR(13)
                             + 'SELECT TOP 1 @PARENTOBJECTID =GIS_OBJECT_ID FROM GIS_OBJECT WITH (NOLOCK) WHERE TABLE_NAME = '''
                             + PARENT_TABLE_NAME + ''' ' + CHAR(13)
                             + 'INSERT INTO GIS_OBJECT (GIS_OBJECT_ID,
								GIS_DATA_MODEL_ID,
								OBJECT_NAME,
								TABLE_NAME,
								MAX_INSTANCES,
								IS_QUOTE_OBJECT,
								PARENT_OBJECT_ID,
								POLARIS_OBJECT_ID,
								IS_SELECTABLE_FOR_SCREEN,
								IS_NON_GIS,
								EDIT_FLAGS) ' + CHAR(13)
                             + 'VALUES(@OBJECTCNT,@DATAMODELID,'''
                             + OBJECT_NAME + ''',''' + TABLE_NAME + ''','
                             + MAX_INSTANCES + ',' + IS_QUOTE_OBJECT
                             + ',@PARENTOBJECTID,' + 'NULL,'
                             + IS_SELECTABLE_FOR_SCREEN + ',' + IS_NON_GIS + ','
                             + EDIT_FLAGS + ') ' + CHAR(13) + @ADDOBJECTSQL
                             + CHAR(13) + ' END ' + CHAR(13) + CHAR(13)
                      FROM   #GISOBJECTS
                      WHERE  ID = @INDEX

                      SET @INDEX=@INDEX + 1
                  END --C
            END --B
      END--A	

    SELECT @INDEX = 1,
           @MAXCNT = MAX(ID)
    FROM   #GISPROPERTIES

    IF @MAXCNT > 0
      BEGIN --A2
          INSERT INTO #QUERIES
          SELECT ' '

          INSERT INTO #QUERIES
          SELECT '--******************** THIS BIT OF SCRIPT WILL INSERT DATA OF PROPERTIES FOR DATAMODEL ( '
                 + @sDataModelCode
                 + ') *******************'

          INSERT INTO #QUERIES
          SELECT 'DECLARE @PROPERTYCNT INT ' + CHAR(13)
                 + 'DECLARE @OBJECTID INT ' + CHAR(13)
                 + 'SELECT @PROPERTYCNT = MAX(GIS_PROPERTY_ID) FROM GIS_PROPERTY WITH(NOLOCK) '
                 + CHAR(13)

          WHILE( @INDEX <= @MAXCNT )
            BEGIN --B2
                DECLARE @PROPCOLNAME VARCHAR(100)
                DECLARE @PROPTABLENAME VARCHAR(100)
                DECLARE @ADDPROPERTYSQL VARCHAR(300)

                SELECT @PROPTABLENAME = GIS_OBJECT_TABLE_NAME,
                       @PROPCOLNAME = COLUMN_NAME
                FROM   #GISPROPERTIES
                WHERE  ID = @INDEX

                EXECUTE spu_CreateGISProperty
                  @PROPTABLENAME,
                  @PROPCOLNAME,
                  @ADDPROPERTYSQL OUTPUT

                INSERT INTO #QUERIES
                SELECT 'SELECT TOP 1 @OBJECTID =GIS_OBJECT_ID FROM GIS_OBJECT WITH (NOLOCK) WHERE TABLE_NAME = '''
                       + GIS_OBJECT_TABLE_NAME + ''' ' + CHAR(13)
                       + 'IF (SELECT GIS_PROPERTY_ID FROM GIS_PROPERTY WITH (NOLOCK) WHERE COLUMN_NAME = '''
                       + COLUMN_NAME
                       + ''' AND GIS_OBJECT_ID=@OBJECTID) IS NULL '
                       + CHAR(13) + 'BEGIN ' + CHAR(13)
                       + 'SET @PROPERTYCNT = @PROPERTYCNT + 1 '
                       + CHAR(13) + 'INSERT INTO GIS_PROPERTY (GIS_PROPERTY_ID,GIS_OBJECT_ID,
						PROPERTY_NAME,
						COLUMN_NAME,
						DATA_TYPE,
						IS_INPUT_PROPERTY,
						IS_IDENTIFYING_PROPERTY,
						IS_PRIMARY_KEY,
						POLARIS_PROPERTY_ID,
						IS_DELETED,
						IS_SEARCH_PROPERTY,
						INDEX_LINKING_ID,
						EDIT_FLAGS,
						SPECIALS_TYPE,
						SPECIALS_TYPE_REFERENCE) '
                       + CHAR(13)
                       + 'VALUES(@PROPERTYCNT,@OBJECTID,'''
                       + PROPERTY_NAME + ''',''' + COLUMN_NAME + ''','
                       + DATA_TYPE + ',' + IS_INPUT_PROPERTY + ','
                       + IS_IDENTIFYING_PROPERTY + ',' + IS_PRIMARY_KEY
                       + ',' + POLARIS_PROPERTY_ID + ',' + IS_DELETED + ','
                       + IS_SEARCH_PROPERTY + ',' + INDEX_LINKING_ID + ','
                       + EDIT_FLAGS + ',' + SPECIALS_TYPE + ','''
                       + SPECIALS_TYPE_REFERENCE + ''')' + ' END '
                       + CHAR(13) + CHAR(13)
                FROM   #GISPROPERTIES
                WHERE  ID = @INDEX

                SET @INDEX=@INDEX + 1
            END --B2
      END--A2

    -----------******************************************************-----------------------
    IF @CAPTURESCREENS <> 0
      BEGIN --A3	
          --SCREEN INFORMATION
          SELECT @INDEX = 1,
                 @MAXCNT = MAX(ID)
          FROM   #GISSCREEN

          IF @MAXCNT > 0
            BEGIN --B3
                INSERT INTO #QUERIES
                SELECT ' '

                INSERT INTO #QUERIES
                SELECT '--*********************************** THIS BIT OF SCRIPT WILL INSERT DATA IN GIS SCREEN IF NEEDED FOR DATAMODEL ( '
                       + @sDataModelCode
                       + ') ***********************************'

                INSERT INTO #QUERIES
                SELECT 'DECLARE @GISSSCREENCNT INT ' + CHAR(13)
                       + 'DECLARE @GISSCAPTIONID INT ' + CHAR(13)
                       + 'DECLARE @GISSPARENTID INT ' + CHAR(13)
                       + 'SELECT @GISSSCREENCNT = MAX(GIS_SCREEN_ID) FROM GIS_SCREEN '
                       + CHAR(13)

                BEGIN --B3A
                    WHILE ( @INDEX <= @MAXCNT )
                      BEGIN --B3B
                          INSERT INTO #QUERIES
                          SELECT 'IF (SELECT GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''
                                 + CODE + ''') IS NULL ' + CHAR(13) + 'BEGIN '
                                 + CHAR(13)
                                 + 'SELECT  @GISSPARENTID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE ='''
                                 + PARENT_SCREEN_CODE + ''' ' + CHAR(13)
                                 + 'EXECUTE SPU_PM_CAPTION_ID_RETURN 1, '''
                                 + DESCRIPTION + ''', @GISSCAPTIONID OUTPUT '
                                 + CHAR(13)
                                 + 'SET @GISSSCREENCNT =@GISSSCREENCNT + 1 '
                                 + CHAR(13) + 'INSERT INTO GIS_SCREEN (GIS_SCREEN_ID,CAPTION_ID,
									CODE,
									DESCRIPTION,
									IS_DELETED,
									EFFECTIVE_DATE,
									PARENT_ID,
									IS_MAINTAINABLE,
									GIS_DATA_MODEL_ID,
									SCRIPT_DEFAULTS,
									SCRIPT_DYNAMIC_LOGIC,
									SCREEN_TYPE,
									SCREEN_HEIGHT,
									SCREEN_WIDTH,
									PRODUCT_OPTION) ' + CHAR(13)
                                 + 'VALUES(@GISSSCREENCNT,@GISSCAPTIONID,'''
                                 + CODE + ''',''' + DESCRIPTION + ''','
                                 + IS_DELETED + ',''' + EFFECTIVE_DATE + ''','
                                 + '@GISSPARENTID,' + IS_MAINTAINABLE + ','
                                 + '@DATAMODELID,'''
                                 + CONVERT(VARCHAR(MAX), SCRIPT_DEFAULTS)
                                 + ''','''
                                 + CONVERT(VARCHAR(MAX), SCRIPT_DYNAMIC_LOGIC)
                                 + ''',' + SCREEN_TYPE + ',' + SCREEN_HEIGHT + ','
                                 + SCREEN_WIDTH + ',' + PRODUCT_OPTION + ') '
                                 + CHAR(13) + ' END ' + CHAR(13) + CHAR(13)
                          FROM   #GISSCREEN
                          WHERE  ID = @INDEX

                          SET @INDEX = @INDEX + 1
                      END --B3B
                END --B3A
            END --B3

          --SCREEN DETAIL INFORMATION	
          SELECT @INDEX = 1,
                 @MAXCNT = MAX(ID)
          FROM   #GISSCREENDETAIL

          IF @MAXCNT > 0
            BEGIN --C3
                INSERT INTO #QUERIES
                SELECT ' '

                INSERT INTO #QUERIES
                SELECT '--*********************************** THIS WILL INSERT DATA IN GIS SCREEN DETAIL IF NEEDED FOR DATAMODEL ( '
                       + @sDataModelCode
                       + ') ***********************************'

                INSERT INTO #QUERIES
                SELECT 'DELETE FROM GIS_SCREEN_DETAIL 	FROM GIS_SCREEN_DETAIL GISSD '
                       + CHAR(13)
                       + 'JOIN GIS_SCREEN GISS ON GISSD.GIS_SCREEN_ID=GISS.GIS_SCREEN_ID '
                       + CHAR(13)
                       + 'JOIN GIS_DATA_MODEL  GISDM ON GISDM.GIS_DATA_MODEL_ID= GISS.GIS_DATA_MODEL_ID '
                       + CHAR(13) + 'WHERE GISDM.CODE='''
                       + @sDataModelCode + ''''

                INSERT INTO #QUERIES
                SELECT ' DECLARE @GISSDSCREENID INT ' + CHAR(13)
                       + ' DECLARE @GISSDOBJECTID INT ' + CHAR(13)
                       + ' DECLARE @GISSDPROPERTYID INT ' + CHAR(13)
                       + ' DECLARE @CHILDSCREENID INT '

                BEGIN --C3A	
                    WHILE ( @INDEX <= @MAXCNT )
                      BEGIN --C3B
                          DECLARE @GIS_SCREEN_CODE AS VARCHAR(10)

                          SELECT @GIS_SCREEN_CODE = CODE
                          FROM   GIS_SCREEN
                          WHERE  GIS_SCREEN_ID IN(SELECT CONVERT(INT, CHILD_SCREEN_ID)
                                                  FROM   #GISSCREENDETAIL
                                                  WHERE  ID = @INDEX
                                                         AND CHILD_SCREEN_ID <> 'NULL')

                          INSERT INTO #QUERIES
                          SELECT 'SELECT @CHILDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''
                                 + @GIS_SCREEN_CODE + '''' + CHAR(13)

                          DECLARE @INSERTTYPE INT

                          SELECT @INSERTTYPE = INSERT_TYPE
                          FROM   #GISSCREENDETAIL
                          WHERE  ID = @INDEX

                          IF @INSERTTYPE = 0
                            BEGIN --C3BA
                                INSERT INTO #QUERIES
                                SELECT 'SELECT @GISSDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''
                                       + GIS_SCREEN_CODE
                                       + ''' AND GIS_DATA_MODEL_ID = (SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''
                                       + @sDataModelCode + ''') ' + CHAR(13)
                                       + 'INSERT INTO GIS_SCREEN_DETAIL (GIS_SCREEN_ID,
										SCREEN_DETAIL_CNT,
										GIS_OBJECT_ID,
										GIS_PROPERTY_ID,
										IS_FRAME,
										TAB_NUMBER,
										CAPTION,
										ITEM_TOP,
										ITEM_LEFT,
										ITEM_HEIGHT,
										ITEM_WIDTH,
										COLUMN_WIDTH,
										PRE_QUOTE_REQUIREMENT,
										POST_QUOTE_REQUIREMENT,
										PURCHASE_REQUIREMENT,
										PARENT_ID,
										HELP_TEXT,
										DEFAULT_OBJECT_ID,
										DEFAULT_PROPERTY_ID,
										IS_VALUATION,
										IS_RATE_AND_PREMIUM,
										CHILD_SCREEN_ID,
										PMFORMAT,
										COLUMN_POSITION,
										TAB_SET_INDEX,
										DATA_MODEL_TYPE) VALUES(@GISSDSCREENID,'
                                       + SCREEN_DETAIL_CNT + ',NULL,'
                                       + GIS_PROPERTY_COLUMN_NAME + ',' + IS_FRAME + ','
                                       + TAB_NUMBER + ',''' + REPLACE(CAPTION, '''', '^')
                                       + ''',' + ITEM_TOP + ',' + ITEM_LEFT + ',' + ITEM_HEIGHT
                                       + ',' + ITEM_WIDTH + ',' + COLUMN_WIDTH + ','
                                       + PRE_QUOTE_REQUIREMENT + ','
                                       + POST_QUOTE_REQUIREMENT + ','
                                       + PURCHASE_REQUIREMENT + ',' + PARENT_ID + ','''
                                       + HELP_TEXT + ''',' + DEFAULT_OBJECT_ID + ','
                                       + DEFAULT_PROPERTY_ID + ',' + IS_VALUATION + ','
                                       + IS_RATE_AND_PREMIUM + ',@CHILDSCREENID,'
                                       + PMFORMAT + ',' + COLUMN_POSITION + ','
                                       + TAB_SET_INDEX + ',' + DATA_MODEL_TYPE + ') '
                                       + CHAR(13)
                                FROM   #GISSCREENDETAIL
                                WHERE  ID = @INDEX
                            END --C3BA

                          IF @INSERTTYPE = 1
                            BEGIN --C3BB
                                INSERT INTO #QUERIES
                                SELECT 'SELECT TOP 1 @GISSDOBJECTID = GIS_OBJECT_ID FROM  GIS_OBJECT WHERE TABLE_NAME = '''
                                       + GIS_OBJECT_TABLE_NAME + ''' ' + CHAR(13)
                                       + 'SELECT @GISSDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''
                                       + GIS_SCREEN_CODE
                                       + '''  AND GIS_DATA_MODEL_ID = (SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''
                                       + @sDataModelCode + ''') ' + CHAR(13)
                                       + 'INSERT INTO GIS_SCREEN_DETAIL (GIS_SCREEN_ID,
										SCREEN_DETAIL_CNT,
										GIS_OBJECT_ID,
										GIS_PROPERTY_ID,
										IS_FRAME,
										TAB_NUMBER,
										CAPTION,
										ITEM_TOP,
										ITEM_LEFT,
										ITEM_HEIGHT,
										ITEM_WIDTH,
										COLUMN_WIDTH,
										PRE_QUOTE_REQUIREMENT,
										POST_QUOTE_REQUIREMENT,
										PURCHASE_REQUIREMENT,
										PARENT_ID,
										HELP_TEXT,
										DEFAULT_OBJECT_ID,
										DEFAULT_PROPERTY_ID,
										IS_VALUATION,
										IS_RATE_AND_PREMIUM,
										CHILD_SCREEN_ID,
										PMFORMAT,
										COLUMN_POSITION,
										TAB_SET_INDEX,
										DATA_MODEL_TYPE) VALUES(@GISSDSCREENID,'
                                       + SCREEN_DETAIL_CNT + ',@GISSDOBJECTID,NULL,'
                                       + IS_FRAME + ',' + TAB_NUMBER + ','''
                                       + REPLACE(CAPTION, '''', '^') + ''',' + ITEM_TOP
                                       + ',' + ITEM_LEFT + ',' + ITEM_HEIGHT + ',' + ITEM_WIDTH
                                       + ',' + COLUMN_WIDTH + ',' + PRE_QUOTE_REQUIREMENT
                                       + ',' + POST_QUOTE_REQUIREMENT + ','
                                       + PURCHASE_REQUIREMENT + ',' + PARENT_ID + ','''
                                       + HELP_TEXT + ''',' + DEFAULT_OBJECT_ID + ','
                                       + DEFAULT_PROPERTY_ID + ',' + IS_VALUATION + ','
                                       + IS_RATE_AND_PREMIUM + ',@CHILDSCREENID,'
                                       + PMFORMAT + ',' + COLUMN_POSITION + ','
                                       + TAB_SET_INDEX + ',' + DATA_MODEL_TYPE + ') '
                                       + CHAR(13)
                                FROM   #GISSCREENDETAIL
                                WHERE  ID = @INDEX
                            END ----C3BB

                          IF @INSERTTYPE = 2
                            BEGIN ----C3BC
                                INSERT INTO #QUERIES
                                SELECT 'SELECT @GISSDPROPERTYID = GIP.GIS_PROPERTY_ID ,@GISSDOBJECTID =GIO.GIS_OBJECT_ID FROM GIS_PROPERTY GIP JOIN GIS_OBJECT GIO ON GIO.GIS_OBJECT_ID=GIP.GIS_OBJECT_ID WHERE '
                                       + CHAR(13) + 'GIP.PROPERTY_NAME = '''
                                       + GIS_PROPERTY_COLUMN_NAME
                                       + ''' AND GIO.TABLE_NAME= '''
                                       + GIS_OBJECT_TABLE_NAME + ''' ' + CHAR(13)
                                       + 'SELECT @GISSDSCREENID = GIS_SCREEN_ID FROM GIS_SCREEN WHERE CODE = '''
                                       + GIS_SCREEN_CODE
                                       + '''  AND GIS_DATA_MODEL_ID = (SELECT GIS_DATA_MODEL_ID FROM GIS_DATA_MODEL WHERE CODE = '''
                                       + @sDataModelCode + ''') ' + CHAR(13)
                                       + 'INSERT INTO GIS_SCREEN_DETAIL (GIS_SCREEN_ID,
										SCREEN_DETAIL_CNT,
										GIS_OBJECT_ID,
										GIS_PROPERTY_ID,
										IS_FRAME,
										TAB_NUMBER,
										CAPTION,
										ITEM_TOP,
										ITEM_LEFT,
										ITEM_HEIGHT,
										ITEM_WIDTH,
										COLUMN_WIDTH,
										PRE_QUOTE_REQUIREMENT,
										POST_QUOTE_REQUIREMENT,
										PURCHASE_REQUIREMENT,
										PARENT_ID,
										HELP_TEXT,
										DEFAULT_OBJECT_ID,
										DEFAULT_PROPERTY_ID,
										IS_VALUATION,
										IS_RATE_AND_PREMIUM,
										CHILD_SCREEN_ID,
										PMFORMAT,
										COLUMN_POSITION,
										TAB_SET_INDEX,
										DATA_MODEL_TYPE) VALUES(@GISSDSCREENID,'
                                       + SCREEN_DETAIL_CNT
                                       + ',@GISSDOBJECTID,@GISSDPROPERTYID,'
                                       + IS_FRAME + ',' + TAB_NUMBER + ','''
                                       + REPLACE(CAPTION, '''', '^') + ''',' + ITEM_TOP
                                       + ',' + ITEM_LEFT + ',' + ITEM_HEIGHT + ',' + ITEM_WIDTH
                                       + ',' + COLUMN_WIDTH + ',' + PRE_QUOTE_REQUIREMENT
                                       + ',' + POST_QUOTE_REQUIREMENT + ','
                                       + PURCHASE_REQUIREMENT + ',' + PARENT_ID + ','''
                                       + HELP_TEXT + ''',' + DEFAULT_OBJECT_ID + ','
                                       + DEFAULT_PROPERTY_ID + ',' + IS_VALUATION + ','
                                       + IS_RATE_AND_PREMIUM + ',@CHILDSCREENID,'
                                       + PMFORMAT + ',' + COLUMN_POSITION + ','
                                       + TAB_SET_INDEX + ',' + DATA_MODEL_TYPE + ') '
                                       + CHAR(13)
                                FROM   #GISSCREENDETAIL
                                WHERE  ID = @INDEX
                            END --C3BC

                          SET @INDEX=@INDEX + 1
                      END --C3B
                END --C3A
            END --C3
      END --A3

    ----@MANAGE UMLS-----
    SELECT @INDEX = 1,
           @MAXCNT = MAX(ID)
    FROM   #UML

    IF @MAXCNT > 0
      BEGIN --A4 
          DECLARE @UMLCODE VARCHAR(10)

          INSERT INTO #QUERIES
          SELECT ' '

          INSERT INTO #QUERIES
          SELECT '--*********************************** THIS WILL INSERT UMLS IF NEEDED FOR DATAMODEL ( '
                 + @sDataModelCode
                 + ') ***********************************'

          WHILE ( @INDEX <= @MAXCNT )
            BEGIN--A4A
                SELECT @UMLCODE = #UML.UMLCODE
                FROM   #UML
                WHERE  ID = @INDEX

                EXECUTE spu_UML_Script
                  @UMLCODE

                SET @INDEX=@INDEX + 1
            END --A4A
      END --A4

    ----@MANAGE UDLS-----
    SELECT @INDEX = 1,
           @MAXCNT = MAX(ID)
    FROM   #UDL

    IF @MAXCNT > 0
      BEGIN --A5
          DECLARE @UDLCODE VARCHAR(255)

          INSERT INTO #QUERIES
          SELECT ' '

          INSERT INTO #QUERIES
          SELECT '--*********************************** THIS WILL INSERT UDLS/PMLOOKUPS IF NEEDED FOR DATAMODEL ( '
                 + @sDataModelCode
                 + ') ***********************************'

          WHILE ( @INDEX <= @MAXCNT )
            BEGIN--A5A
                SELECT @UDLCODE = #UDL.UDLNAME
                FROM   #UDL
                WHERE  ID = @INDEX

                EXECUTE spu_UDL_Script
                  @UDLCODE

                SET @INDEX=@INDEX + 1
            END --A5A
      END --A5

    IF @COPYPRODUCTANDRISK = 1
      BEGIN --B1
          EXECUTE spu_GetProductAndRisk
            @sDataModelCode
      END --B1

    --***********************************=====================FINAL=========================***********************************---
    SELECT @MAXCNT = CONVERT(INT, MAX(ID))
    FROM   #QUERIES

    IF @MAXCNT > 0
      BEGIN--Y		
          IF @CAPTURESCREENS <> 0
            BEGIN--YA1
                INSERT INTO #QUERIES
                SELECT 'UPDATE GIS_SCREEN 	' + CHAR(13)
                       + 'SET SCRIPT_DEFAULTS = REPLACE(CONVERT(VARCHAR(MAX),SCRIPT_DEFAULTS),''^'',''''''''), '
                       + CHAR(13)
                       + 'SCRIPT_DYNAMIC_LOGIC = REPLACE(CONVERT(VARCHAR(MAX),SCRIPT_DYNAMIC_LOGIC),''^'','''''''') '
                       + CHAR(13)
                       + 'WHERE SCRIPT_DEFAULTS IS NOT NULL AND GIS_DATA_MODEL_ID =@DATAMODELID'

                INSERT INTO #QUERIES
                SELECT 'UPDATE GIS_SCREEN_DETAIL ' + CHAR(13)
                       + 'SET CAPTION = REPLACE(CAPTION,''^'','''''''') '
                       + CHAR(13)
                       + 'WHERE CAPTION IS NOT NULL AND GIS_SCREEN_ID IN( SELECT GIS_SCREEN_ID FROM GIS_SCREEN  WHERE GIS_DATA_MODEL_ID =@DATAMODELID)'
            END --YA1

          INSERT INTO #QUERIES
          SELECT 'UPDATE GIS_PROPERTY ' + CHAR(13)
                 + 'SET SPECIALS_TYPE_REFERENCE=CONVERT(VARCHAR(10),GISUDH.GIS_USER_DEF_HEADER_ID) '
                 + CHAR(13) + 'FROM GIS_PROPERTY GISP '
                 + CHAR(13)
                 + 'JOIN GIS_USER_DEF_HEADER GISUDH '
                 + CHAR(13)
                 + 'ON GISUDH.CODE=GISP.SPECIALS_TYPE_REFERENCE '
                 + CHAR(13) + 'WHERE GISP.SPECIALS_TYPE=6'
				  --INSERT INTO #QUERIES
				  --SELECT
				  --'UPDATE GIS_USER_DEF_HEADER ' + CHAR(13) +
				  --'SET DESCRIPTION = REPLACE(DESCRIPTION,''^'','''''''')' 
				  --INSERT INTO #QUERIES
				  --SELECT
				  --'UPDATE GIS_USER_DEF_DETAIL ' + CHAR(13) +
				  --'SET DESCRIPTION = REPLACE(DESCRIPTION,''^'','''''''')' 
      END --Y
    ELSE
      BEGIN--Z
          INSERT INTO #QUERIES
          SELECT '--NO SCRIPT AVAILABLE FOR DATAMODEL ( '
                 + @sDataModelCode + ')'
      END --Z	

    SELECT QUERY
    FROM   #QUERIES
    ORDER  BY ID ASC

    DROP TABLE #GISOBJECTS
    DROP TABLE #GISPROPERTIES
    DROP TABLE #GISSCREEN
    DROP TABLE #GISSCREENDETAIL
    DROP TABLE #UML
    DROP TABLE #UDL
    DROP TABLE #QUERIES 
