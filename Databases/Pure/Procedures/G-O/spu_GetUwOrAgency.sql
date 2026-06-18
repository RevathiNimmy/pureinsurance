SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetUwOrAgency'
GO


CREATE PROCEDURE spu_GetUwOrAgency
    @UwOrAgency char(1) = null output,
    @UwType char(1) = null output
AS

    Select
        @UwOrAgency = Isnull(Max(value), 'A'),
        @UwType = Isnull(Max(value), 'U')
    From
        hidden_options
    Where 
        branch_id = 1 
    And option_number = 1

GO


