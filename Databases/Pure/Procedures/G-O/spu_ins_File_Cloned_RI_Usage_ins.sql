SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ins_File_Cloned_RI_Usage_ins'
GO

CREATE PROCEDURE spu_ins_File_Cloned_RI_Usage_ins
(  
 @insurance_file_cnt             int,
 @ins_file_cloned_RI_usage_id  int OUTPUT  
)  
AS  
  
-- get the id for the manual review status  
  
-- check that our record doesn't exist already, dupes would be 'bad'  
IF NOT EXISTS  
    (  
    SELECT  
        ins_file_cloned_RI_usage_id  
    FROM  
        Insurance_File_cloned_RI_Usage  
    WHERE  
        insurance_file_cnt = @insurance_file_cnt  
    )  
BEGIN  
    INSERT INTO  
     Insurance_File_cloned_RI_Usage  
    (  
     insurance_file_cnt,  
     status 
    )  
    VALUES  
    (  
     @insurance_file_cnt,  
     1
    )  
END  
  
SELECT @ins_file_cloned_RI_usage_id = @@IDENTITY

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO