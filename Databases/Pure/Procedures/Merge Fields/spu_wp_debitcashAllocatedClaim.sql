DDLDROPPROCEDURE 'spu_wp_debitcashAllocatedClaim'
GO
CREATE PROCEDURE spu_wp_debitcashAllocatedClaim
	@PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
select cl.Claim_Number,cl.Policy_Number,pc.code 'primary_cause',ad.alloc_ccy_amount,cl.insurer_short_name 'agentcode',cl.Client_name 'client_name',cl.client_short_name 'client_code' 
from document d 
join transdetail td on d.document_id=td.document_id
join allocationdetail ad on ad.transdetail_id=td.transdetail_id
join stats_folder sf on d.document_ref=sf.document_ref
join claim cl on cl.claim_id=sf.loss_id
join primary_cause pc on pc.primary_cause_id=cl.Primary_Cause_id
where d.documenttype_id in 
(select documenttype_id from documenttype where code in ('CLP','CLA','CLO','CLR'))
and ad.allocationdetail_id=@Instance2  
GO