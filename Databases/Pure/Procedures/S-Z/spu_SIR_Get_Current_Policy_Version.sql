SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Current_Policy_Version'
GO

--
-- Find the current verion of the policy that has not expired or is in the future.
-- Accepts insurance_folder_cnt but if not available pass insurance_file_cnt and we'll find it from there.
-- Works for Broking and Underwriting. 
-- For both, we ignore versions that have 'policy_ignore' set.
--

CREATE PROCEDURE spu_SIR_Get_Current_Policy_Version
    @InsuranceFolderCnt int,
    @InsuranceFileCnt int
AS

-- See if underwriting or broking system for use later
DECLARE @underwriting_flag varchar(20)

SELECT  @underwriting_flag = value
FROM    hidden_options 
WHERE   branch_id = 1 
AND     option_number = 1

-- If we do not have it then get the insurance_folder_cnt via the insurance_file_cnt 
IF @InsuranceFolderCnt = 0
BEGIN
   SELECT @InsuranceFolderCnt = insurance_folder_cnt 
   FROM   insurance_File
   WHERE  insurance_file_cnt = @InsuranceFileCnt
END

-- Find the current verion of the policy that has not expired or is in the future
SELECT  MAX(ifi.insurance_file_cnt)
FROM    insurance_file ifi
WHERE   ifi.insurance_folder_cnt    = @InsuranceFolderCnt
AND     ifi.cover_start_date        <= getdate()  -- Ignore future dated policy versions 
AND     ifi.expiry_date             >= getdate()  -- Ignore old versions
AND 	ifi.policy_ignore           IS NULL

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO