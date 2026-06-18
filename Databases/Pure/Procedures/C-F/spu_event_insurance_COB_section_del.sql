SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_event_insurance_COB_section_del'
GO
 
CREATE PROCEDURE spu_event_insurance_COB_section_del
	@Insurance_section_id integer 
AS
 
DELETE FROM event_insurance_COB_section
WHERE Insurance_section_id = @Insurance_section_id
GO
 
