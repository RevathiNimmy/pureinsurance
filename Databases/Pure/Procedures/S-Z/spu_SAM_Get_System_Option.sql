SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_System_Option'
GO


CREATE PROCEDURE spu_SAM_Get_System_Option  
  
@code varchar(10),   
@option_number int,   
@option_value varchar(255)  OUTPUT
  
AS  
  
if @option_number>=2003 and @option_number<=2007  
Begin  
SELECT @option_value = code FROM system_options so  WITH(NOLOCK) ,Gis_User_Def_Header gh  WITH(NOLOCK)  
WHERE branch_id in (SELECT source_id FROM source WITH(NOLOCK) WHERE code = @code)  
AND option_number = @option_number  
and so.value=gh.Gis_User_Def_Header_id  
SELECT @option_value=ISNULL(@option_value,0)  
End  
else if @option_number=5042
Begin  
SELECT @option_value = code FROM system_options so  WITH(NOLOCK) ,PMUser_Group ug  WITH(NOLOCK)  
WHERE branch_id in (SELECT source_id FROM source WITH(NOLOCK) WHERE code = @code)  
AND option_number = @option_number  
and so.value=ug.pmuser_group_id 
SELECT @option_value=ISNULL(@option_value,0)  
End  
Else  
SELECT @option_value = isnull(value,'') FROM system_options WITH(NOLOCK)  
INNER JOIN Source s WITH(NOLOCK) ON branch_id = s.source_id  
WHERE s.code = @code  
AND option_number = @option_number  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
