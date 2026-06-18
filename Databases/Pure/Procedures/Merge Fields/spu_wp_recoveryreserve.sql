SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure spu_wp_recoveryreserve
GO

CREATE  PROCEDURE spu_wp_recoveryreserve
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

SELECT  rt.description recovery_type_description,    
    cur.description recovery_curr_description,    
    sum(r.initial_reserve) recovery_Initial_Reserve,    
    sum(r.revised_reserve) recovery_Revised_Reserve,    
    sum(r.received_to_date) recovery_Paid_To_Date,    
    sum(r.initial_reserve + r.revised_reserve - r.received_to_date) recovery_current_reserve,    
    --Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)    
    p.resolved_name    
    --End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc)    
    
FROM    claim_peril cp,    
    recovery_type rt,       
    currency cur,
    recovery r left join party p    
    ON  r.recovery_party_cnt=p.party_cnt  
    
WHERE   cp.claim_id = @ClaimCnt    
AND cp.claim_peril_id = @Instance2    
AND r.claim_peril_id = cp.claim_peril_id    
AND r.recovery_id = @instance3    
AND rt.recovery_type_id = r.recovery_type_id    
AND r.currency_id = cur.currency_id    
AND rt.is_salvage = 0    

group by cur.description, rt.description, p.resolved_name    


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

