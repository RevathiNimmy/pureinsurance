EXECUTE DDLDropProcedure 'spu_SAM_Get_Advanced_Script_Option'
GO
CREATE PROCEDURE spu_SAM_Get_Advanced_Script_Option  
@taxGroupId int,   
@advancedTaxScript varchar(100)  OUTPUT  ,
@sTaxGroupCode varchar(10)=Null,
@o_sTaxGroupCode Varchar(10)=NULL OUTPUT,
@o_nTaxGroupID int=0 OUTPUT
AS  
BEGIN
IF @taxGroupId>0 
SELECT 
	  @advancedTaxScript=advanced_tax_script,
	  @o_nTaxGroupID= tax_group_id,
	  @o_sTaxGroupCode=code
	  FROM tax_group WHERE tax_group_id =@taxGroupId 
ELSE
SELECT
	  @advancedTaxScript=advanced_tax_script,
	  @o_nTaxGroupID= tax_group_id,
	  @o_sTaxGroupCode=code
	  FROM tax_group WHERE code =@sTaxGroupCode

END  
Go
