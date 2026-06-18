SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_witholding_tax_sel'
GO

CREATE  PROCEDURE spu_TXN_witholding_tax_sel
(@tax_code varchar(10))
AS

SELECT is_withholding_tax FROM Tax_Group WHERE code=@tax_code


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

