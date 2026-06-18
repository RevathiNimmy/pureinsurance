SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_InsuranceFileDetails_From_Financialplan'
GO

Create PROCEDURE spu_Get_InsuranceFileDetails_From_Financialplan
  @nPFprem_finance_cnt int,
  @nPFprem_finance_version int
AS
  BEGIN
    Select IF1.insurance_file_cnt,IF1.insurance_folder_cnt,IF1.insured_cnt,IF1.insurance_ref,cur.iso_code  FROM PFPremiumFinance PF
      JOIN Insurance_File IF1 ON
      IF1.insurance_file_cnt =PF.Insurance_File_Cnt
    JOIN Currency cur ON
    IF1.currency_id=cur.currency_id
      WHERE PF.pfprem_finance_cnt = @nPFprem_finance_cnt
      AND PF.pfprem_finance_version = @nPFprem_finance_version
  END
GO