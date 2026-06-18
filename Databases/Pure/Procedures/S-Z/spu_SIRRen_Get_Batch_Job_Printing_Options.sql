--**********************************
-- Author : Pankaj Kaushik
--   
-- History: 28/07/2008    
--
-- Task : WR9 Batch Renewals
--***********************************
EXECUTE DDLDropProcedure 'spu_SIRRen_Get_Batch_Job_Printing_Options'
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_SIRRen_Get_Batch_Job_Printing_Options
     @Batch_Renewal_Job_Id INT
  
AS  
BEGIN  
  
SELECT  
        renewal_docs_destination,  
        report_sort_order  
  
 FROM Batch_Renewal_Job WHERE batch_renewal_job_id = @Batch_Renewal_Job_Id
  
END  
GO
