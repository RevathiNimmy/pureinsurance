SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Get_Payment_HUB_Configurations'
GO

CREATE PROCEDURE spu_Get_Payment_HUB_Configurations  
  
AS    
  
SELECT distinct option_number,ISNULL(value,'') 'Value',description FROM system_options  
WHERE option_number in (5185,  
5186,  
5187,  
5188,  
5189,  
5190,  
5191,  
5192,  
5193,  
5194,  
5195,  
5196,  
5197,  
5198,  
5199,
5203,
5204,
5205,
5241)