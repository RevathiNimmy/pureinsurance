SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

EXEC DDLDROPPROCEDURE [spu_SAM_FindPolicies_By_RiskIndex]
GO

CREATE  PROCEDURE spu_SAM_FindPolicies_By_RiskIndex
 @search_object_name VARCHAR(70) = NULL,
 @search_value VARCHAR(255),
 @Specials_Type_Filter INT =0,
 @party_cnt INT =0,
 @MaxRowsToFetch INT = -1
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @SQL VARCHAR(500),
	@SQL2 VARCHAR(500),
	@SQL3 VARCHAR(500),
	@print VARCHAR(255),
	@gis_data_model_id  INTEGER,
	@object_name  VARCHAR(70),
	@property_name   VARCHAR(70),
	@table_name  VARCHAR(70),
	@column_name  VARCHAR(70),
	@policy_binder_id    INTEGER,
	@specials_type VARCHAR(10)

	CREATE TABLE #Matches_Found (policy_binder_id int, object_name varchar(70), property_name varchar(70), value varchar(255),Specials_Type Varchar(10))
	DECLARE @gis_data_model_code VARCHAR(10)
	DECLARE c_Data_Models CURSOR FAST_FORWARD FOR
	Select code 
	FROM gis_data_model
	Where Is_Deleted = 0 AND gis_data_model_type_id<>2
	OPEN c_Data_Models
	FETCH NEXT FROM c_Data_Models
	INTO @gis_data_model_code
	WHILE (@@FETCH_STATUS = 0)
	BEGIN
		/* First Select the Data Model ID from the Data Model Code */
		SELECT @gis_data_model_id = gis_data_model_id
		FROM gis_data_model
		WHERE code = @gis_data_model_code
		/* Then Build a Cursor for the Search Properties for this Data Model */
		/* If the Object to Search is known then Limit to that Object */
		IF(@search_object_name IS NULL) OR (@search_object_name = '')
			DECLARE c_search_properties CURSOR FAST_FORWARD FOR
			SELECT object_name,
			table_name,
			property_name,
			column_name,
			specials_type
			FROM gis_object o
			INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)
			WHERE o.gis_data_model_id = @gis_data_model_id
			AND
			(
			(@Specials_Type_Filter = 0 AND is_search_property = 1)
			OR
			(@Specials_Type_Filter <> 0)
			)
		ELSE
			DECLARE c_search_properties CURSOR FAST_FORWARD FOR
			SELECT  object_name,
			table_name,
			property_name,
			column_name,
			specials_type
			FROM gis_object o
			INNER JOIN gis_property p ON (o.gis_object_id = p.gis_object_id)
			WHERE is_search_property = 1
			AND o.gis_data_model_id = @gis_data_model_id
			AND o.object_name = @search_object_name
		/* Then Loop Round the Cursor and Do the Searches */
		OPEN c_search_properties
		FETCH NEXT FROM c_search_properties
		INTO @object_name,
		@table_name,
		@property_name,
		@column_name,
		@specials_type
		WHILE (@@FETCH_STATUS = 0)
		BEGIN
			SELECT @SQL = "INSERT INTO #Matches_Found (Policy_Binder_id, object_name, property_name, value, Specials_Type) SELECT " + RTRIM(LTRIM(@gis_data_model_code)) + "_policy_binder_id, '" + RTRIM(LTRIM(@object_name)) + "', '" +
			RTRIM(LTRIM(@property_name)) + "'," + RTRIM(LTRIM(@column_name))  + ", '" + RTRIM(LTRIM(@Specials_Type)) + "'"
			SELECT @SQL2 = " FROM " + RTRIM(LTRIM(@table_name)) + " WHERE " + RTRIM(LTRIM(@column_name))
			IF RIGHT(@search_value,1) = "%"
				SELECT @SQL2 = @SQL2 + " LIKE "
			ELSE
				IF @specials_type <> 0
					SELECT @SQL2 = @SQL2 + " LIKE "
				ELSE
					SELECT @SQL2 = @SQL2 + " = "
			SELECT @SQL3 = "'" + @search_value + "'"
			EXEC (@SQL + @SQL2 + @SQL3)
			FETCH NEXT FROM c_search_properties
			INTO @object_name,
			@table_name,
			@property_name,
			@column_name,
			@specials_type
		END
		CLOSE c_search_properties
		DEALLOCATE c_search_properties
		FETCH NEXT  FROM c_Data_Models
		INTO @gis_data_model_code
	END
	IF @MaxRowsToFetch <>-1
	BEGIN
	SET NOCOUNT ON
	SET ROWCOUNT @MaxRowsToFetch
END
DECLARE @insurancefolderCnts VARCHAR(MAX)
DECLARE @insurancefilecnts VARCHAR(MAX)
DECLARE @riskcnts VARCHAR(MAX)
SELECT
@insurancefolderCnts = COALESCE(@insurancefolderCnts + ',','') + CONVERT(VARCHAR,ifi.insurance_folder_cnt),
@insurancefilecnts = COALESCE(@insurancefilecnts + ',','') + CONVERT(VARCHAR,ifi.insurance_file_cnt),
@riskcnts = COALESCE(@riskcnts + ',', '') + CONVERT(VARCHAR,ifrl.risk_cnt)
FROM
#Matches_Found m
INNER JOIN gis_policy_link gpl
ON gpl.gis_policy_link_id = m.policy_binder_id
AND
(
(@Specials_Type_Filter = 0)
OR
(@Specials_Type_Filter <> 0 AND m.specials_type = @Specials_Type_Filter)
)
INNER JOIN insurance_file_risk_link ifrl
ON ifrl.risk_cnt = gpl.risk_id
INNER JOIN Insurance_File ifi
ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt
INNER JOIN Insurance_File_System ifs
ON ifs.insurance_file_cnt = ifi.insurance_file_cnt
AND (ifi.insurance_file_status_id IS NULL
OR ifi.insurance_file_status_id = 3   --REN hardcoded as 3
OR ifi.insurance_file_type_id = 8) --MTACan hardcoded as 8
AND ifi.policy_ignore IS NULL
INNER JOIN insurance_File_Type ift
ON ift.insurance_file_type_id = ifi.insurance_file_type_id
INNER JOIN Product pr
ON pr.product_id = ifi.product_id
INNER JOIN Policy_Type PT
ON PT.policy_type_id = ifi.policy_type_id
INNER JOIN Insurance_Folder ifo
ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
INNER JOIN Party p
ON p.party_cnt = ifo.insurance_holder_cnt
LEFT OUTER JOIN party_Lifestyle PL
ON P.party_cnt = PL.party_cnt
WHERE
(@party_cnt=0  OR p.party_cnt  =@party_cnt )
IF @MaxRowsToFetch <>-1
BEGIN
	SET ROWCOUNT 0
	SET NOCOUNT OFF
END
DROP TABLE #Matches_Found
SELECT @insurancefolderCnts,@insurancefilecnts,@riskcnts

SET NOCOUNT OFF
END
