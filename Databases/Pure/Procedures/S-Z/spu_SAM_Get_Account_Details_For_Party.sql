SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Account_Details_For_Party
GO

CREATE  PROCEDURE spu_SAM_Get_Account_Details_For_Party

@party_cnt int

AS
    SELECT account_id,
           account_name,
           short_code,
           --Start (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.2.4)
           Party.is_deleted
    FROM   Account
        INNER JOIN Party ON
                   Party.Party_cnt=Account.Account_key
        --End (Prakash C Varghese) - (Tech Spec - UIIC WR51 - Claim Payment - Payment Workflow.doc) - (6.2.4)
    WHERE  Account.Account_key = @party_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
