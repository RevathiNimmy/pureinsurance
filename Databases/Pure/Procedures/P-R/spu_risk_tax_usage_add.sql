SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_risk_tax_usage_add'
GO

CREATE PROCEDURE spu_risk_tax_usage_add
    	@Risk_Code_id int,
    	@COB_Rating_Section_Id int,
	@Tax_Group_Id int 
AS

/* Update the values */
INSERT INTO  risk_tax_usage
(risk_code_id,COB_Rating_Section_id,Tax_Group_Id)
VALUES (@Risk_Code_id,@COB_Rating_Section_Id,@Tax_Group_Id)
GO
 
