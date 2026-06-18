SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Copy_TransExportDetail_for_Cloned_Reversal'
GO

Create Procedure spu_Copy_TransExportDetail_for_Cloned_Reversal 
@ClonedInsuranceFileCnt  INT,  
@transaction_export_folder_cnt INT,
@Stats_folder_cnt INT   
  
AS

Declare @Old_transaction_export_folder_cnt INT

Select @Old_transaction_export_folder_cnt = MIN(transaction_export_folder_cnt) from Transaction_Export_Folder where insurance_file_cnt = @ClonedInsuranceFileCnt
And transaction_export_folder_cnt Not In (@transaction_export_folder_cnt) and loss_id Is Null And transaction_type_code not like 'DRI%'

Insert Into Transaction_Export_Detail
Select @transaction_export_folder_cnt,
      transaction_export_detail_id,
      Case When transdetail_type_code ='GROSS' Then 0 Else -transaction_amount END,
      transaction_ledger_code,
      account_type_code,
      transaction_account_key,
      ceded_ref,
      cover_share_percent,
      Case When transdetail_type_code ='GROSS' Then 0 Else -sum_insured_total END,
      Case When transdetail_type_code ='GROSS' Then 0 Else -charges_total END,
      Case When transdetail_type_code ='GROSS' Then 0 Else -taxes_total END,
      -recoveries_total,
      -commission_excluded,
      -withholding_tax_excluded,
      mapping_code,
      spare,
      purchase_order_no,
      purchase_invoice_no,
      base_transaction_amount,
      base_taxes_amount,
      suspended,
      release_to_income,
      release_account_code,
      transdetail_type_code,
      tax_group_id,
      tax_band_id,
      manually_released,
      released_on_full_settlement,
      released_for_whole_posting,
      released_on_policy_effective ,
     fee_type
From Transaction_Export_Detail 
Where transaction_export_folder_cnt = @Old_transaction_export_folder_cnt 
and (transdetail_type_code Like 'REIN%' Or mapping_code in (SELECT ri_shortname
															FROM stats_detail
															WHERE stats_folder_cnt = @Stats_folder_cnt
															AND Stats_detail_type IN ('TAN', 'TAX')
															AND tax_type_code IS NOT NULL
															GROUP BY tax_type_code, ri_shortname, stats_detail_type) 
            or transdetail_type_code Like ('GROSS'))


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
