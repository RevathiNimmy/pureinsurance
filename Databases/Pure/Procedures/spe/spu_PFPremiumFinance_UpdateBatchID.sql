EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_UpdateBatchID'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFPremiumFinance_UpdateBatchID

@pfprem_finance_cnt int,
@pfprem_finance_version int,
@batch_id int

AS BEGIN

    UPDATE PFPremiumFinance 
    SET batch_id = @batch_id
    WHERE pfprem_finance_cnt = @pfprem_finance_cnt
    AND pfprem_finance_version = @pfprem_finance_version

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
