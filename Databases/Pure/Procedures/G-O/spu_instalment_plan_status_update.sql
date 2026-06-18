EXECUTE DDLDropProcedure 'spu_instalment_plan_status_update' 
GO
CREATE PROCEDURE spu_instalment_plan_status_update
@transdetail_id VARCHAR(25)
AS
BEGIN
DECLARE @PFStatusIndLive VARCHAR(10) = '040'
DECLARE @PFStatusIndCompleted VARCHAR(10) = '900'

UPDATE pfp SET statusind = (CASE WHEN td.outstanding_amount <> 0 THEN @PFStatusIndLive WHEN td.outstanding_amount = 0 THEN @PFStatusIndCompleted END)
FROM pfpremiumfinance pfp INNER JOIN transdetail td ON td.transdetail_id = pfp.PlanTransaction_id 
WHERE td.transdetail_id = @transdetail_id AND pfp.statusind IN (@PFStatusIndLive,@PFStatusIndCompleted)

END