SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_party_account_handler
GO

CREATE PROCEDURE spu_wp_party_account_handler
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @forename VARCHAR(60) OUTPUT,  
    @initials VARCHAR(20) OUTPUT,  
    @department VARCHAR(255) OUTPUT,  
    @party_title_code VARCHAR(255) OUTPUT  
AS 
BEGIN
  
    SELECT  
        @forename = pah.forename,  
        @initials = pah.initials,  
        @department = d.description,  
        @party_title_code = pah.party_title_code  
    FROM    
        party_account_handler pah
        LEFT OUTER JOIN department d  
            ON pah.department_id = d.department_id
    WHERE 
        pah.party_cnt = @PartyCnt  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

