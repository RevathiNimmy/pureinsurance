SET QUOTED_IDENTIFIER OFF 
GO

Execute DDLDropProcedure 'spu_SIR_Lapse_OOS_versions'
GO
CREATE PROCEDURE spu_SIR_Lapse_OOS_versions
 @base_insurance_file_cnt INTEGER,  
 @insurance_folder_cnt INTEGER 
  As  
 BEGIN  
DECLARE @lapsed_date DATETIME  
DECLARE @LapseId INTEGER  
DECLARE @LapseDesc VARCHAR(255)

	-- Select laspsed info from first live version in chronological order
	SELECT  @lapsed_date = ifi.lapsed_date,
			@LapseId = ifi.lapsed_reason_id,
			@LapseDesc = ifi.lapsed_description
				FROM insurance_file ifi 
		WHERE insurance_file_cnt = 
			(Select TOP 1 insurance_file_cnt From insurance_file 
				Where insurance_folder_cnt = @insurance_folder_cnt and insurance_file_type_id IN (2, 5, 6, 9)
					Order By cover_start_date desc) 
   
	If ISNULL(@LapseId, 0) > 0 AND Datepart(YYYY , (ISNULL(@lapsed_date,'1899-12-29 00:00:00.000')) ) > Datepart(YYYY,'1899-12-29 00:00:00.000')
		UPDATE insurance_file   
			SET  lapsed_reason_id = @LapseId ,  
					lapsed_date = @lapsed_date,   
						lapsed_description = @LapseDesc
		WHERE Base_Insurance_File_Cnt = @base_insurance_file_cnt   
    
END  
    
GO
SET QUOTED_IDENTIFIER OFF
GO

