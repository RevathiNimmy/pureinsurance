/* 
		Purpose : E007 
		Stored Precedure name : spu_PortfolioTransfer_RI_Usage_ins
		SQL SERVER 2005 
*/

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
EXECUTE DDLDropProcedure 'spu_PortfolioTransfer_RI_Usage_ins'
GO

CREATE PROCEDURE spu_PortfolioTransfer_RI_Usage_ins
(  
 @insurance_file_cnt int,
 @transferDate Datetime,  
 @ins_file_PT_RI_usage_id  int OUTPUT  
)  
AS  
 
 DECLARE @iPTRIStatusID int  
   
 -- get the id for the manual review status  

SELECT  
    @iPTRIStatusID = PT_RI_status_type_id  
FROM  
    PT_RI_Status_Type  
WHERE  
    code = 'MANREVIEW' 

SET @iPTRIStatusID = ISNULL(@iPTRIStatusID, 1) -- default id for 'MANREVIEW'  
 
-- check that our record doesn't exist already, dupes would be 'bad'  
IF NOT EXISTS  
    (  
    SELECT  
        ins_file_PT_RI_usage_id  
    FROM  
        Insurance_File_PT_RI_Usage  
    WHERE  
        insurance_file_cnt = @insurance_file_cnt  
    )  
BEGIN  
    INSERT INTO  
     Insurance_File_PT_RI_Usage  
    (  
     insurance_file_cnt,  
     status, transferDate  
    )  
    VALUES  
    (  
     @insurance_file_cnt,  
     1, @transferDate
    )  
END  
  
SELECT @ins_file_PT_RI_usage_id = @@IDENTITY
