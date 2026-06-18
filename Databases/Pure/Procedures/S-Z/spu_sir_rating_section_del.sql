SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sir_rating_section_del'
GO


CREATE PROCEDURE spu_sir_rating_section_del
    @insurance_file_cnt Int,
    @risk_id int
AS


Begin

    --Delete the rating sections

    Delete rating_section
    Where Risk_cnt = @risk_id

end
GO


