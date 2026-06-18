SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_wp_CreditControlOIP'
GO

CREATE PROCEDURE spu_wp_CreditControlOIP
    @HasOIP tinyint OUTPUT,
    @OIPCount Int OUTPUT,
    @Insurance_file_cnt int
AS

    DECLARE @DataModelCode VARCHAR(10)
    DECLARE @SQL VARCHAR(500)	
    DECLARE @OIP1 INT
    DECLARE @OIP2 INT
    DECLARE @OIP3 INT
    DECLARE @OIP4 INT
    DECLARE @OIP5 INT
    DECLARE @ROWS INT
    DECLARE @TableName NVARCHAR(255)

    CREATE TABLE #TempOIPList
       (OIP1 INT,
        OIP2 INT,
        OIP3 INT,
        OIP4 INT,
        OIP5 INT)

    DECLARE DMList CURSOR FAST_FORWARD FOR
        SELECT RTRIM(Code)
          FROM GIS_Data_Model gdm
    INNER JOIN GIS_Policy_Link gpl ON gpl.gis_data_model_id = gdm.gis_data_model_id
    INNER JOIN Insurance_File_Risk_Link ifrl ON ifrl.risk_cnt = gpl.risk_id
         WHERE ifrl.insurance_file_cnt = @insurance_file_cnt
      GROUP BY RTRIM(Code)
    OPEN DMList
    FETCH NEXT FROM DMList INTO @DataModelCode
    WHILE @@FETCH_STATUS = 0
    BEGIN        
	SELECT @TableName = @DataModelCode + '_OIP'
	if exists (select NULL from dbo.sysobjects where id = object_id(@TableName) and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
	  SELECT @SQL = 'INSERT INTO #TempOIPList SELECT OIP.OIP1, OIP.OIP2, OIP.OIP3, OIP.OIP4, OIP.OIP5 '
        SELECT @SQL = @SQL + 'FROM ' + @DataModelCode + '_OIP oip '
        SELECT @SQL = @SQL + 'INNER JOIN ' + @DataModelCode + '_Policy_Binder pb ON '
        SELECT @SQL = @SQL + 'pb.' + @DataModelCode + '_policy_binder_id = oip.' + @DataModelCode + '_policy_binder_id '
        SELECT @SQL = @SQL + 'INNER JOIN GIS_Policy_Link gpl ON gpl.gis_policy_link_id = pb.gis_policy_link_id '
        SELECT @SQL = @SQL + 'INNER JOIN Insurance_File_Risk_Link ifrl ON ifrl.risk_cnt = gpl.risk_id '
        SELECT @SQL = @SQL + 'WHERE ifrl.insurance_file_cnt = ' + CONVERT(VARCHAR(50), @Insurance_File_Cnt)
        EXECUTE (@SQL)
	END
      FETCH NEXT FROM DMList INTO @DataModelCode
    END
    CLOSE DMList
    DEALLOCATE DMList

    SELECT @ROWS = COUNT(*) FROM #TempOIPList
    IF  @ROWS > 0 
    BEGIN
	    DECLARE OIPList CURSOR FAST_FORWARD FOR
	    SELECT * FROM #TempOIPList
	
	    OPEN OIPList
	    FETCH NEXT FROM OIPList INTO @OIP1,@OIP2,@OIP3,@OIP4,@OIP5
	    WHILE @@FETCH_STATUS = 0
	    BEGIN
		  If NOT @OIP1 IS NULL  
		  BEGIN
			SET @OIPCount = @OIPCount + 1
			SET @HasOIP = 1
		  END
	
		  If NOT @OIP2 IS NULL  
		  BEGIN
			SET @OIPCount = @OIPCount + 1
			SET @HasOIP = 1
		  END
	
		  If NOT @OIP3 IS NULL  
		  BEGIN
			SET @OIPCount = @OIPCount + 1
			SET @HasOIP = 1
		  END
	
		  If NOT @OIP4 IS NULL  
		  BEGIN
			SET @OIPCount = @OIPCount + 1
			SET @HasOIP = 1
		  END
	
		  If NOT @OIP5 IS NULL  
		  BEGIN
			SET @OIPCount = @OIPCount + 1
			SET @HasOIP = 1
		  END	  
	
	        FETCH NEXT FROM OIPList INTO @OIP1,@OIP2,@OIP3,@OIP4,@OIP5
	    END
	    CLOSE OIPList
	    DEALLOCATE OIPList
    END

    DROP TABLE #TempOIPList
GO