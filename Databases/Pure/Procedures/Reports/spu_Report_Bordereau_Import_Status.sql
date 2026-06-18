SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDROPPROCEDURE 'spu_Report_Bordereau_Import_Status'
GO

CREATE PROCEDURE spu_Report_Bordereau_Import_Status  
@import_type varchar(20),  
@import_date varchar(30)  
AS  
  
declare @new_import_type varchar(20)  
IF @import_type  = 'Written Premium'  
BEGIN  
SET @new_import_type  ='Policy_BDX_Import'  
END  
ELSE IF @import_type  = 'Paid Premium'  
BEGIN  
SET @new_import_type  ='Premium_BDX_Import'  
END  
ELSE IF @import_type  = 'Claims'  
BEGIN  
SET @new_import_type  ='Claim_BDX_Import'  
END  
  
SELECT log_date,@import_type 'class_name',text ErrorDesc,  
SUBSTRING(err_description, PATINDEX('%(Agent:%',err_description)+7,CASE WHEN PATINDEX('%)%',err_description)-PATINDEX('%(Agent:%',err_description)-7<0 THEN 0 ELSE PATINDEX('%)%',err_description)-PATINDEX('%(Agent:%',err_description)-7 END) Agent,  
SUBSTRING(err_description, PATINDEX('%<CoverHolder:%',err_description)+13,CASE WHEN PATINDEX('%>%',err_description)-PATINDEX('%<CoverHolder:%',err_description)-13<0 THEN 0 ELSE PATINDEX('%>%',err_description)-PATINDEX('%<CoverHolder:%',err_description)-13
 END) CoverHolder  
 from PMMessage  
where class_name like @new_import_type  
and CONVERT(datetime,convert(varchar,log_date,106))=@import_date  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
