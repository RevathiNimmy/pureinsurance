ddldropprocedure spu_TXN_policy_fee_fetch
go

CREATE PROCEDURE spu_TXN_policy_fee_fetch
(
@from_event bit,
@insurance_file_cnt int
)
AS

IF @from_event=0
	SELECT
	PF.party_cnt,
	CASE PT.code
	WHEN 'FE' THEN 1
	WHEN 'IN' THEN 2
	WHEN 'EX' THEN 3
	WHEN 'DI' THEN 4
	ELSE 0
	END AS Type,
	P.name,
	PF.fee_amount,
	ISNULL(TC.tax_group_id,0),
	ISNULL(TG.code,''),
	ISNULL(TC.percentage,0)/100,
	ISNULL(TC.value,0),
	PF.Total_Fee,
	PF.commission_percentage/100,
	PF.commission_amount,
	PF.insurer_fee_type,
	FTOS.Fsa_Type_Of_Sale_Id,
        CASE WHEN FTOS.Fsa_Type_Of_Sale_Id = -1 THEN
            NULL
        ELSE
	    FTOS.Description
	END,
	PF.fee_percentage,
	P.shortname,
	PE.fee_charge  
	FROM
	policy_fee PF
	INNER JOIN party P ON PF.party_cnt=P.party_cnt
	INNER JOIN party_type PT ON pt.party_type_id=P.party_type_id
	LEFT OUTER JOIN tax_calculation TC ON TC.policy_fee_id=PF.policy_fee_id
	LEFT OUTER JOIN tax_group TG ON TC.tax_group_id=TG.tax_group_id
	LEFT OUTER JOIN Fsa_Type_Of_Sale FTOS ON PF.Fsa_Type_Of_Sale_Id=FTOS.Fsa_Type_Of_Sale_Id
	LEFT OUTER JOIN party_extra PE ON PF.party_cnt = PE.party_cnt
	WHERE
	PF.insurance_file_cnt=@insurance_file_cnt
	ORDER BY    
        pf.policy_fee_id
ELSE
	SELECT
	PF.party_cnt,
	CASE PT.code
	WHEN 'FE' THEN 1
	WHEN 'IN' THEN 2
	WHEN 'EX' THEN 3
	WHEN 'DI' THEN 4
	ELSE 0
	END AS Type,
	P.name,
	PF.fee_amount,
	ISNULL(TC.tax_group_id,0),
	ISNULL(TG.code,''),
	ISNULL(TC.percentage,0)/100,
	ISNULL(TC.value,0),
	PF.Total_Fee,
	PF.commission_percentage/100,
	PF.commission_amount,
	PF.insurer_fee_type,
	FTOS.Fsa_Type_Of_Sale_Id,		
        CASE WHEN FTOS.Fsa_Type_Of_Sale_Id = -1 THEN
            NULL
        ELSE
	    FTOS.Description
	END,
	PF.fee_percentage,
	P.shortname,
	PE.fee_charge 
	FROM
	event_policy_fee PF
	INNER JOIN party P ON PF.party_cnt=P.party_cnt
	INNER JOIN party_type PT ON pt.party_type_id=P.party_type_id
	LEFT OUTER JOIN event_tax_calculation TC ON TC.policy_fee_id=PF.policy_fee_id
	LEFT OUTER JOIN tax_group TG ON TC.tax_group_id=TG.tax_group_id
	LEFT OUTER JOIN Fsa_Type_Of_Sale FTOS ON PF.Fsa_Type_Of_Sale_Id=FTOS.Fsa_Type_Of_Sale_Id
	LEFT OUTER JOIN party_extra PE ON PF.party_cnt = PE.party_cnt
	WHERE
	PF.insurance_file_cnt=@insurance_file_cnt
	ORDER BY    
        pf.policy_fee_id