SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetRsrvTypForPrlTyp'
GO


CREATE PROCEDURE spu_GetRsrvTypForPrlTyp
    @PerilType integer
AS


select  peril_type_reserve_type_id , reserve_type.name,
            reserve_type.description ,reserve_type.Include_in_Total,
            peril_type_reserve_type.is_main_reserve
    from    peril_type_reserve_type, reserve_type
    where   peril_type_reserve_type.reserve_type_id = reserve_type.reserve_type_id
    and     Peril_Type_Id = @PerilType
GO


