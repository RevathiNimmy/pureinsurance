SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_risk_code_section_del'
GO

CREATE PROCEDURE spu_TXN_risk_code_section_del(@risk_code_id int)

AS
DELETE FROM risk_tax_usage WHERE risk_code_id = @risk_code_id


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

