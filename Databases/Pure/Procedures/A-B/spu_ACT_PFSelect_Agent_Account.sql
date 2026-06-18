SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ACT_PFSelect_Agent_Account'
GO

-- Get the account that commission should be paid into

CREATE PROCEDURE spu_ACT_PFSelect_Agent_Account

    @lTransDetailId INT

AS

SELECT
    A.account_id
FROM
    Account A
JOIN
    Party_Agent PA ON (PA.party_cnt = A.account_key)
JOIN
    Insurance_file INF ON (INF.lead_agent_cnt = PA.party_cnt)
JOIN
    PFPremiumFinance PF ON (PF.Insurance_File_Cnt = INF.Insurance_File_Cnt)
WHERE
    PF.commission_transdetail_id = @lTransDetailId

GO


