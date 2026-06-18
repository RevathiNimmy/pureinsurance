SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure spu_SAM_Product_sel 
GO
--Start (Sriram P)Tech Spec - UIIC - Product Risk Maintenance (6.2)
CREATE PROCEDURE spu_SAM_Product_sel(
    @product_id int,
    @column_name varchar (255))
AS  
BEGIN
	DECLARE @sql varchar(255)
	IF @column_name LIKE 'claims_UDT_%' 
		BEGIN

		SET @sql = 'SELECT LTRIM(RTRIM(CODE)) FROM GIS_User_Def_Header WHERE
					GIS_User_Def_Header_ID IN (SELECT ' + LTRIM(RTRIM(@column_name))
		SET @sql = @sql + ' FROM Product  '

		SET @sql = @sql + 'WHERE product_id = '  + CONVERT(varchar(5),@product_id) + ')'
	
		END
	
	ELSE
	
	BEGIN
	
		SET @sql = 'SELECT ' + LTRIM(RTRIM(@column_name))
	  

	SET @sql = @sql + ' FROM Product  '

	SET @sql = @sql + 'WHERE product_id = '  + CONVERT(varchar(5),@product_id) 
	END
	EXEC (@sql)
END

--End (Sriram P)Tech Spec - UIIC - Product Risk Maintenance (6.2)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
