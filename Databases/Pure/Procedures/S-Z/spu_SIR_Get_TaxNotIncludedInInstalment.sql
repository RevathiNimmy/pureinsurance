SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_TaxNotIncludedInInstalment'
GO

CREATE  PROCEDURE spu_SIR_Get_TaxNotIncludedInInstalment

    @Insurance_File_Cnt int

AS
DECLARE
        @total_Risk_Tax money,
	@total_Policy_Tax money,
	@total_Risk_Tax_EFF money,
	@total_Policy_Tax_EFF money,
	@total_Risk_Tax_EXFF money,
	@total_Policy_Tax_EXFF money,
	@total_Risk_Tax_EFD money,
	@total_Policy_Tax_EFD money,
	@total_Risk_Tax_Client money,
	@total_Policy_Tax_Client money,
	@total_Risk_Tax_NonClient money,
	@total_Policy_Tax_NonClient money,
	@total_Tax_EFF money

/*Total Tax at Risk Level */

 SELECT  @total_Risk_Tax = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	LEFT JOIN risk r WITH(NOLOCK)
        ON r.risk_cnt = t.risk_cnt
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is not NULL AND r.is_risk_selected=1

/*Total Tax at Policy Level */

 SELECT @total_Policy_Tax = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is NULL
	AND pfprem_finance_cnt IS NULL
/*Total Tax at Risk Level Where is_not_applied_to_client is NO */

 SELECT  @total_Risk_Tax_Client = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	LEFT JOIN risk r WITH(NOLOCK)
        ON r.risk_cnt = t.risk_cnt
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is not NULL AND r.is_risk_selected=1 AND t.is_not_applied_to_client=0
    AND transtype not in ('TTRITP','TTRITC')
    
/*Total Tax at Policy Level Where is_not_applied_to_client is NO*/

 SELECT  @total_Policy_Tax_Client = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is NULL  AND t.is_not_applied_to_client=0 
    AND pfprem_finance_cnt IS NULL
/*Total Tax at Risk Level Where is_not_applied_to_client is NO and Include tax in instalment is Yes */

 SELECT  @total_Risk_Tax_EFF = SUM(ISNULL(t.value,0))
	 FROM Tax_Calculation t WITH(NOLOCK)
	LEFT JOIN risk r WITH(NOLOCK)
        ON r.risk_cnt = t.risk_cnt
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is not NULL AND r.is_risk_selected=1 AND t.is_not_applied_to_client=0 and t.include_tax_in_instalments=1
    AND transtype not in ('TTRITP','TTRITC')
/*Total Tax at Policy Level Where is_not_applied_to_client is NO and Include tax in instalment is Yes */

 SELECT  @total_Policy_Tax_EFF = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is NULL AND t.is_not_applied_to_client=0 and t.include_tax_in_instalments=1
	AND pfprem_finance_cnt IS NULL
/*Total Tax at Risk Level Where is_not_applied_to_client is NO and Include tax in instalment is No */

 SELECT  @total_Risk_Tax_EXFF = SUM(ISNULL(t.value,0))
	 FROM Tax_Calculation t WITH(NOLOCK)
	LEFT JOIN risk r WITH(NOLOCK)
        ON r.risk_cnt = t.risk_cnt
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is not NULL AND r.is_risk_selected=1 AND t.include_tax_in_instalments=0
	AND transtype not in ('TTRITP','TTRITC')


/*Total Tax at Policy Level Where is_not_applied_to_client is NO and Include tax in instalment is No */

 SELECT  @total_Policy_Tax_EXFF = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is NULL AND t.is_not_applied_to_client=0 and t.include_tax_in_instalments=0
	AND pfprem_finance_cnt IS NULL
	AND transtype not in ('TTAC')
/*Total Tax Deposit at Risk Level Where is_not_applied_to_client is NO and Include tax in instalment is Yes and Spread tax across instalment is NO */

 SELECT  @total_Risk_Tax_EFD = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	LEFT JOIN risk r WITH(NOLOCK)
        ON r.risk_cnt = t.risk_cnt
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is not NULL AND r.is_risk_selected=1 AND t.is_not_applied_to_client=0 and t.include_tax_in_instalments=1
	AND t.spread_tax_across_instalments=0

 /*Total Tax Deposit at Policy Level Where is_not_applied_to_client is NO and Include tax in instalment is Yes and Spread tax across instalment is NO */

 SELECT  @total_Policy_Tax_EFD = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is NULL  AND t.is_not_applied_to_client=0 and t.include_tax_in_instalments=1
	AND t.spread_tax_across_instalments=0
	AND pfprem_finance_cnt IS NULL

/*Total Tax at Risk Level Where is_not_applied_to_client is YES*/
 SELECT @total_Risk_Tax_NonClient = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	LEFT JOIN risk r WITH(NOLOCK)
        ON r.risk_cnt = t.risk_cnt
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is not NULL AND r.is_risk_selected=1 AND t.is_not_applied_to_client=1 

/*Total Tax at Policy Level Where is_not_applied_to_client is YES*/
 SELECT @total_Policy_Tax_NonClient = SUM(ISNULL(t.value,0))
	FROM Tax_Calculation t WITH(NOLOCK)
	WHERE t.Insurance_File_Cnt=@Insurance_File_Cnt
	AND t.risk_cnt is NULL  AND t.is_not_applied_to_client=1
	AND pfprem_finance_cnt IS NULL
/*Total amount at Where Include amount in instalment is No*/  
 SELECT  @total_Tax_EFF = SUM(ISNULL(t.value + PFU.base_fee_amount,0))    
       	FROM Tax_Calculation t WITH(NOLOCK)    
 	INNER JOIN Policy_fee_u PFU WITH(NOLOCK)
	ON	PFU.insurance_file_cnt = t.Insurance_file_cnt	
       	WHERE 	t.Insurance_File_Cnt=@Insurance_File_Cnt  
       	and 	t.include_tax_in_instalments=0  
	and 	PFU.include_fee_in_instalments=0 

 SELECT

	isnull(@total_Risk_Tax,0) ,
	isnull(@total_Policy_Tax,0) ,
   	isnull(@total_Risk_Tax_EFF,0),
   	isnull(@total_Policy_Tax_EFF,0),
   	isnull(@total_Risk_Tax_EXFF,0),
	isnull(@total_Policy_Tax_EXFF,0),
	isnull(@total_Risk_Tax_EFD,0),
	isnull(@total_Policy_Tax_EFD,0),
	isnull(@total_Risk_Tax_Client,0),
	isnull(@total_Policy_Tax_Client,0),
	isnull(@total_Risk_Tax_NonClient,0),
	isnull(@total_Policy_Tax_NonClient,0),
        isnull( @total_Tax_EFF,0)




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
