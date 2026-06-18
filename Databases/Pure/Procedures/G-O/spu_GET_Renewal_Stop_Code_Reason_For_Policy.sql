
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_GET_Renewal_Stop_Code_Details_For_Policy'
GO

CREATE PROCEDURE spu_GET_Renewal_Stop_Code_Details_For_Policy
 @insurance_file_cnt INT,  
 @RenewalMethodID INT OUTPUT,  
 @RenewalMethodDescription VARCHAR(255) OUTPUT,  
 @RenewalStopCodeID INT OUTPUT,  
 @RenewalStopCodeDescription VARCHAR(255) OUTPUT  
AS  
BEGIN  

    SELECT
        @RenewalMethodID = INSF.renewal_method_id,
        @RenewalMethodDescription = RM.description,
        @RenewalStopCodeID = INSF.renewal_stop_code_id,
        @RenewalStopCodeDescription = RSC.description
    FROM
        insurance_file INSF
        LEFT OUTER JOIN renewal_stop_code RSC
            ON INSF.renewal_stop_code_id = RSC.renewal_stop_code_id
        LEFT OUTER JOIN renewal_method RM
            ON INSF.renewal_method_id = RM.renewal_method_id
    WHERE 
        INSF.insurance_file_cnt = @insurance_file_cnt

END  

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
