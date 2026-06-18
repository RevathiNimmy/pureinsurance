SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyLoyalityScheme'
GO

CREATE  PROCEDURE spu_wp_PartyLoyalityScheme    
    @PartyCnt INT,    
    @InsuranceFileCnt INT,    
    @RiskId INT = NULL,    
    @ClaimCnt INT,    
    @DocumentRef VARCHAR(25),    
    @Instance1 INT,    
    @Instance2 INT,    
    @Instance3 INT    
AS   
SELECT  Ls.code,    
ls.description,    
pls.Membership_Number,    
pls.other_reference,    
pls.start_date,    
pls.end_date,    
pls.main_membership_number    
FROM Party_Loyalty_Scheme pls join     
Loyalty_Scheme ls on    
 pls.loyalty_scheme_id = ls.loyalty_scheme_id    
WHERE party_cnt = @PartyCnt    
AND Party_Loyalty_Scheme_Id= @Instance2

GO

