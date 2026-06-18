SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Arrangement_Line_upd'
GO


CREATE PROCEDURE spu_RI_Arrangement_Line_upd
    @ri_arrangement_line_id int,
    @this_share_percent float,
    @premium_percent float,
    @commission_percent float,
    @agreement_code varchar(255),
    @sum_insured money,
    @premium_value money,
    @commission_value money,
    @premium_tax money,
    @commission_tax money,
    @is_commission_modified tinyint
AS

    Update  ri_arrangement_line
    Set     this_share_percent = @this_share_percent * 100,
            premium_percent = @premium_percent * 100,
            commission_percent = @commission_percent * 100,
            agreement_code = @agreement_code,
            sum_insured = @sum_insured,
            premium_value = @premium_value,
            commission_value = @commission_value,
            premium_tax = @premium_tax,
            commission_tax = @commission_tax,
            is_commission_modified = @is_commission_modified
    Where   ri_arrangement_line_id = @ri_arrangement_line_id
    

Go



