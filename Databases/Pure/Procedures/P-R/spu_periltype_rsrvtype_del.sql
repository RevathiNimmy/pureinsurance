SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_periltype_rsrvtype_del'
GO


CREATE PROCEDURE spu_periltype_rsrvtype_del
    @PerilTypeReserveTypeID integer
AS


delete from Peril_Type_Reserve_Type
    where Peril_Type_Reserve_Type_id = @PerilTypeReserveTypeID
GO


