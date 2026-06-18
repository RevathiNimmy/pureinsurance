SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sir_peril_del'
GO


CREATE PROCEDURE spu_sir_peril_del
    @insurance_file_cnt int,
    @risk_id int,
    @rating_section_id int = null
AS


Begin

    if @rating_section_id is null

        Delete Peril
        Where risk_cnt  = @risk_id
    Else
        Delete Peril
        Where risk_cnt  = @risk_id
        and   rating_section_id = @rating_section_id
End
GO


