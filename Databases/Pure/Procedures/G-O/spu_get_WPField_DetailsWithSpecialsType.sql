EXECUTE DDLDropProcedure 'spu_get_WPField_DetailsWithSpecialsType'
GO
   
CREATE PROCEDURE spu_get_WPField_DetailsWithSpecialsType
	@CCMWPFields CCMWPFields READONLY,
	@Insurance_file_cnt INT,  
 	@Claim_id INTEGER = 0   
AS
DECLARE @TEMP TABLE ( TableName VARCHAR(255),DataStructureName VARCHAR(255), ColumnName VARCHAR(255), specials_type INT, Included tinyint, DataModel VARCHAR(50))
Declare @kSpecialsTypeSW INT = 5
DECLARE @Table_name VARCHAR(255)

	INSERT INTO @TEMP (TableName , DataStructureName, ColumnName , specials_type , Included, DataModel)
	SELECT CCM.Table_Name, WP.DataStructure_Name, CCM.Column_name, 0 , 1, WP.Data_Model  FROM @CCMWPFields CCM
	JOIN WP_FIELDS WP ON CCM.Table_Name = WP.Table_Name AND
	CCM.Column_name = WP.column_name
	WHERE ISNULL(WP.specials_type,0) <> @kSpecialsTypeSW					-- 5 : Standard Wording Specials Type

UNION
	SELECT DISTINCT CCM.Table_Name,CCM.Table_Name, CCM.Table_Name,5,1,NULL
	FROM @CCMWPFields CCM WHERE CCM.Table_Name= 'PolicyStandardWordings'

	DECLARE Cur CURSOR FOR
	SELECT DISTINCT WP.table_name FROM @CCMWPFields CCM	JOIN WP_FIELDS WP ON CCM.Table_Name = WP.Table_Name
	WHERE specials_type = @kSpecialsTypeSW
	AND ( CCM.Column_name = WP.column_name OR CCM.Column_name IN ('FilePath','FileName'))

	OPEN cur
	FETCH NEXT FROM cur INTO @Table_name
	WHILE @@FETCH_STATUS = 0
	BEGIN
		

		--For SW
		INSERT INTO @TEMP (TableName ,DataStructureName, ColumnName , specials_type , Included, DataModel)
		SELECT TOP 1 CCM.Table_Name,WP.DataStructure_Name, WP.Column_name, @kSpecialsTypeSW , 1,WP.Data_Model  FROM @CCMWPFields CCM
		JOIN WP_FIELDS WP ON CCM.Table_Name = WP.Table_Name 
		WHERE ISNULL(WP.specials_type,0) = @kSpecialsTypeSW		AND WP.column_name NOT IN( 'SWCODE','SWDESC') AND WP.Table_Name=@Table_name

		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO @TEMP (TableName ,DataStructureName, ColumnName , specials_type , Included,DataModel)
				SELECT TOP 1 CCM.Table_Name,WP.DataStructure_Name, WP.Column_name, @kSpecialsTypeSW , 0,WP.Data_Model  FROM @CCMWPFields CCM
			JOIN WP_FIELDS WP ON CCM.Table_Name = WP.Table_Name AND CCM.Column_name = WP.column_name
			WHERE ISNULL(WP.specials_type,0) = @kSpecialsTypeSW	AND WP.column_name NOT IN( 'SWCODE','SWDESC') AND WP.Table_Name=@Table_name
		END

		FETCH NEXT FROM cur INTO @Table_name
	END
	CLOSE cur
	DEALLOCATE cur

	SELECT * FROM @Temp 
	WHERE DataModel IN 
			(SELECT gdm.Code FROM insurance_file ifl JOIN Insurance_file_risk_link ifrl ON ifl.insurance_file_cnt = ifrl.insurance_file_cnt
			JOIN Risk ON ifrl.risk_cnt = Risk.risk_cnt 
			JOIN risk_type ON Risk.Risk_type_id = risk_type.risk_type_id
			JOIN gis_screen ON risk_type.gis_screen_id= gis_screen.gis_screen_id
			JOIN gis_data_model gdm ON gis_screen.gis_data_model_id= gdm.gis_data_model_id 
			WHERE ifl.insurance_file_cnt = @Insurance_file_cnt) OR DataModel IS NULL
			or
			DataModel IN  
			(SELECT gdm.Code FROM claim cc   
			JOIN gis_screen ON cc.gis_screen_id= gis_screen.gis_screen_id    
			JOIN gis_data_model gdm ON gis_screen.gis_data_model_id= gdm.gis_data_model_id    
			WHERE cc.claim_id = @Claim_id)
		ORDER BY Tablename
