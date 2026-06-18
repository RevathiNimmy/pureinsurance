SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_get_Commission_tax_group'
GO

CREATE PROCEDURE spu_SIR_get_commission_tax_group
	@InsurerCnt int,
    	@InsuranceFileCnt int,
    	@CommissionTaxGroupId int OUTPUT
AS


 
BEGIN
DECLARE @PartyType char(2)

DECLARE	@EffectiveDate datetime 
DECLARE @RiskSectionId integer
DECLARE @RiskCodeId integer
DECLARE @RiskGroupId integer
DECLARE @SchemeId integer


DECLARE @Rate1 numeric(19,4) 
DECLARE @Value1 numeric(19,4)
DECLARE @MinimumTotal1 numeric(19,4)
DECLARE @Rate2 numeric(19,4) 
DECLARE @Value2 numeric(19,4)
DECLARE @MinimumTotal2 numeric(19,4)
DECLARE @Rate3 numeric(19,4) 
DECLARE @Value3 numeric(19,4)
DECLARE @MinimumTotal3 numeric(19,4) 

SELECT @PartyType = (SELECT pt.code FROM Party_Type PT
			JOIN party P ON P.party_type_id = PT.party_type_id
			WHERE P.party_cnt = @InsurerCnt)

SELECT @effectivedate = getdate()

IF @PartyType = 'IN'
BEGIN


	SELECT @SchemeId=I.scheme,@RiskGroupId =RC.risk_group_id,@RiskCodeId=I.risk_code_id  FROM risk_code RC
					    JOIN Insurance_File I ON I.risk_code_id = RC.risk_code_id
					    WHERE I.Insurance_File_Cnt = @InsuranceFileCnt
	SELECT @RiskSectionId =(SELECT ISNULL(MIN(RTU.COB_Rating_Section_Id),0)
				FROM Risk_Tax_Usage RTU
				JOIN Insurance_File I ON I.risk_code_id = RTU.risk_code_id
				WHERE I.insurance_file_cnt = @InsuranceFileCnt)



 

	exec spu_get_commissionrates @InsurerCnt,@SchemeId,@RiskGroupId,@RiskCodeId,@RiskSectionId,@EffectiveDate,
	@Rate1 OUTPUT,@Value1 OUTPUT,@MinimumTotal1 OUTPUT,@Rate2 OUTPUT,@Value2 OUTPUT,@MinimumTotal2 OUTPUT,
	@Rate3 OUTPUT,@Value3 OUTPUT,@MinimumTotal3 OUTPUT,@CommissionTaxGroupId OUTPUT
END
ELSE
BEGIN
 
	SELECT @RiskGroupId = (SELECT RC.risk_group_id FROM Risk_Code RC
				JOIN insurance_file I ON I.risk_code_id = RC.risk_Code_id
				WHERE I.insurance_file_Cnt = @InsuranceFileCnt)
 
	SELECT @CommissionTaxGroupId =(SELECT ISNULL(MAX(FA.tax_group_id),0)
				FROM Fee_Amounts FA
 				WHERE FA.party_cnt = @InsurerCnt
				AND FA.risk_group_id = @RiskGroupId
				 
			  )
 
  
END

END
GO