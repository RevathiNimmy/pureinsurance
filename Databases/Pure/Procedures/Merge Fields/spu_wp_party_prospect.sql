SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_wp_party_prospect
GO

CREATE PROCEDURE spu_wp_party_prospect
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @ClaimCnt INT,  
    @agent_reference VARCHAR(20) OUTPUT,  
    @current_intermediary VARCHAR(100) OUTPUT,  
    @prospect_status VARCHAR(255) OUTPUT  
AS
BEGIN  
  
    SELECT  
        @agent_reference = pp.agent_reference,  
        @current_intermediary = p.resolved_name,  
        @prospect_status = ps.description  
    FROM    
        party_prospect pp
        LEFT OUTER JOIN party p
            ON pp.current_intermediary = p.party_cnt       
        LEFT OUTER JOIN prospect_status ps  
            ON pp.prospect_status_id = ps.prospect_status_id  
    WHERE 
        pp.party_cnt = @PartyCnt

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

