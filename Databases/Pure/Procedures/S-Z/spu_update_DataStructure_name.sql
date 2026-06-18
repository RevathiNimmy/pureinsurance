SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_update_DataStructure_name'
GO


CREATE PROCEDURE spu_update_DataStructure_name
	@DataModelCode VARCHAR(20),
    @Sub_Group VARCHAR(255),
	@Loop1 VARCHAR(255),
	@Loop2 VARCHAR(255),
	@Loop3 VARCHAR(255),
	@Table_Name VARCHAR(255),
	@DataStructure_Name VARCHAR(255)
AS


BEGIN

UPDATE wp_fields
SET DataStructure_Name = @DataStructure_Name
WHERE sub_group=@Sub_Group
AND loop1=@Loop1
AND loop2=@Loop2
AND loop3=@Loop3
AND Table_Name=@Table_Name 
AND data_model=@DataModelCode

END
GO


