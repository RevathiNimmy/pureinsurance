ddldropprocedure 'spu_PFEDI_GetPolicyDetailsMTA'
go

CREATE PROCEDURE spu_PFEDI_GetPolicyDetailsMTA
(
    @pfprem_finance_cnt INT,
    @pfprem_finance_version INT,
    @InsuranceFileCnt INT
)
AS
SELECT DISTINCT
        t.insurance_ref,
	iff.cover_start_date,
	iff.expiry_date,
	iff.renewal_date,
	PI.name,
	RC.description,
	PRD.description,
	'',
 	t.amount,
	isnull((select sum(t2.amount) from 
	transdetail t2
	join account a2 on a2.account_id=t2.account_id
	join Ledger l2 ON l2.ledger_id = a2.ledger_id
	where l2.ledger_short_name='FE'
	and t2.document_id=t.document_id),0) fees,

	isnull((select sum(t3.amount) from 
	transdetail t3
	join account a3 on a3.account_id=t3.account_id
	join Party p3 ON p3.party_cnt=a3.account_key
	Join Party_Type pt3 on pt3.party_type_id=p3.party_type_id
	where pt3.code='EX'
	and t3.document_id=t.document_id),0) extras,
   	PI.abi_code_on_81 insurer_abi_number,
	RG.abi_code
    FROM
        PFTransaction_id PFT
Inner JOIN Transdetail T on t.transdetail_id = PFT.pftransaction_id
Inner JOIN Insurance_File IFF on iff.insurance_file_cnt=pft.insurance_file_cnt
Inner JOIN Party PI ON PI.party_cnt=IFF.lead_insurer_cnt
Inner JOIN Product PRD ON PRD.product_id=IFF.product_id
Left JOIN Risk_Code RC ON RC.risk_code_id=IFF.risk_code_id  
LEFT JOIN Risk_Group RG ON RC.risk_group_id=RG.risk_group_id  
WHERE
	PFT.pfprem_finance_cnt=@pfprem_finance_cnt AND
	PFT.pfprem_finance_version=@pfprem_finance_version AND
	PFT.insurance_file_cnt=@InsuranceFileCnt
