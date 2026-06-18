
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_Sel_Latest_Valid_Plan'
GO


CREATE PROCEDURE spu_PFPremiumFinance_Sel_Latest_Valid_Plan
     @insurancefilecnt INTEGER,
     @sBusiness_type VARCHAR(10) = NULL
AS

DECLARE @sFilter1 AS VARCHAR(10)
DECLARE @sFilter2 AS VARCHAR(10)
DECLARE @sFilter3 AS VARCHAR(10)  
DECLARE @dtInception_date_tpi AS DATETIME
DECLARE @nInsurance_folder_cnt AS INTEGER
DECLARE @nPFprem_finance_cnt INTEGER
DECLARE @nPFprem_finance_version INTEGER
DECLARE @sPFStatusIndSaved  AS VARCHAR(10)= '010'
DECLARE @sPFStatusIndLive  AS VARCHAR(10)= '040'
DECLARE @sPFStatusIndOnHold  AS VARCHAR(10)= '140'
DECLARE @sPFStatusIndCompleted  AS VARCHAR(10)= '900'


SELECT	@nInsurance_folder_cnt = insurance_folder_cnt, 
		@dtInception_date_tpi = inception_date_tpi
FROM	Insurance_File
WHERE	insurance_file_cnt = @insurancefilecnt

IF @sbusiness_type = 'MTA' OR @sbusiness_type = 'REN'
    BEGIN
		SET @sFilter1 = @sPFStatusIndLive
		SET @sFilter2 = @sPFStatusIndCompleted
    END
ELSE IF @sbusiness_type = 'MTC' 
	BEGIN  
		SET @sFilter1 = @sPFStatusIndSaved
		SET @sFilter2 = @sPFStatusIndLive 
		SET @sFilter3 = @sPFStatusIndOnHold
	END
    ELSE
    BEGIN
		SET @sFilter1 = @sPFStatusIndLive
    END

SELECT TOP 1 @nPFprem_finance_cnt=P.pfprem_finance_cnt, 
			 @nPFprem_finance_version=P.pfprem_finance_version,
			 @insurancefilecnt = P.insurance_file_cnt
FROM		 PFPremiumFinance P
			 INNER JOIN insurance_file ifl ON ifl.insurance_file_cnt=P.insurance_file_cnt
WHERE		 ifl.insurance_folder_cnt=@nInsurance_folder_cnt
		     AND	ifl.inception_date_tpi=@dtInception_date_tpi
			 AND P.StatusInd IN (@sFilter1, @sFilter2,@sFilter3)
ORDER BY ifl.insurance_file_cnt DESC, P.pfprem_finance_version DESC

IF @nPFprem_finance_cnt IS NOT NULL
	EXEC spu_PFPremiumFinance_Sel_SingleFromInsuranceFileCount @insurancefilecnt


GO
