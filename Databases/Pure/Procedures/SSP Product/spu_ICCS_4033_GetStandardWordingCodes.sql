DDLDROPPROCEDURE 'spu_ICCS_4033_GetStandardWordingCodes'
GO            
CREATE PROCEDURE spu_ICCS_4033_GetStandardWordingCodes            
        @lPartyCnt INT,            
        @lPolicyLinkId INT,            
        @lPolicyBinderId INT,            
        @lInsuranceFileCnt INT,            
        @lInsuranceFolderCnt INT            
            
AS            
    
SELECT dt.code FROM policy_standard_wording psw INNER JOIN  Document_Template dt ON psw.Document_Template_id = dt.Document_Template_id   
WHERE psw.insurance_file_cnt = @lInsuranceFileCnt
GO