SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Report_LocumTenens_SFU'
GO


CREATE      PROCEDURE spu_Report_LocumTenens_SFU
  @branch_id   	INT,
  @startDTCombo VARCHAR(50) = '',
  @endDTCombo   VARCHAR(50) = ''
AS

DECLARE  @SQL    	NVARCHAR(1000),
  @Party_Cnt    	INT,
  @GIS_Screen_ID   	INT,
  @GIS_Model_ID   	INT,
  @GIS_data_Code   	VARCHAR(50),
  @GIS_Policy_Link_ID  	INT,
  @Title_Code  		VARCHAR(50),
  @Fore_Name   		VARCHAR(50),
  @Last_Name   		VARCHAR(50),
  @Party_code   	VARCHAR(50),
  @Table_Name  		VARCHAR(50),
  @Count_Properties  	INT,
  @Party_Name  		VARCHAR(50),
  @ibranchid            INT,
  @dtPeriodStartDate	DATETIME,
  @dtPeriodEndDate	DATETIME,
  @IsLocum              INT,
  @RiskType             INT


SELECT 	@dtPeriodStartDate = CONVERT (Datetime, @startDTCombo),
	@dtPeriodEndDate = CONVERT (Datetime, @endDTCombo)

IF  @branch_id IS NULL
   SELECT @iBranchID = 0
ELSE
   SELECT @iBranchID = @branch_id

CREATE TABLE #ListParty
(

  Party_Name   VARCHAR(255),
  --Last_Name   VARCHAR(50),
  Licence_Number  VARCHAR(255),
  Title    VARCHAR(255),
  Expiration_Date  DATETIME,
  Party_Cnt   INT
)

SET NOCOUNT ON

DECLARE Party_List
   CURSOR STATIC FOR
   SELECT
     P.Party_Cnt,
     PT.GIS_Screen_id,
     GS.GIS_Data_Model_id,
     GD.Code,
     GPL.GIS_Policy_Link_Id,
     PT.code,
     P.name
   FROM Party P
   INNER JOIN party_type PT
      ON P.Party_Type_id = PT.Party_Type_id
   INNER JOIN gis_screen GS
      ON PT.Gis_Screen_id = GS.Gis_Screen_id
   INNER JOIN gis_data_model GD
      ON GD.Gis_data_model_id = GS.Gis_data_model_id
   INNER JOIN gis_policy_link GPL
      ON P.Party_Cnt = GPL.Party_Cnt

   WHERE
                (
                (@ibranchid <> 0 AND p.source_id = @ibranchid)
                  OR
                (@ibranchid =0)
                )
 AND pt.code IN ('PC')
 ORDER BY p.name

 OPEN Party_List
   FETCH NEXT FROM Party_List
    INTO
     @Party_Cnt,
     @GIS_Screen_ID,
     @GIS_Model_ID,
     @GIS_data_Code,
     @GIS_Policy_Link_ID,
     @Party_code,
     @Last_Name

  WHILE @@Fetch_Status = 0
  BEGIN

   IF @Party_code = 'PC'
    BEGIN
  SELECT  @Fore_Name = forename,
   @Title_Code = party_title_code
      FROM Party_Personal_Client
      WHERE Party_cnt = @Party_Cnt
  SET @Party_Name = @Fore_Name + ' ' + @Last_Name
 END
 ELSE
     SELECT  @Party_Name = resolved_name
      FROM Party
      WHERE Party_cnt = @Party_Cnt


SELECT 	@RiskType = Risk_Type
	FROM PB_Policy_Binder PPB	
	
	INNER JOIN PB_PERCLNT PBP
	ON PPB.PB_Policy_Binder_id = PBP.PB_Policy_Binder_Id
WHERE	PPB.Gis_Policy_Link_id = @GIS_Policy_Link_ID

IF @RiskType =469
	SET @Table_Name = 'PB_PHY'
ELSE IF @RiskType = 470
	SET @Table_Name = 'PB_ANC'

SELECT  @Count_Properties = COUNT(property_name)
  	FROM  Gis_Property GP
  	WHERE GP.property_name IN ('License_Num','License_Exp_Date','Title')


  IF @Count_Properties>= 3
  BEGIN
	   IF @Table_Name = 'PB_PHY'
	   BEGIN
		   SET @SQL ='SELECT @IsLocum = Locum_Tenen FROM ' + @Table_Name 
		   SET @SQL = @SQL + ' Where ' + RTRIM(CONVERT(VARCHAR(50),@GIS_data_Code)) + '_Policy_binder_id'
		   SET @SQL = @SQL + ' = ' + CONVERT(VARCHAR(50),@GIS_Policy_Link_ID)
		
		   EXECUTE SP_EXECUTESQL @SQL,N'@IsLocum INT OUTPUT',@IsLocum OUTPUT
	   END
	   IF @IsLocum=1
	   BEGIN
		   SET @SQL = 'INSERT INTO #ListParty(Licence_Number,Title,Expiration_Date,party_cnt) '
		   SET @SQL = @SQL + 'SELECT License_Num,'
		   SET @SQL = @SQL + '(SELECT Description FROM GIS_User_Def_Detail WHERE gis_user_def_detail_id = (select title from ' + RTRIM(CONVERT(VARCHAR(50),@Table_Name)) 
		   SET @SQL = @SQL + ' Where ' + RTRIM(CONVERT(VARCHAR(50),@GIS_data_Code)) + '_Policy_binder_id'
		   SET @SQL = @SQL + ' = ' + CONVERT(VARCHAR(50),@GIS_Policy_Link_ID) + ')),'
		   SET @SQL = @SQL + 'License_Exp_date,GPL.party_cnt FROM ' + RTRIM(CONVERT(VARCHAR(50),@Table_Name))
		   SET @SQL = @SQL + ' PG INNER JOIN gis_policy_link GPL'
		   SET @SQL = @SQL + ' ON GPL.gis_policy_link_id = PG.' + RTRIM(CONVERT(VARCHAR(50),@GIS_data_Code))
		   SET @SQL = @SQL + '_Policy_Binder_Id'
		   SET @SQL = @SQL + ' Where ' + RTRIM(CONVERT(VARCHAR(50),@GIS_data_Code)) + '_Policy_binder_id'
		   SET @SQL = @SQL + ' = ' + CONVERT(VARCHAR(50),@GIS_Policy_Link_ID)
	
	     	   EXEC (@SQL)
	
		   IF Exists(SELECT party_cnt FROM #ListParty WHERE party_cnt = @party_cnt)
			    UPDATE  #ListParty
			    SET Party_Name = @Party_Name
			    WHERE party_cnt = @party_cnt
	   END	
  END

   FETCH NEXT FROM Party_List
    INTO
     @Party_Cnt,
     @GIS_Screen_ID,
     @GIS_Model_ID,
     @GIS_data_Code,
     @GIS_Policy_Link_ID,
     @Party_code,
     @Last_Name

 END

CLOSE Party_List
DEALLOCATE Party_List

SELECT
  Party_Name,
  Licence_Number,
  Title,
  Expiration_Date,
  Party_cnt
FROM    #ListParty
WHERE
	(
		(@startDTCombo = ''
		AND @endDTCombo = ''
		)
  		  OR
		(@startDTCombo <> ''
		AND @endDTCombo <> ''
		AND Expiration_Date BETWEEN @dtPeriodStartDate AND @dtPeriodEndDate
		)
  		  OR
		(@startDTCombo <> ''
		AND @endDTCombo = ''
		AND Expiration_Date  >= @dtPeriodStartDate)
  		  OR
		(@startDTCombo = ''
		AND @endDTCombo <> ''
		AND Expiration_Date  <= @dtPeriodEndDate)

	)

DROP TABLE #ListParty


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

