SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_periltype_rsrvtype_upd'
GO


CREATE PROCEDURE spu_periltype_rsrvtype_upd
    @PerilTypeReserveTypeID integer,
    @ReserveType integer,
    @PerilTypeID integer,
    @MainReserve bit
AS


update peril_type_reserve_type
    set     Reserve_type_id = @ReserveType,
            peril_type_id = @PerilTypeID,
            is_main_reserve = @MainReserve
    where Peril_type_Reserve_type_id = @PerilTypeReserveTypeID
GO


