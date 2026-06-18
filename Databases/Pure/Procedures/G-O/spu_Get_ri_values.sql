EXECUTE ddldropprocedure 'spu_Get_ri_values'
go

CREATE PROCEDURE spu_Get_ri_values    
    @insurance_file_cnt INT,    
    @risk_cnt INT,  
	@value NUMERIC(19, 4) OUTPUT ,
    @effective_date DATE = NULL     
      
AS

BEGIN

DECLARE @insurance_folder_cnt INT,
    @risk_type_id INT,
    @data_model VARCHAR(70),
    @seq_id INT,
    @table VARCHAR(70),
    @column VARCHAR(70),  
    @risk_type_ri_limit_version_id INT    

DECLARE @inds_id0 INT,
    @inds_id1 INT,
    @inds_id2 INT,
    @inds_id3 INT

DECLARE @ProcessId INT

DECLARE @SQL NVARCHAR(1000)    

IF @effective_date IS NULL
SELECT @effective_date=convert(datetime, convert(varchar(10),GETDATE() , 112))

--Dont mix DDL and DML statements, it forces a recompile (RC) PN 39575
  -- Need to get the property to get the column names.
  -- Use cursor as we'll potentially have more than one.
CREATE TABLE #ReadFromDynamicSQL(inds_id INT NULL)

-- Got the insurance file cnt and risk cnt
-- Need to get the risk type from the risk

SELECT @insurance_folder_cnt = insurance_folder_cnt
    FROM Insurance_File
    WHERE insurance_file_cnt = @insurance_file_cnt

SELECT @risk_type_id = risk_type_id
    FROM Risk
    WHERE risk_cnt = @risk_cnt

-- Need to get the data model to build up the file names.
SELECT @data_model = rtrim(ltrim(d.code))
    FROM GIS_Data_Model d,
    GIS_Screen s,
    Risk r
    WHERE r.risk_cnt = @risk_cnt
    AND s.gis_screen_id = r.gis_screen_id
    AND d.gis_data_model_id = s.gis_data_model_id

DECLARE c_columns CURSOR FAST_FORWARD FOR
    SELECT r.risk_type_ri_properties_seq_id,
    o.table_name,
    p.column_name,  
    rlv.risk_type_ri_limit_version_id     
    FROM GIS_Object o,
    GIS_Property p,
    Risk_Type_RI_Properties r,  
    Risk_Type_RI_Limit_Version rlv    
    WHERE r.risk_type_id = @risk_type_id
    AND r.gis_property_id = p.gis_property_id
    AND p.gis_object_id = o.gis_object_id
    AND rlv.Risk_Type_RI_Limit_Version_id = r.Risk_Type_RI_Limit_Version_id  AND rlv.risk_type_id = r.risk_type_id  
    AND rlv.ri_limit_start_date <= @effective_date AND rlv.ri_limit_end_date >= @effective_date  
    ORDER BY r.risk_type_ri_properties_seq_id

OPEN c_columns
FETCH NEXT FROM c_columns INTO @seq_id, @table, @column, @risk_type_ri_limit_version_id    

WHILE @@fetch_status = 0 BEGIN
    DELETE FROM #ReadFromDynamicSQL

    -- Now we hold the indicator on the user_def_detail record.
    -- ISS5365 - Peter Finney - Replaced hard coded RSA with @data_model
    SELECT @SQL = 'INSERT INTO #ReadFromDynamicSQL ' +
        'SELECT d.gis_user_def_header_inds_id ' +
        'FROM gis_user_def_detail as d ' +
        'inner join ' + @table + ' as t on d.GIS_user_def_detail_id = t.' + @column + ' ' +
        'inner join ' + @data_model + '_policy_binder as pb on t.' + @data_model + '_policy_binder_id = pb.' + @data_model + '_policy_binder_id ' +
        'inner join gis_policy_link as gpl on pb.gis_policy_link_id = gpl.gis_policy_link_id ' +
        'WHERE gpl.insurance_file_cnt = ' + cast(@insurance_folder_cnt as varchar(20)) + ' ' +
        'AND gpl.risk_id = ' + cast(@risk_cnt as varchar(20))

    EXECUTE(@SQL)

    SELECT @inds_id0 = 0
    SELECT @inds_id0 = inds_id FROM #ReadFromDynamicSQL

    IF @seq_id = 1 
	BEGIN
        SELECT @inds_id1 = @inds_id0
    END 
	ELSE IF @seq_id = 2 
	BEGIN
        SELECT @inds_id2 = @inds_id0
    END 
	ELSE IF @seq_id = 3 
	BEGIN
        SELECT @inds_id3 = @inds_id0
    END

    FETCH NEXT FROM c_columns INTO @seq_id, @table, @column ,@risk_type_ri_limit_version_id   
END

CLOSE c_columns
DEALLOCATE c_columns

DROP TABLE #ReadFromDynamicSQL

/* So now we've got the inds, we need to work backwards until we have a value */
SELECT @value = NULL, @seq_id = 3

WHILE @seq_id > 0 BEGIN
    SELECT @value = NULL
    SELECT @value = value
    FROM Risk_Type_RI_Values
    WHERE risk_type_id = @risk_type_id
    AND gis_user_def_header_inds_id1 = @inds_id1
    AND gis_user_def_header_inds_id2 = @inds_id2
    AND gis_user_def_header_inds_id3 = @inds_id3
	AND risk_type_ri_limit_version_id = @risk_type_ri_limit_version_id  

    IF @value IS NOT NULL BEGIN
        SELECT @seq_id = 0
        BREAK
    END ELSE IF @seq_id = 1 BEGIN
        SELECT @seq_id = 0
    END ELSE IF @seq_id = 2 BEGIN
        SELECT @inds_id2 = 0, @seq_id = 1
    END ELSE IF @seq_id = 3 BEGIN
        SELECT @inds_id3 = 0, @seq_id = 2
    END
END

SELECT @value

END

