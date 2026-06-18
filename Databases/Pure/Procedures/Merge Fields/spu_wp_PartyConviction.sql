
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_PartyConviction'
GO

CREATE  PROCEDURE spu_wp_PartyConviction    
    @PartyCnt INT,    
    @InsuranceFileCnt INT,    
    @RiskId INT = NULL,    
    @ClaimCnt INT,    
    @DocumentRef VARCHAR(25),    
    @Instance1 INT,    
    @Instance2 INT,    
    @Instance3 INT    
AS   
SELECT   
    PC.code,    
  --convert (nvarchar,cast (PC.conviction_date as datetime),106) as conviction_date,
   PC.conviction_date as  conviction_date,
   PC.description ,    
    PC.fine_amt ,    
    PC.sentence_code ,    
    PC.sentence_description ,    
    PC.sentence_duration ,    
    PC.sentence_duration_qualifier ,    
  -- convert (nvarchar,cast (PC.sentence_effective_date as datetime),106) as sentence_effective_date,
    PC.sentence_effective_date as sentence_effective_date,

    PC.status_code ,    
    PC.alcohol_level ,    
    PC.alcohol_measurement_method ,    
    PC.driving_licence_penalty_pts,
    p.ccjs  

From Party_conviction PC Join 
Party P On
PC.party_cnt=P.party_cnt
 
WHERE PC.party_cnt=@PartyCnt  
AND Party_conviction_Id= @Instance2  
GO
