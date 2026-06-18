SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_Risk_Type_Rating_section_type_Add  
GO

CREATE PROCEDURE spu_Risk_Type_Rating_section_type_Add    
 @risk_type_id INT,  
 @rating_section_type_id INT,  
 @UserId INT = NULL,
 @UniqueId VARCHAR(50) = NULL,
 @ScreenHierarchy VARCHAR(500) = NULL
AS    
    
INSERT INTO    
    Risk_Type_Rating_section_type (risk_type_id,rating_section_type_id,UserId,UniqueId,ScreenHierarchy)    
VALUES    
    (@risk_type_id,@rating_section_type_id,@UserId,@UniqueId,@ScreenHierarchy)    
