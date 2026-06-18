SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_clm_get_policy_account_handlers'
GO

CREATE PROCEDURE
spu_clm_get_policy_account_handlers
(
@insurance_file_cnt int
)
AS

SELECT
account_handler_cnt,
P1.resolved_name AS account_handler,
P2.consultant_cnt as account_executive_cnt,
(SELECT resolved_name  FROM Party WHERE party_cnt= P2.consultant_cnt) 
AS account_executive
FROM
insurance_file I
LEFT OUTER JOIN party P1 ON I.account_handler_cnt=P1.party_cnt
LEFT OUTER JOIN party P2 ON I.insured_cnt=P2.party_cnt
WHERE I.insurance_file_cnt=@insurance_file_cnt
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

