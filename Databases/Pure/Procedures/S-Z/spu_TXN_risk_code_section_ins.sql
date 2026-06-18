SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_risk_code_section_ins'
GO

CREATE PROCEDURE spu_TXN_risk_code_section_ins(
	@risk_code_id int, 
	@COB_rating_section_id int,
	@sequence int)

AS

INSERT INTO risk_tax_usage(
	risk_code_id,
	COB_rating_section_id,
	sequence)
VALUES
	(@risk_code_id, 
	@COB_rating_section_id,
	@sequence)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
