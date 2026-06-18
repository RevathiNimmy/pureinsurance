--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 18/06/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Job_Details'
GO


CREATE PROCEDURE spu_SIRRen_Get_Job_Details
     @Batch_Renewal_Job_Code VARCHAR(20)
  
AS  
BEGIN  

SELECT 
	BRJ.batch_renewal_job_id,
        BRJ.code,
        BRJ.is_active,
        BRJ.batch_renewal_job_type_id,
        BRJT.code,
        BRJ.renewal_docs_destination,
        BRJ.report_sort_order,
        BRJ.sam_server,
		BRJ.description AS Batch_Job_Description, 
		BRJT.Description As Batch_Job_Type_Description,
		ISNULL(BRJ.run_extended_rule ,0) run_extended_rule

	FROM Batch_Renewal_Job BRJ
	LEFT JOIN batch_renewal_job_type BRJT ON BRJT.batch_renewal_job_type_id = BRJ.batch_renewal_job_type_id
        Where BRJ.code = @Batch_Renewal_Job_Code 
	AND BRJ.is_active = 1 

END  
GO

