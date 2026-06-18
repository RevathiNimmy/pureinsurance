SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_Risk_Type_Rating_section_type_Del  
GO

CREATE PROCEDURE spu_Risk_Type_Rating_section_type_Del  
	@risk_type_id INT,  
	@rating_section_type_id INT,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS  

UPDATE Risk_Type_Rating_Section_Type SET
	UserId = @UserId,
	UniqueId = @UniqueId,
	ScreenHierarchy = @ScreenHierarchy
	WHERE    
    risk_type_id=@risk_type_id
And rating_section_type_id in  
(SELECT rating_section_type_id FROM rating_section_type WHERE is_deleted = 0) 

DELETE from    
    Risk_Type_Rating_section_type   
WHERE    
    risk_type_id=@risk_type_id
And rating_section_type_id in   
(SELECT rating_section_type_id FROM rating_section_type WHERE is_deleted = 0)  
  
