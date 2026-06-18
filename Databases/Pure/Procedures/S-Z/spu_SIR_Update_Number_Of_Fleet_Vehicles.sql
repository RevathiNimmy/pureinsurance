SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Update_Number_Of_Fleet_Vehicles'
GO

CREATE PROCEDURE spu_SIR_Update_Number_Of_Fleet_Vehicles  
  
@insurance_file_cnt int,  
@number_of_fleet_vehicles int  
AS  
  
BEGIN  
  
 UPDATE insurance_file  
 SET number_of_fleet_vehicles=@number_of_fleet_vehicles  
 WHERE insurance_file_cnt =@insurance_file_cnt  
  
END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

