SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_event_update_insurance_file_from_COB_sections'
GO
 
CREATE PROCEDURE spu_event_update_insurance_file_from_COB_sections
	@Insurance_file_cnt integer
AS
 
DECLARE @Premium_Excluding_Tax numeric(19,4),
 	@Tax_applied Numeric(19,4),
	@Premium_Including_Tax Numeric(19,4),	
 	@Commission_Payable Numeric(19,4),
	@Broker_Cnt integer,
	@Insurer VARCHAR(20)

SELECT 	@Premium_Excluding_Tax=ISNULL(SUM(premium_excluding_tax),0),
	@Tax_Applied=ISNULL(SUM(tax_applied),0),
	@Premium_Including_tax=ISNULL(SUM(premium_including_tax),0),
	@Commission_payable=ISNULL(SUM(commission_payable),0),
	@Broker_Cnt=ISNULL(MIN(commission_cnt),0)
	FROM event_insurance_COB_section where insurance_file_Cnt = @Insurance_File_Cnt

SELECT @Insurer = (SELECT P.shortname FROM Party P
		   JOIN event_insurance_File I ON I.lead_insurer_cnt = P.party_cnt 
		   WHERE I.insurance_file_cnt=@Insurance_file_cnt)

IF (SELECT @Insurer) = 'MULTI'
BEGIN
	SELECT @Commission_payable=ISNULL(SUM(coinsurer_commission_amount),0)
	FROM event_policy_coinsurers where insurance_file_Cnt = @Insurance_File_Cnt
END  

UPDATE event_insurance_file
	SET this_premium=@Premium_Including_Tax,
	net_premium=@premium_excluding_tax,
	commission_amount=@commission_payable,
	tax_amount=@tax_applied,
	broker_cnt=@broker_cnt,
	brokerage_amount=0,
	is_minimum_brokerage_flag=0,
	iptable_amount=0,
	ipt_percentage=0,
	is_ipt_overridden=0,
	vatable_amount=0,
	vat_percentage=0,
	vat_amount=0,
	commission_percentage=0,
        is_insurer_rate_table=0
	WHERE insurance_file_Cnt = @Insurance_File_Cnt
 	AND policy_type_id = 3
 
GO
  
 
