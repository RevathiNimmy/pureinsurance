SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


Execute DDLDropProcedure 'spu_Get_User_Authority_Limit'
GO

CREATE PROCEDURE spu_Get_User_Authority_Limit
    @userid int

AS

SELECT     has_payments_authority, payments_amount, 
           has_claim_payments_authority, claim_payments_amount, payments_currency_id, claims_payments_currency_id,is_recommender, recommender_currency_amount     
FROM         User_Authorities
WHERE     (user_id = @userid)

GO
