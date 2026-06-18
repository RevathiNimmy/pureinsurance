SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Renewal_NCD_GetNCDDetails'
GO


CREATE PROCEDURE spu_Renewal_NCD_GetNCDDetails
    @insurance_file_cnt INT
AS

BEGIN

    DECLARE @DataModel as varchar(10)
    DECLARE @PolicyBinder Varchar(50)
    DECLARE @TableName Varchar(50)
    DECLARE @Sql1 nvarchar(1000)
    DECLARE @Temp as varchar(50)
    
    SELECT @Temp = Cast(@insurance_file_cnt AS VARCHAR(50)) 
    
    -- Extract the Data model first
    SELECT @DataModel = d.code from GIS_Policy_Link p, GIS_Data_Model d
    	   WHERE p.Insurance_File_Cnt = @insurance_file_cnt
    	   AND   d.GIS_Data_Model_Id = p.GIS_Data_Model_Id
    
    -- Use this datamodel-code to build the table name
    SELECT @TableName = LTRIM(RTRIM(@DataModel)) + '_Policy' 	   
    SELECT @PolicyBinder = LTRIM(RTRIM(@DataModel)) + '_Policy_Binder_Id' 	   
    	   

    /* Build the Select statement */
    SELECT @Sql1 = 'SELECT i.Insurance_Folder_Cnt, i.Insurance_Ref, isnull(NCBYears,0) from ' + @TableName + ' p, GIS_Policy_Link l '
    		   + ', Insurance_File i WHERE l.Insurance_file_cnt = ' + @Temp
    		   + ' AND   i.Insurance_file_cnt = ' + @Temp
    		   + ' AND   p.' + @PolicyBinder + ' = l.GIS_Policy_Link_Id'

    -- run the execute statement now
    EXECUTE sp_executesql  @Sql1

 

END
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
