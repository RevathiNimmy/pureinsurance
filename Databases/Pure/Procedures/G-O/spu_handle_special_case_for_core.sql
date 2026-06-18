EXECUTE DDLDropProcedure 'spu_handle_special_case_for_core'
GO
   
CREATE PROCEDURE spu_handle_special_case_for_core
	@Table_Name VARCHAR(255)
AS  

BEGIN 

	IF @Table_Name = 'CorePolicyEvent'
		UPDATE wp_fields
		SET Table_Name = @Table_Name + 'Desc'
		WHERE Table_Name=@Table_Name
		AND Loop1 IS NULL
		AND loop2 IS NULL
		AND loop3 IS NULL
		AND data_model IS NULL
	ELSE
		UPDATE wp_fields
		SET Table_Name = @Table_Name + 'Additional'
		WHERE Table_Name=@Table_Name
		AND Loop1 IS NULL
		AND loop2 IS NULL
		AND loop3 IS NULL
		AND data_model IS NULL
	

END  
GO