
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_Reserve_For_Claim_Payment'
GO

CREATE PROCEDURE spu_Get_Reserve_For_Claim_Payment
    @Claim_payment_id INT
AS

    SELECT Sum(Initial_reserve)+Sum(Revised_reserve) 
    FROM Reserve R
    JOIN Claim_payment CP 
    ON R.Claim_peril_id = CP.Claim_peril_id 
    WHERE CP.Claim_payment_id= @Claim_payment_id