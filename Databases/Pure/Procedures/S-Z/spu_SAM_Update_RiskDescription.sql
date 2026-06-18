Execute DDLDropProcedure 'spu_SAM_Update_RiskDescription'
GO

CREATE PROCEDURE spu_SAM_Update_RiskDescription

@risk_cnt int,  
@riskdescription Varchar(255) ,
@risk_inception_date Datetime = null   
AS  
If @riskdescription <>''    
UPDATE Risk
SET description = @riskdescription
WHERE risk_cnt = @risk_cnt  
Else  
If @risk_inception_date Is NOT Null  
UPDATE Risk    
SET inception_date  = @risk_inception_date    
WHERE risk_cnt = @risk_cnt 
GO
