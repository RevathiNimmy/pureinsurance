SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_wording_risk_link_add'
GO
CREATE PROCEDURE spu_wording_risk_link_add
    @document_template_id int,
    @risk_type_id int,
	@branch_id int = 1,
	@default smallint = 0
AS

BEGIN  
 IF NOT EXISTS (SELECT 1 FROM wording_risk_type_link WHERE   
     document_template_id = @document_template_id AND  
     risk_type_id         = @risk_type_id AND  
     branch_id            = @branch_id)  
 BEGIN  
  INSERT INTO wording_risk_type_link (  
   document_template_id,  
   risk_type_id,  
   branch_id,  
   [default])  
  VALUES (  
   @document_template_id,  
   @risk_type_id,  
   @branch_id,  
   @default)  
 END  
END  

