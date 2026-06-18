DDLDROPPROCEDURE 'spu_wp_debitcashAllocatedPolicy'  
GO
CREATE PROCEDURE spu_wp_debitcashAllocatedPolicy
 @PartyCnt INT,    
    @InsuranceFileCnt INT,    
    @RiskID INT,    
    @ClaimCnt INT,    
    @DocumentRef VARCHAR(25),    
    @Instance1 INT,    
    @Instance2 INT,    
    @Instance3 INT    
AS    
    
select ifi.insurance_ref,p.description,ifi.insured_name,ad.alloc_ccy_amount,ag.shortname 'agentcode',ag.tax_number 'agent_tax_number',c.shortname 'clientcode', c.tax_number 'client_tax_number', 

CASE WHEN d.document_ref like 'INC%' THEN 'GROSS' ELSE td.spare END 'spare' ,d.document_ref

from document d    
join transdetail td on d.document_id=td.document_id    
join allocationdetail ad on ad.transdetail_id=td.transdetail_id    
join insurance_file ifi on ifi.insurance_file_cnt=d.insurance_file_cnt    
join product p on ifi.product_id=p.product_id    
left join party ag on ag.party_cnt=ifi.lead_agent_cnt    
join party c on c.party_cnt=ifi.insured_cnt    
where d.documenttype_id in    
(select documenttype_id from documenttype where code in ('SND','SED','SEC','SID','SRD','IND','INC','JN','CLP'))    
and ad.allocationdetail_id=@Instance2 
GO