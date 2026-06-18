SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ICCS_4033_PolicyHeader'
GO


CREATE PROCEDURE spu_ICCS_4033_PolicyHeader          
        @lPartyCnt INT,          
        @lPolicyLinkId INT,          
        @lPolicyBinderId INT,          
        @lInsuranceFileCnt INT,          
        @lInsuranceFolderCnt INT          
          
AS          
  
SELECT insurance_ref 'PolicyNumber',  
inception_date 'InceptionDate',  
expiry_date 'ExpiryDate',  
renewal_date 'RenewalDate',
(SELECT PS.description FROM Policy_Status PS WHERE PS.Policy_Status_id  = INF.Policy_Status_id) 'Policy_Status',  
 Pd.description 'Product'--, 
 --MID changes revert back
-- (Select expiry_date from Insurance_file where insurance_file_cnt= (Select top 1 (insurance_file_cnt) from insurance_file IFL where IFL.insurance_folder_cnt=@lInsuranceFolderCnt and IFL.insurance_file_cnt < @lInsuranceFileCnt order by 1 desc )) 'PrevExpiryDate'
FROM         Insurance_File  INF     
JOIN Product PD  
ON Pd.Product_id =INF.Product_id      
WHERE     (INF.insurance_file_cnt = @lInsuranceFileCnt)
GO


