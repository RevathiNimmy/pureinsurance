SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_party_consultant
GO

CREATE PROCEDURE spu_wp_party_consultant
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
        @forename = pc.forename,  
        @initials = pc.initials,  
        @department = d.description,  
        @party_title_code = pc.party_title_code  
    FROM    
        party_consultant pc
        LEFT OUTER JOIN department d  
            ON pc.department_id = d.department_id  
    WHERE 
        pc.party_cnt = @PartyCnt  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO