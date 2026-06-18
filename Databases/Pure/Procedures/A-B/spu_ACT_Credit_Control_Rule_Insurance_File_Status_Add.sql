SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Credit_Control_Rule_Insurance_File_Status_Add'
GO

CREATE PROCEDURE spu_ACT_Credit_Control_Rule_Insurance_File_Status_Add    
@credit_control_rule_id INT,   
@insurance_file_status_id INT,
@UserId INT,
@UniqueId VARCHAR(50),
@ScreenHierarchy VARCHAR(500)
  
AS  
  
INSERT INTO Credit_Control_Rule_Insurance_File_Status  
(credit_control_rule_id,   
insurance_file_status_id,
UserId,
UniqueId,
ScreenHierarchy
)  
  
VALUES  
  
(@credit_control_rule_id,   
@insurance_file_status_id,
@UserId,
@UniqueId,
@ScreenHierarchy
)  


GO
