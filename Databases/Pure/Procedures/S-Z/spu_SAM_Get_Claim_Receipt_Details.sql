SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Claim_Receipt_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Claim_Receipt_Details

@claim_id integer

AS

DECLARE      @enumClaimReceivable   char(1) = '0'
            ,@enumParty             char(1) = '1'
            ,@enumAgent             char(1) = '2'
            ,@enumClient            char(1) = '3'
            ,@enumInsurer           char(1) = '4'
            
DECLARE      @enumAgentCode   nvarchar(2) = 'AG'
            ,@enumClientCode  nvarchar(2) = 'PC'
            ,@enumInsurerCode nvarchar(2) = 'IN'
	SELECT 
		cr.claim_receipt_id,
		cr.claim_id,
		cr.claim_peril_id,
		cr.date_of_receipt,
		cr.party_cnt,
		p.shortname,
		cr.Amount,
		cr.tax_amount,
		cr.comments,
		cr.created_by,
		cr.insured_domiciled,
		cr.insured_percentage,
		cr.insured_tax_number,
		cr.receivable_tax_percentage,
		cr.is_tax_exempt,
		cr.is_settlement,		
		LTRIM(RTRIM(convert(varchar,mt.code))) as 'PayeeMediaType',
		cr.PayeeName,
		cr.PayeeBankName,
		cr.PayeeSortCode,
		cr.PayeeAccountNo,
  con.code PayeeCountry,  
		cr.PayeeComments,
		cr.PayeeMediaRef,
		cr.document_id,
		cr.currency_id,
		cur.code as currrency_code,
		cr.PayeeAddress1,
		cr.PayeeAddress2,
		cr.PayeeAddress3,
		cr.PayeeAddress4,
		cr.PayeePostalCode,
		cr.ThirdPartyReference,
		cr.base_claim_receipt_id,
		cr.version_id,
		p.party_type_id,  
		(case when cr.party_cnt = 0       then @enumClaimReceivable -- Claim Payable
		when pt.Code   = @enumAgentCode   then @enumAgent           -- Agent(AG)
		when pt.Code   = @enumClientCode  then @enumClient          -- Personal Client(PC)
		when pt.Code   = @enumInsurerCode then @enumInsurer         -- Insurer(IN)
		else @enumParty end)party_type                              -- Party 
	
	FROM Claim_Receipt cr

		LEFT JOIN Party p ON 
			p.party_cnt = cr.party_cnt
	
		LEFT JOIN Currency cur ON 
			cur.currency_id = cr.currency_id
	
  LEFT JOIN Country con ON  
   cr.PayeeCountry = con.country_id  
   LEFT JOIN Party_Type pt ON      
   p.party_type_id = pt.party_type_id  
   Left Join MediaType MT On MT.mediatype_id=cr.PayeeMediaType 

	WHERE claim_id = @claim_id





GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
