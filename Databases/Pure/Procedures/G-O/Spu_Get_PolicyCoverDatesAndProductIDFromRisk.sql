
EXECUTE DDLDropProcedure 'Spu_Get_PolicyCoverDatesAndProductIDFromRisk'
GO

Create Procedure Spu_Get_PolicyCoverDatesAndProductIDFromRisk
@Risk_cnt int
AS 

Select cover_start_date , expiry_date, product_id from insurance_file
Where insurance_file_cnt = (Select insurance_file_cnt from 	
			Insurance_file_risk_link 
			Where risk_cnt = @Risk_cnt)

GO


