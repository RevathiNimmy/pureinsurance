SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_clm_get_transdetails'
GO

CREATE PROCEDURE spu_clm_get_transdetails    
@nDocument_id int    
AS    
BEGIN  
 
Select Account_id,
		transdetail_id , 
		OutStanding_Amount ,
		transdetail_type_id 
		From TransDetail 
		Where OutStanding_Amount <> 0 AND Document_id=@nDocument_id
 
END    

GO