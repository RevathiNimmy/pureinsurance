
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_get_WPFields_for_DataBackbone'
GO


/*************************************************************************/  
/*Get wp_fields for Data Backbone structure*/
/*************************************************************************/  
  
  
CREATE PROCEDURE spu_get_WPFields_for_DataBackbone
@DataModelCode AS VARCHAR(50),
@Sub_Group As VARCHAR(255)
AS        
        
BEGIN
	DECLARE @HasEndorsement AS BIT
	DECLARE @ParentDatastructureName VARCHAR(255)

	SELECT  @HasEndorsement = 1
	FROM wp_fields
	WHERE data_model=@DataModelCode
	AND sub_group=@Sub_Group
	AND specials_type = 5

	SELECT @ParentDatastructureName =ISNULL(Table_Name, '')
	FROM  wp_fields
	WHERE data_model=@DataModelCode
	AND sub_group=@Sub_Group
	AND specials_type <> 5
	AND ISNULL(loop1, '')=''

	SELECT DISTINCT
	ISNULL(main_group, '') AS main_group,
	ISNULL(sub_group, '') AS sub_group,
	ISNULL(loop1, '') AS loop1,
	ISNULL(loop2, '') AS loop2,
	ISNULL(loop3, '') AS loop3,
	ISNULL(Table_Name, '') AS Table_Name,
	ISNULL(DataStructure_Name, '') AS DataStructure_Name,
	@HasEndorsement AS HasEndorsement,
	@ParentDatastructureName AS ParentDatastructureName
	FROM wp_fields
	WHERE data_model=@DataModelCode
	AND sub_group=@Sub_Group
	AND specials_type <> 5
END  
GO