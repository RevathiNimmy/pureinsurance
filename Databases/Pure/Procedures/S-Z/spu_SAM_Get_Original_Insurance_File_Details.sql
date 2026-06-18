SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Get_Original_Insurance_File_Details'
GO

CREATE PROCEDURE spu_SAM_Get_Original_Insurance_File_Details

@insurance_file_cnt int 

AS 
DECLARE @sPFStatusIndLive  AS VARCHAR(10)= '040'
DECLARE @sPFStatusIndCompleted  AS VARCHAR(10)= '900'
DECLARE @nInsurance_folder_cnt AS INTEGER
DECLARE @dtInception_date_tpi AS DATETIME

SELECT @nInsurance_folder_cnt = insurance_folder_cnt,
              @dtInception_date_tpi = inception_date_tpi
FROM   Insurance_File
WHERE  insurance_file_cnt = @insurance_file_cnt


SELECT TOP 1 
             P.insurance_file_cnt AS original_insurance_file_cnt,
             P.pfprem_finance_cnt,
             P.pfprem_finance_version AS pfprem_finance_version,
						   P.party_bank_id
FROM          PFPremiumFinance P
INNER JOIN    insurance_file ifl 
              ON     ifl.insurance_file_cnt=P.insurance_file_cnt
WHERE         ifl.insurance_folder_cnt=@nInsurance_folder_cnt
                           AND    ifl.inception_date_tpi=@dtInception_date_tpi
                           AND P.StatusInd IN (@sPFStatusIndLive,@sPFStatusIndCompleted)
ORDER BY      ifl.insurance_file_cnt DESC, 
                     P.pfprem_finance_version DESC                

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

