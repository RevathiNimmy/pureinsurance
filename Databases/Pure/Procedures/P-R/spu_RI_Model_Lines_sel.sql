SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_RI_Model_Lines_sel'
GO
CREATE PROCEDURE spu_RI_Model_Lines_sel    
    @ri_arrangement_id int      
AS      
    SELECT RML.ri_model_line_id,RA.ri_arrangement_id,rml.ri_model_id ,rml.treaty_id ,t.code  
 FROM RI_Model_Line RML    
 LEFT JOIN RI_Arrangement RA ON RML.ri_model_id =RA.ri_model_id  
 left join treaty t on rml.treaty_id =t.treaty_id   
 where RA.ri_arrangement_id=@ri_arrangement_id