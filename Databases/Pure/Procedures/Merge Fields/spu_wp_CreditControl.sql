SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_wp_creditcontrol'
GO


CREATE PROCEDURE spu_wp_creditcontrol
    @PartyCnt INT,
    @InsuranceFileCnt INT,
    @RiskId INT,
    @ClaimCnt INT,
    @DocumentRef VARCHAR(25),
    @Instance1 INT,
    @Instance2 INT,
    @Instance3 INT
AS

DECLARE @underwriting_broking CHAR(1)

SELECT @underwriting_broking = value 
FROM hidden_options  
WHERE branch_id = 1 
AND option_number = 1  

IF @underwriting_broking = 'A'
BEGIN
    SELECT  ISNULL(SUM(CCI.amount),0) 'CreditControlBalance'
    FROM    credit_control_item CCI
    JOIN    account ACT
    ON      ACT.account_id=CCI.account_id
    JOIN credit_control_step CCS
    ON CCS.credit_control_step_id = CCI.credit_control_step_id
    WHERE   (ISNULL(CCI.is_deleted,0)=0)  
    AND     ACT.account_key = @PartyCnt
    AND CCI.amount > CCS.policy_tolerance_amount

END

ELSE

BEGIN
    SELECT  ISNULL(SUM(CCI.amount),0) 'CreditControlBalance'
    FROM    credit_control_item CCI
    JOIN    account ACT
    ON      ACT.account_id=CCI.account_id
    WHERE   (ISNULL(CCI.is_deleted,0)=0)
    AND     ACT.account_key = @PartyCnt
END

GO
