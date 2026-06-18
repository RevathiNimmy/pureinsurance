SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_installment_Plan_Status'
GO
CREATE PROCEDURE spu_Get_installment_Plan_Status 
@sInstalment_Plan_Ref VARCHAR(20)
AS
SELECT pfp.statusind,A.short_code,C.code  FROM PFPremiumFinance pfp 
INNER JOIN TRANSDETAIL TD ON td.insurance_ref =@sInstalment_Plan_Ref
INNER JOIN ACCOUNT A ON TD.account_id =A.account_id 
INNER JOIN PARTY P ON P.party_cnt =A.account_key 
INNER JOIN CURRENCY C ON TD.currency_id =C.currency_id 
WHERE pfp.pfprem_finance_cnt =CONVERT(INT, @sInstalment_Plan_Ref) AND P.party_type_id  =18
GO 
