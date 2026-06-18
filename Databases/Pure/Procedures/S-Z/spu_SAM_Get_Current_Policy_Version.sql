SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDROPPROCEDURE 'spu_SAM_Get_Current_Policy_Version'
GO

CREATE PROCEDURE spu_SAM_Get_Current_Policy_Version
    @InsuranceFolderCnt int,
    @InsuranceFileCnt int
AS


IF @InsuranceFolderCnt = 0
BEGIN
   SELECT @InsuranceFolderCnt = insurance_folder_cnt
   FROM   insurance_File
   WHERE  insurance_file_cnt = @InsuranceFileCnt
END

SELECT  MAX(ifi.insurance_file_cnt)
FROM    insurance_file ifi
WHERE   ifi.insurance_folder_cnt    = @InsuranceFolderCnt
AND   ifi.policy_ignore           IS NULL

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
