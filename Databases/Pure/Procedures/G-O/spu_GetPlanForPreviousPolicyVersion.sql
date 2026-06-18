SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS OFF
GO

EXEC DDLDropProcedure 'spu_GetPlanForPreviousPolicyVersion'
GO

CREATE PROCEDURE spu_GetPlanForPreviousPolicyVersion
	@Insurance_Folder_Cnt int,
	@Current_Policy_Version int
AS

DECLARE @Previous_Policy_Version int

-- Get previous policy version (file type limited)
SELECT  @Previous_Policy_Version = MAX(InFi2.Policy_version)
FROM    Insurance_File AS InFi2
WHERE   InFi2.Insurance_Folder_Cnt = @Insurance_Folder_Cnt 
        -- Get only policy versions prior to this one
AND     InFi2.Policy_version < @Current_Policy_version
        -- Only include InsFileTypes POLICY, MTA PERM & MTAREINS
AND     InFi2.Insurance_File_Type_ID In (2, 5, 9)


-- Get associated "LIVE" finance plan
SELECT  PF.pfprem_finance_cnt, 
        PF.pfprem_finance_version, 
        InFi.Insurance_File_Cnt
FROM    Insurance_File AS InFi 
JOIN    PFPremiumFinance AS PF ON InFi.Insurance_File_Cnt = PF.Insurance_File_Cnt
WHERE   InFi.Insurance_Folder_Cnt = @Insurance_Folder_Cnt 
AND     InFi.Policy_version = @Previous_Policy_Version
AND     PF.StatusInd = '040'

GO

SET QUOTED_IDENTIFIER  OFF    
SET ANSI_NULLS  ON
GO