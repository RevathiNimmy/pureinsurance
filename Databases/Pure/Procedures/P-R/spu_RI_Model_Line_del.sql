SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_RI_Model_Line_del'
GO


CREATE PROCEDURE spu_RI_Model_Line_del      
    @ri_model_line_id INT = NULL,      
    @ri_model_id INT = NULL,      
    @audit_ri_model_id INT,    
    @UserId INT,    
    @UniqueId VARCHAR(50),    
    @ScreenHierarchy VARCHAR(500)    
AS  
BEGIN  
    -- Step 1: Insert the data into the Audit_ri_model_line table for the relevant rows  
    INSERT INTO Audit_ri_model_line      
    SELECT @audit_ri_model_id,      
           ri_model_line_id,      
           ri_model_id,      
           priority,      
           number_of_lines,      
           line_limit,      
           Treaty_id,      
           Share_percent,      
           is_obligatory  ,
		   premium_calculation_basis_Id
    FROM RI_Model_Line      
    WHERE ri_model_line_id = @ri_model_line_id      
       OR ri_model_id = @ri_model_id;  
  
    IF @ri_model_id IS NOT NULL  
    BEGIN  

		UPDATE ril  
          SET ScreenHierarchy = CONCAT(@ScreenHierarchy, '/Treaty Line(', LTRIM(RTRIM(t.description)), ')'),  
               UniqueId = @UniqueId,  
			   UserId = @UserId  
		FROM RI_Model_Line  ril INNER JOIN Treaty t ON ril.treaty_id=t.treaty_id
        WHERE ri_model_id = @ri_model_id;   
       
		DELETE FROM ri_model_line      
        WHERE ri_model_id = @ri_model_id;  
    END  
  
    IF @ri_model_line_id IS NOT NULL      
    BEGIN  
        DELETE FROM ri_model_line      
        WHERE ri_model_line_id = @ri_model_line_id;  
    END  
  
END  

GO


