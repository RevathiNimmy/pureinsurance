SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Check_CC_Manual_Auth_Code'
GO


CREATE PROCEDURE spu_ACT_Check_CC_Manual_Auth_Code
          @nAccountId INT,

          @sManualAuthCode VARCHAR(255)=NULL,

          @bIsEntryExits tinyINT = 0 OUTPUT



AS



SELECT  @bIsEntryExits=count(PFPremiumFinance.auth_code)

FROM    Party_Bank INNER JOIN

        PFPremiumFinance ON Party_Bank.party_bank_id = PFPremiumFinance.party_bank_id

WHERE   Party_Bank.account_id=@nAccountId  

AND     PFPremiumFinance.auth_code=@sManualAuthCode








