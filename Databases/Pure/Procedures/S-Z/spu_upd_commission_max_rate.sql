SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_upd_commission_max_rate'
GO
CREATE PROCEDURE spu_upd_commission_max_rate  
 @insurance_file_cnt int,
 @risk_type varchar(50),
 @commission_band varchar(50)

As
BEGIN  
DECLARE @Party_Agent_Type_id As Int
DECLARE @Party_cnt As Int
DECLARE @Product_id As Int
DECLARE @Risk_Type_id As Int
DECLARE @transaction_type_id As Int
DECLARE @Commission_band_id As Int
DECLARE @effective_date As DateTime
DECLARE @rate As NUMERIC(19, 10)
DECLARE @Is_Value TINYINT
DECLARE @tax_group_id As Int
DECLARE @transaction_type VARCHAR(10)
DECLARE @maximum_rate NUMERIC(19, 4)

SELECT @Risk_Type_id=r.risk_type_id,@Party_Agent_Type_id=PA.party_agent_type_id,@Party_cnt=P.party_cnt,
@Product_id=IF1.product_id,@Commission_band_id=ac.commission_band_id,
@effective_date=IF1.cover_start_date,@transaction_type=Case IFT.code WHEN 'QUOTE' THEN  'NB'
WHEN 'RENEWAL' THEN 'REN' WHEN 'MTAQUOTE' THEN 'MTA' WHEN 'MTAQCAN' THEN 'MTC' WHEN 'MTAQREINS' THEN 'MTR'  End
,@transaction_type_id=Case IFT.code WHEN 'QUOTE' THEN  4
WHEN 'RENEWAL' THEN 10 WHEN 'MTAQUOTE' THEN 9 WHEN 'MTAQCAN' THEN 7 WHEN 'MTAQREINS' THEN 20  End
 FROM Insurance_file IF1
JOIN Insurance_File_risk_link IFRL
ON IF1.insurance_file_cnt=IFRL.insurance_file_cnt
JOIN Risk r ON r.risk_cnt=IFRL.risk_cnt
JOIN risk_type rt On r.risk_type_id=rt.risk_type_id
JOIN Party P ON p.party_cnt=IF1.lead_agent_cnt
JOIN Party_Agent PA ON p.party_cnt=PA.party_cnt
JOIN Agent_Commission ac ON ac.insurance_file_cnt=if1.insurance_file_cnt
JOIN Commission_Band CB ON ac.commission_band_id=CB.commission_band_id
JOIN Insurance_File_Type IFT ON IFT.insurance_file_type_id=IF1.insurance_file_type_id
 WHERE IF1.insurance_file_cnt=@insurance_file_cnt AND rt.code=@risk_type AND CB.code=@commission_band
EXECUTE spu_Sir_Calc_Commission_Rate
     @Party_Agent_Type_id,
     @Party_cnt,
     @Product_id,
     @Risk_Type_id,
     @transaction_type_id,
     @Commission_band_id,
     @effective_date,
     @rate OUT,
     @Is_Value OUT,
     @tax_group_id OUT,
     @transaction_type,
     @insurance_file_cnt,
     1,
     @maximum_rate OUT
		Update Agent_Commission SET Maximum_rate=@maximum_rate WHERE insurance_file_cnt=@insurance_file_cnt  AND risk_type_id=@Risk_Type_id
	
END  

GO

