SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_periltype_rsrvtype_add'
GO


CREATE PROCEDURE spu_periltype_rsrvtype_add
    @PerilTypeReserveTypeID integer OUTPUT,
    @ReserveType integer,
    @PerilTypeID integer,
    @MainReserve bit
AS


insert into peril_type_reserve_type
        (Reserve_type_id, peril_type_id, is_main_reserve)
        values (@ReserveType, @PerilTypeID,@MainReserve)

    select @PerilTypeReserveTypeID = @@Identity
GO


