SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_List_Type_Item_Exists'
GO

CREATE PROCEDURE spu_GIS_List_Type_Item_Exists  
    @table varchar(50),  
    @code varchar(50) ,
    @version integer = 0
 
    
AS  
  
DECLARE @selectstatement Nvarchar(200)  
  
BEGIN  
  
SELECT @selectstatement = 'SELECT * FROM ' + @table + ' WHERE code = ' + '''' + @code +''''  

IF @Version <> 0 
 	SELECT @selectstatement =@selectstatement + ' And UDL_Version = ' + Convert(varchar,@version)
ELSE
	SELECT @selectstatement =@selectstatement + ' And UDL_Version = (SELECT MAX(udl_version) From ' + @table + ' WHERE code = ' + '''' + @code + ''')'

  
EXECUTE sp_executeSQL @selectstatement  
  
END  

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
