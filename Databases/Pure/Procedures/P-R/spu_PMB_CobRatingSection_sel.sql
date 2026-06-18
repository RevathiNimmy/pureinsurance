SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_PMB_CobRatingSection_sel'
GO

CREATE PROCEDURE spu_PMB_CobRatingSection_sel
    @cob_rating_section_id int

AS

BEGIN

	select 
		code,
		Description,
		is_deleted,
		effective_date,
		is_levy_section
	from 
		cob_rating_section 
	where 
		cob_rating_section_id=@cob_rating_section_id

END
go