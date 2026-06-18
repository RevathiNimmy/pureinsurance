SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_sir_report_code_sel'
GO

CREATE PROCEDURE spu_sir_report_code_sel   
 -- Add the parameters for the stored procedure here  
 @ReportID int  
AS  
BEGIN  
 -- SET NOCOUNT ON added to prevent extra result sets from  
 -- interfering with SELECT statements.  
 SET NOCOUNT ON;  
    
 select Code from  report where report_id=@ReportID  
END 
GO