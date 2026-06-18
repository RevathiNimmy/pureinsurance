SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Period_Id_From_Ledger'
GO

CREATE PROCEDURE spu_ACT_Get_Period_Id_From_ledger 
@nAccountId INT,
@nSourceId INT 

AS

DECLARE @bSystemOption INT

SELECT @bSystemOption = value FROM system_options WHERE option_number = 5038 AND branch_id = @nSourceId -- 5038=Posting by Policy Effective and Transaction Date
IF @bSystemOption = 0
SELECT current_period_id FROM ledger l 
						INNER JOIN account a ON a.ledger_id=l.ledger_id
						WHERE account_id = @nAccountId




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
