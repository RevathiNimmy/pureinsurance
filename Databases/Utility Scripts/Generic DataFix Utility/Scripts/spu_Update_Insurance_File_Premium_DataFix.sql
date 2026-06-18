SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_Update_Insurance_File_Premium_DataFix'

GO

CREATE proc spu_Update_Insurance_File_Premium_DataFix
@insurance_file_cnt INT
AS
BEGIN

DECLARE @PremiumTotal NUMERIC(19,4)
SELECT @PremiumTotal=SUM(total_this_premium)  from insurance_file_risk_link ifrl INNER JOIN risk r ON r.risk_cnt =ifrl.risk_cnt WHERE insurance_file_cnt =@insurance_file_cnt
UPDATE insurance_file SET this_premium=@PremiumTotal,net_premium=@PremiumTotal WHERE insurance_file_cnt = @insurance_file_cnt

UPDATE insurance_file_risk_link set is_risk_edited=1 where insurance_file_cnt = @insurance_file_cnt and original_risk_cnt IS NOT NULL AND status_flag IN ('C','D')

END