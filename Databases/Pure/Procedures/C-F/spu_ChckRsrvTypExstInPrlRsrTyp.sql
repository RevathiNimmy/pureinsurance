SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ChckRsrvTypExstInPrlRsrTyp'
GO


CREATE PROCEDURE spu_ChckRsrvTypExstInPrlRsrTyp
    @ReserveType integer,
    @Periltype integer
AS


declare @RecdExists boolean
    declare @RecdCount integer

    select @RecdCount = count(*)
    from peril_type_reserve_type
    where Peril_type_Id = @PerilType
    and Reserve_Type_Id = @ReserveType

    if @RecdCount > 0
    BEGIN
        SELECT @RecdExists =1
    END
    else
    BEGIN
        SELECT @RecdExists = 0
    END
    SELECT @RecdExists
GO


