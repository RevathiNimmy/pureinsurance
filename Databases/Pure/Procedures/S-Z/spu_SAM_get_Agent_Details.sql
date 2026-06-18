SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_get_Agent_Details'
GO
--Start (Vijayakumar Ramasamy) - Tech Spec - UIICWR23 - New Business - Pre Payment Functionality -(6)
CREATE PROCEDURE spu_SAM_get_Agent_Details 
@insurance_file_cnt int
As

SELECT pat.Code , is_float_balance_account,is_overdraft_account,
       float_balance_limit , overDraft_limit ,pa.Overdraft_expiry 
FROM   Insurance_file ifl LEFT JOIN party_agent pa ON 
       ifl.lead_agent_cnt=pa.party_cnt LEFT JOIN 
       Party_agent_type pat ON pa.party_agent_type_id=pat.party_agent_type_id 
WHERE insurance_file_cnt = @insurance_file_cnt


--End (Vijayakumar Ramasamy) - Tech Spec - UIICWR23 - New Business - Pre Payment Functionality -(6)

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


