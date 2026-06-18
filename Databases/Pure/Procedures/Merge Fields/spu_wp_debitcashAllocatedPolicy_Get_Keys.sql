DDLDROPPROCEDURE 'spu_wp_debitcashAllocatedPolicy_Get_Keys'
GO  
CREATE PROCEDURE spu_wp_debitcashAllocatedPolicy_Get_Keys  
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskID INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
select ad2.allocationdetail_id 'allocationdetail_id' from document d  
join transdetail td on d.document_id=td.document_id  
join allocationdetail ad on ad.transdetail_id=td.transdetail_id  
join allocation a on ad.allocation_id=a.allocation_id  
join allocationdetail ad2 on ad2.allocation_id=a.allocation_id  
join transdetail td2 on ad2.transdetail_id=td2.transdetail_id  
join document d2 on d2.document_id=td2.document_id  
where d2.documenttype_id in  
(select documenttype_id from documenttype where code in ('SND','SED','SEC','SID','SRD','INC','JN','CLP','IND'))  
and d.document_ref=@DocumentRef  
GO