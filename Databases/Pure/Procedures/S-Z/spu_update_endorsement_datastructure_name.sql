SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_update_endorsement_datastructure_name'
GO

CREATE PROCEDURE spu_update_endorsement_datastructure_name
	@DataModelCode VARCHAR(20),    
	@Table_Name VARCHAR(255),
	@DataStructure_Name VARCHAR(255),
	@Column_Name VARCHAR(255)
AS

BEGIN

UPDATE wp_fields
SET DataStructure_Name = @DataStructure_Name + '_' + @Column_Name
WHERE Table_Name = @Table_Name 
AND data_model = @DataModelCode
AND specials_type = 5
AND column_name = @Column_Name  

END
GO


