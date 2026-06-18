SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_Get_Default_Payment_Terms'
GO

CREATE PROCEDURE spu_SAM_Get_Default_Payment_Terms
	@nInsuranceFileCnt INT,
	@bIsSelectionMode TINYINT=0
AS

	DECLARE @sPaymentMethod VARCHAR(50)
	DECLARE @nInstalmentPlan INT 
	DECLARE @nInstalmentPlanVersion INT 
	DECLARE @nSchemeNo INT
	DECLARE @nSchemeVersion INT
	DECLARE @nInstalmentInsuranceFileCnt INT

    DECLARE @bUse_nb_payment_term_at_renselection TINYINT
    DECLARE @bIsTrueMonthlyPolicy TINYINT
    DECLARE @nInsurance_File_TypeID INT
    DECLARE @nInsurance_Folder_cnt INT
    DECLARE @nInstalmentPlanCount INT
    DECLARE @dtInceptionTPIDateReplacedVersion DATETIME
    DECLARE @dtInceptionTPIDateCurrentVersion DATETIME
    DECLARE @nNBRENInsuranceFileCnt INT
    DECLARE @dtCoverStartDate DATETIME

    DECLARE @sInsurance_File_TypeCode CHAR(10)
    DECLARE @IsAutoInstalment INT = 0
    DECLARE @IsAutoInstalment_Option_Number INT = 1024
    DECLARE @Branch_Id INT
    DECLARE @sCurrentPaymentMethod VARCHAR(50)
    DECLARE @dtPreferredDate DATETIME
    DECLARE @nDayInMonth INT
	DECLARE @sFrequency VARCHAR(10)

	
	SET @nInstalmentPlan=0
	SET @nInstalmentPlanVersion=0
	SET @nSchemeNo=0
	SET @nSchemeVersion=0
		
		SELECT @bUse_nb_payment_term_at_renselection=ISNULL(PROD.use_nb_payment_term_at_renselection,0),
				@bIsTrueMonthlyPolicy =ISNULL(PROD.is_true_monthly_policy,0),
				@nInsurance_File_TypeID=INF.insurance_file_type_id,
				@nInsurance_Folder_cnt =INF.insurance_folder_cnt,
				@dtInceptionTPIDateCurrentVersion =INF.inception_date_tpi,
				@dtCoverStartDate=INF.cover_start_date,
				@sInsurance_File_TypeCode=IFT.code,
				@Branch_Id = INF.branch_id,
                @sCurrentPaymentMethod=INF.payment_method
				FROM Product PROD
				JOIN Insurance_file INF on INF.product_id=PROD.product_id
				LEFT JOIN Insurance_File_Type IFT on INF.insurance_file_type_id=IFT.insurance_file_type_id
				WHERE INF.insurance_file_cnt=@nInsuranceFileCnt

		SELECT @IsAutoInstalment = value from System_Options where option_number= @IsAutoInstalment_Option_Number and branch_id = @Branch_Id
		If @IsAutoInstalment = 1 
		BEGIN
			SET @bUse_nb_payment_term_at_renselection = 0
		END

        IF @bIsSelectionMode=1 OR @nInsurance_File_TypeID = (SELECT INFT.insurance_file_type_id from Insurance_File_Type INFT where UPPER(LTRIM(RTRIM(INFT.code))) = 'RENEWAL')
				SELECT @dtInceptionTPIDateReplacedVersion=@dtInceptionTPIDateCurrentVersion
		ELSE
		SELECT @dtInceptionTPIDateReplacedVersion=inception_date_tpi
				FROM Insurance_file WHERE Insurance_File_Status_id=3
				AND insurance_folder_cnt =@nInsurance_Folder_cnt

	--SET DEFAULTS
	   IF  (@nInsurance_File_TypeID=2 OR @nInsurance_File_TypeID=3) AND  @bIsTrueMonthlyPolicy<>1
			BEGIN
				SELECT @sPaymentMethod='INVOICE',
				@nInstalmentPlan =0,
				@nInstalmentPlanVersion =0

				SELECT @sPaymentMethod = ISNULL(INF.payment_method,'INVOICE') FROM Insurance_File INF where INF.insurance_file_cnt = @nInsuranceFileCnt
			END

		IF @nInsurance_File_TypeID <> 3 AND @sInsurance_File_TypeCode <> 'VOID' AND @bIsSelectionMode=0
		BEGIN
		   SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
			@nInstalmentPlanVersion=PF.pfprem_finance_version,
			@sPaymentMethod=INF.payment_method,
			@nDayInMonth = pf.DayOfWeekOrMonth
			FROM PFPremiumFinance PF
			JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
			LEFT JOIN Insurance_File_Type INFTYPE ON INFTYPE.insurance_file_type_id = INF.insurance_file_type_id 
			WHERE INF.insurance_file_cnt=@nInsuranceFileCnt
			AND INF.inception_date_tpi=@dtInceptionTPIDateCurrentVersion
			AND INFTYPE.code IN ('POLICY','MTA PERM','MTAREINS','VOIDREP','VOIDRENREP')
			AND (PF.StatusInd in ('040','900'))
			ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC

			IF ISNULL(@nInstalmentPlan,0)=0
			BEGIN
				SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
				@nInstalmentPlanVersion=PF.pfprem_finance_version,
				@sPaymentMethod=INF.payment_method,
				@nDayInMonth = pf.DayOfWeekOrMonth
				FROM PFPremiumFinance PF
				JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
				WHERE INF.insurance_folder_cnt=@nInsurance_Folder_cnt
				AND INF.inception_date_tpi=@dtInceptionTPIDateCurrentVersion
				AND INF.insurance_file_type_id in (2,5,9)
				AND (PF.StatusInd in ('040','900'))
				ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC
			END
		END

		IF (@nInsurance_File_TypeID=3 OR @bIsSelectionMode=1) AND @bUse_nb_payment_term_at_renselection=0
		BEGIN

		    SELECT @nInstalmentPlanCount=Count(PF.pfprem_finance_cnt)
			FROM PFPremiumFinance PF
			JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
			WHERE INF.insurance_folder_cnt =@nInsurance_Folder_cnt
			AND  INF.inception_date_tpi=@dtInceptionTPIDateReplacedVersion
			AND (PF.StatusInd='040' or PF.StatusInd='900' or PF.StatusInd='010' or PF.StatusInd='011')

			IF @nInstalmentPlanCount=0
			BEGIN
				SELECT @sPaymentMethod='INVOICE',
				@nInstalmentPlan =0,
				@nInstalmentPlanVersion =0				
			END

			IF @nInstalmentPlanCount>0
			BEGIN
				SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
				@nInstalmentPlanVersion=PF.pfprem_finance_version,
                @dtPreferredDate =PF.first_instalment_date,
				@nDayInMonth=pf.DayOfWeekOrMonth
				FROM PFPremiumFinance PF
				JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
				WHERE INF.insurance_folder_cnt =@nInsurance_Folder_cnt
				AND INF.inception_date_tpi=@dtInceptionTPIDateReplacedVersion
				AND (PF.StatusInd='040' or PF.StatusInd='900' or PF.StatusInd='010' or PF.StatusInd='011')
				ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC
			END
		END

		IF (@nInsurance_File_TypeID=3 AND @bIsSelectionMode=0) 
		BEGIN
		IF (UPPER(@sCurrentPaymentMethod)<>'INVOICE' AND UPPER(@sCurrentPaymentMethod)<>'PAYNOW' AND UPPER(@sCurrentPaymentMethod)<>'CREDITCARD')
			BEGIN
			SELECT TOP 1 @nNBRENInsuranceFileCnt = insurance_file_cnt
			FROM Insurance_File INF
			WHERE INF.insurance_folder_cnt=@nInsurance_Folder_cnt
			AND  INF.inception_date_tpi =@dtInceptionTPIDateCurrentVersion
			AND INF.insurance_file_type_id =3
			ORDER by insurance_file_cnt ASC

			SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
			@nInstalmentPlanVersion=PF.pfprem_finance_version,
			@sPaymentMethod=INF.payment_method
			FROM PFPremiumFinance PF
			JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
			WHERE INF.insurance_file_cnt=@nNBRENInsuranceFileCnt
			ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC

			--IF ON NB WE WERE ON INSTALMENT MAKE CHECK IF SELECTION HAS A PLAN IF SO THEN PICK THAT
			IF ISNULL(@nInstalmentPlan,0)>0 AND EXISTS( SELECT pfprem_finance_cnt
										FROM PFPremiumFinance PF
										JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
										WHERE INF.insurance_folder_cnt =@nInsurance_Folder_cnt
										AND INF.inception_date_tpi=@dtInceptionTPIDateCurrentVersion
										AND PF.StatusInd IN ('040','900','010','011'))
			BEGIN
				SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
				@nInstalmentPlanVersion=PF.pfprem_finance_version,
				@dtPreferredDate =pf.first_instalment_date,
				@nDayInMonth=pf.DayOfWeekOrMonth
				FROM PFPremiumFinance PF
				JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
				WHERE INF.insurance_folder_cnt =@nInsurance_Folder_cnt
				AND INF.inception_date_tpi=@dtInceptionTPIDateCurrentVersion
				AND PF.StatusInd IN ('040','900','010','011')
				ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC
			END
			END
		END

		ELSE IF (@nInsurance_File_TypeID=3 OR @bIsSelectionMode=1)AND @bUse_nb_payment_term_at_renselection=1
		BEGIN

			SELECT TOP 1 @nNBRENInsuranceFileCnt = insurance_file_cnt
			FROM Insurance_File INF
			WHERE INF.insurance_folder_cnt=@nInsurance_Folder_cnt
			AND  INF.inception_date_tpi =@dtInceptionTPIDateReplacedVersion
			AND INF.insurance_file_type_id in (2,3,5,9)
			ORDER by insurance_file_cnt ASC

			SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
			@nInstalmentPlanVersion=PF.pfprem_finance_version,
			@sPaymentMethod=INF.payment_method,
            @dtPreferredDate =pf.first_instalment_date,
			@nDayInMonth=pf.DayOfWeekOrMonth
			FROM PFPremiumFinance PF
			JOIN Insurance_File INF on PF.Insurance_File_Cnt=INF.insurance_file_cnt
			WHERE INF.insurance_file_cnt=@nNBRENInsuranceFileCnt
			ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC

		END
		IF @sInsurance_File_TypeCode='MTAQUOTE' OR @sInsurance_File_TypeCode='MTAQTETEMP' OR @sInsurance_File_TypeCode='MTAQREINS'
		BEGIN

			SET @nInstalmentPlan=0
			SET @nInstalmentPlanVersion=0
			SET @sPaymentMethod=''

			SELECT TOP 1 @nInstalmentPlan= PF.pfprem_finance_cnt,
			@nInstalmentPlanVersion=PF.pfprem_finance_version,
			@sPaymentMethod=INF.payment_method
			FROM PFPremiumFinance PF
			JOIN Insurance_File INF ON PF.Insurance_File_Cnt=INF.insurance_file_cnt
			WHERE INF.insurance_folder_cnt=@nInsurance_Folder_cnt AND PF.StatusInd in ('040','900') 
			AND INF.inception_date_tpi = @dtInceptionTPIDateCurrentVersion AND INF.cover_start_date <= @dtCoverStartDate
			ORDER BY PF.pfprem_finance_cnt DESC,PF.pfprem_finance_version DESC

			IF ISNULL(@nInstalmentPlan,0)=0
			BEGIN
				
				SELECT TOP 1 @sPaymentMethod=IFF.payment_method FROM Insurance_File IFF
				JOIN Insurance_File_Type IFT ON IFF.insurance_file_type_id=IFT.insurance_file_type_id
				WHERE insurance_folder_cnt=@nInsurance_Folder_cnt AND insurance_file_cnt<>@nInsuranceFileCnt 
				AND inception_date_tpi <= @dtInceptionTPIDateCurrentVersion AND cover_start_date <= @dtCoverStartDate
				AND IFT.code IN ('POLICY','MTA PERM','MTA TEMP','MTAREINS') AND IFF.payment_method NOT IN('INSTALMENT','INSTALMENTS','Direct Debit','PremiumFinance')
				ORDER BY insurance_file_cnt DESC
				
				SET @nInstalmentPlanVersion=0
				SET @nInstalmentPlan=0
			END
		END
		IF ISNULL(@nInstalmentPlan,0)>0
			BEGIN
				SET @sPaymentMethod='INSTALMENT'
				SET @nInstalmentPlan=ISNULL(@nInstalmentPlan,0)
				SET @nInstalmentPlanVersion=ISNULL(@nInstalmentPlanVersion,0)
                SET @dtPreferredDate =@dtPreferredDate
				SET @nDayInMonth=ISNULL(@nDayInMonth,0)
				SELECT @nSchemeNo=P.SchemeNo,  
					@nSchemeVersion=P.SchemeVersion,  
					@nInstalmentInsuranceFileCnt = P.Insurance_File_Cnt, 
					@sFrequency= PFF.code
					FROM PFPremiumFinance  P 
					INNER JOIN   PFScheme S  ON  S.companyno = P.companyno  AND  S.schemeno = P.schemeno  AND  S.schemeversion = P.schemeversion  
					INNER JOIN   PFRF  PF   ON  PF.pfrf_id = P.pfrf_id  
					INNER JOIN   PFFrequency PFF   ON  PFF.pffrequency_id = PF.pFfrequency_id  
					WHERE pfprem_finance_cnt=@nInstalmentPlan  
					AND pfprem_finance_version=@nInstalmentPlanVersion  
			END

			IF @sInsurance_File_TypeCode = 'MTA PERM  '
			BEGIN
				SELECT TOP 1 @sPaymentMethod=IFF.payment_method FROM Insurance_File IFF
				JOIN Insurance_File_Type IFT ON IFF.insurance_file_type_id=IFT.insurance_file_type_id
				WHERE insurance_folder_cnt=@nInsurance_Folder_cnt AND insurance_file_cnt = @nInsuranceFileCnt 
				AND inception_date_tpi <= @dtInceptionTPIDateCurrentVersion AND cover_start_date <= @dtCoverStartDate
				AND IFT.code IN ('MTA PERM') AND IFF.payment_method NOT IN('INSTALMENT','INSTALMENTS','Direct Debit','PremiumFinance')
				ORDER BY insurance_file_cnt DESC
			END

		IF Isnull(@sPaymentMethod,'')=''
		SET @sPaymentMethod='INVOICE'
		--FINAL OUTPUT
		SELECT  UPPER(@sPaymentMethod) AS 'DefaultPaymentMethod',
				@nInstalmentPlan AS 'DefaultInstalmentPlan',
				@nInstalmentPlanVersion AS 'DefaultInstalmentPlanVersion',
				ISNUll(@nSchemeNo,0) AS 'DefaultSchemeNumber',
				ISNUll(@nSchemeVersion,0) AS	'DefaultSchemeVersion',
				ISNUll(@nInstalmentInsuranceFileCnt,0) AS	'InstalmentInsuranceFileCnt',
				@dtPreferredDate AS	'SavedPreferredDate',
				ISNUll(@nDayInMonth,0) AS	'SavedDayInMonth',
				UPPER(@sFrequency) AS 'Frequency' 
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
