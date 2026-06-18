SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_get_claim_clipol_id
GO

CREATE PROCEDURE spu_get_claim_clipol_id
    @Claim_id INT
AS  
BEGIN   

    DECLARE  
        @sShortName VARCHAR(20),  
        @AgentUnderwriter VARCHAR(1)  
  
    SELECT  @AgentUnderwriter = value  
    FROM    hidden_options  
    WHERE   branch_id = 1 and option_number = 1  
  
    IF ISNULL(@AgentUnderwriter, ' ') = ' '  
        SELECT @AgentUnderwriter = 'A'  
  
    IF @AgentUnderwriter = 'A'  
        SELECT DISTINCT c.policy_id, 
                        inf.insurance_holder_cnt, 
                        ins.lead_agent_cnt, cp.insurer_type,  
                        (SELECT SUM(r.Initial_reserve)  
                         FROM Reserve r
                             INNER JOIN Claim_peril cp
                                 ON r.claim_peril_id=cp.claim_peril_id      
                         WHERE cp.claim_id = @claim_id  
                        ) total_reserve, 
                        p.shortname,
                        ins.insurance_folder_cnt  
        FROM claim c
            INNER JOIN insurance_file ins
                ON ins.insurance_file_cnt = c.policy_id 
            INNER JOIN insurance_folder inf
                ON ins.insurance_folder_cnt = inf.insurance_folder_cnt
            INNER JOIN party p  
                ON p.party_cnt = inf.insurance_holder_cnt  
            LEFT OUTER JOIN claim_party cp
                ON cp.claim_id = c.claim_id  
                AND cp.insurer_type = 1 
        WHERE   
            c.claim_id = @claim_id
            
    ELSE  
  
        -- do we have any reinsurer/fac Reinsurer apart from Retained?  
        SELECT @sShortName = MIN(ISNULL(p.ShortName, p.ShortName))  
        FROM claim_ri_arrangement_line ral  
            LEFT OUTER JOIN treaty_party tp 
                ON tp.treaty_id = ral.treaty_id  
            LEFT OUTER JOIN party p 
                ON p.party_cnt IN (tp.party_cnt, ral.party_cnt)  
        WHERE   ral.type <> 'R'  
            AND ral.claim_id = @claim_id  
  
    SELECT ifi.insurance_file_cnt,  
           ifi.insured_cnt,  
           ifi.lead_agent_cnt,  
           @sShortName AS shortname,  
           (  
            SELECT SUM(r.Initial_reserve) + SUM(r.revised_reserve)  
            FROM Reserve r 
                JOIN Claim_peril cp 
                    ON r.claim_peril_id = cp.claim_peril_id  
            WHERE cp.claim_id = @Claim_id  
           ) AS total_incurred,  
           ' ',  
           ifi.insurance_folder_cnt,  
           c.Document_Generated_Status  
    FROM Claim c 
        JOIN Insurance_File ifi 
            ON c.policy_id = ifi.insurance_file_cnt  
    WHERE c.claim_id = @Claim_id  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO