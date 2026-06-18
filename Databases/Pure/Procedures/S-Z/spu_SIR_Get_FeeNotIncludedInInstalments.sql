SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_FeeNotIncludedInInstalments'
GO

/**********************************************************************************************************************************
** Created by Vivek Gupta
** 02 March 2006
** BPIS-Partial Instalment
**
**********************************************************************************************************************************/






CREATE  PROCEDURE spu_SIR_Get_FeeNotIncludedInInstalments 
    @Insurance_File_Cnt int
   
AS
DECLARE
        @total_Risk_Fees money,
	@total_Policy_Fees money,
	@total_Risk_Fees_ELFF money,
	@total_Policy_Fees_ELFF money,
	@total_Risk_Fees_EXFF money,
	@total_Policy_Fees_EXFF money,
	@total_Risk_Fees_ELFD money,
	@total_Policy_Fees_ELFD money

/*Total Fees at Risk Level */
 SELECT  @total_Risk_Fees = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf INNER JOIN risk r ON pf.risk_cnt = r.risk_cnt
		INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = pf.risk_cnt
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
		AND r.is_risk_selected = 1
     AND NOT pf.risk_cnt is NULL
 -- AND NOT fa.tax_group_id IS NULL

/*Total Fee at Policy Level */
SELECT  @total_Policy_Fees = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
    AND pf.risk_cnt is NULL
  --AND NOT fa.tax_group_id IS NULL

/* Total Fee at Risk Level where 'Include Fee in Instlment' is Yes */
SELECT  @total_Risk_Fees_ELFF = SUM(ISNULL(pf.currency_amount,0))
		FROM    policy_fee_u pf INNER JOIN risk r ON pf.risk_cnt = r.risk_cnt
		INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = pf.risk_cnt
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
		AND r.is_risk_selected = 1
  AND NOT pf.risk_cnt is NULL AND pf.include_fee_in_instalments=1

/* Total Fee at Policy Level where 'Include Fee in Instlment' is Yes */
SELECT  @total_Policy_Fees_ELFF = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
  AND pf.risk_cnt is NULL AND pf.include_fee_in_instalments=1

/*  Total Fee at Risk Level where 'Include Fee in Instlment' is No */
SELECT  @total_Risk_Fees_EXFF = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf INNER JOIN risk r ON pf.risk_cnt = r.risk_cnt
		INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = pf.risk_cnt
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
		AND r.is_risk_selected = 1
  AND NOT pf.risk_cnt is NULL AND pf.include_fee_in_instalments=0

/* Total Fee at Policy Level where 'Include Fee in Instlment' is No */
SELECT  @total_Policy_Fees_EXFF = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
  AND pf.risk_cnt IS NULL AND pf.include_fee_in_instalments=0

/* Total Risk Fees for Deposit */
SELECT  @total_Risk_Fees_ELFD = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf INNER JOIN risk r ON pf.risk_cnt = r.risk_cnt
		INNER JOIN insurance_file_risk_link ifrl ON ifrl.risk_cnt = pf.risk_cnt
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
		AND r.is_risk_selected = 1
  AND NOT pf.risk_cnt IS NULL AND (pf.include_fee_in_instalments=1 AND pf.spread_fee_across_instalments=0)

/* Total Policy Fees for Deposit */
SELECT  @total_Policy_Fees_ELFD = SUM(ISNULL(pf.currency_amount,0))
        FROM    policy_fee_u pf
        WHERE   pf.insurance_file_cnt = @Insurance_File_Cnt
  AND pf.risk_cnt IS NULL AND (pf.include_fee_in_instalments=1 AND pf.spread_fee_across_instalments=0)

select isnull(@total_Risk_Fees,0) ,
	isnull(@total_Policy_Fees,0) ,
	isnull(@total_Risk_Fees_ELFF,0) ,
	isnull(@total_Policy_Fees_ELFF,0) ,
	isnull(@total_Risk_Fees_EXFF,0) ,
	isnull(@total_Policy_Fees_EXFF,0) ,
	isnull(@total_Risk_Fees_ELFD,0) ,
	isnull(@total_Policy_Fees_ELFD,0)




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

