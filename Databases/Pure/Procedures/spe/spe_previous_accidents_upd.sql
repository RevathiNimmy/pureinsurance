SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spe_previous_accidents_upd'
GO

CREATE PROCEDURE spe_previous_accidents_upd  
@party_cnt int,  
@previous_accidents_id int,   
@date datetime,   
@description varchar(70),   
@is_at_fault tinyint  
  
AS  
  
  
UPDATE previous_accidents  
SET [date] = @date,   
[description] = @description,   
is_at_fault = @is_at_fault  
WHERE previous_accidents_id = @previous_accidents_id  
AND party_cnt =@party_cnt  
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
