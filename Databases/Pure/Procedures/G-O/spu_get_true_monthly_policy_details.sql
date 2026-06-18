SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_true_monthly_policy_details'
GO

CREATE PROCEDURE spu_get_true_monthly_policy_details 
	@insurance_file_cnt int,
	@is_true_monthly_policy tinyint OUTPUT,
	@lead_month_in_cycle tinyint OUTPUT,
	@sub_month_in_cycle tinyint OUTPUT,
	@lead_allow_consolidated_commission tinyint OUTPUT,
	@sub_allow_consolidated_commission tinyint OUTPUT,
	@transaction_type_code VARCHAR(10) OUTPUT,
	@renewal_count int OUTPUT

  AS

SELECT Distinct
	@is_true_monthly_policy=ISNULL(p.is_true_monthly_policy,0),
	@lead_month_in_cycle=ISNULL(p.lead_month_in_cycle,0),
	@sub_month_in_cycle=ISNULL(p.sub_month_in_cycle,0),
	@lead_allow_consolidated_commission=ISNULL(ifi.lead_allow_consolidated_commission,0),
	@sub_allow_consolidated_commission=ISNULL(ifi.sub_allow_consolidated_commission,0),
	@transaction_type_code=sf.transaction_type_code,
	@renewal_count=ISNULL(ifo.renewal_count,0)
	
    FROM Insurance_File ifi
    JOIN Product p ON p.product_id = ifi.product_id
	JOIN Insurance_Folder ifo ON ifo.insurance_folder_cnt = ifi.insurance_folder_cnt
    LEFT JOIN stats_folder sf ON sf.insurance_file_cnt = ifi.insurance_file_cnt
WHERE  ifi.insurance_file_cnt=@insurance_file_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
