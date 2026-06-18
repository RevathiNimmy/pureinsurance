SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_Policy_Intermediary_Agent_Account'
GO


CREATE PROCEDURE spu_Get_Policy_Intermediary_Agent_Account
    @InsuranceFolderCnt int,
	@InsuranceFileCnt INT
AS  

IF EXISTS(SELECT NULL FROM Party_Agent pa INNER JOIN Insurance_file i ON i.lead_agent_cnt=pa.party_cnt 
            WHERE insurance_file_cnt=@InsuranceFileCnt AND party_agent_type_id=5)
SELECT TOP 1 intermediary_agent_account_id  
FROM insurance_file  
WHERE insurance_folder_cnt=@InsuranceFolderCnt AND insurance_file_type_id=2  
ORDER BY insurance_file_cnt  

GO


