SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_insurance_COB_section_del'
GO
 
CREATE PROCEDURE spu_insurance_COB_section_del
	@Insurance_section_id integer 
AS

DELETE FROM tax_calculation
WHERE Insurance_section_id = @Insurance_section_id

DELETE FROM policy_coinsurers
WHERE Insurance_section_id = @Insurance_section_id
 
DELETE FROM insurance_COB_section
WHERE Insurance_section_id = @Insurance_section_id
GO
 
