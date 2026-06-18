SET QUOTED_IDENTIFIER OFF
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Products_List_For_PickList'
GO

CREATE PROCEDURE spu_SIR_Get_Products_List_For_PickList

	@BatchRenewalJobId	INT = NULL

As
BEGIN

	SELECT product_id,description,0 
	FROM Product 
	WHERE is_deleted = 0 AND
	product_id NOT IN (SELECT product_id FROM Batch_Renewal_Job_Products WHERE batch_renewal_job_id = @BatchRenewalJobId)  
	UNION ALL SELECT BRJP.product_id,  
	P.description,1  
	FROM Batch_Renewal_Job_Products BRJP  
	LEFT JOIN Product P ON BRJP.product_id = P.product_id  
	WHERE BRJP.batch_renewal_job_id = @BatchRenewalJobId 

END

GO