EXECUTE DDLDropProcedure 'spu_get_subgroup_for_endorsement'
GO
   
CREATE PROCEDURE spu_get_subgroup_for_endorsement
	@Table_Name VARCHAR(255),
	@Column_Name VARCHAR(255)
AS  

BEGIN 
	SELECT DISTINCT data_model, sub_group, loop1,loop2,loop3, field_name  
	FROM wp_fields
	WHERE Table_Name=@Table_Name
	AND column_name=@Column_Name
	AND specials_type=5 
 
END  
GO