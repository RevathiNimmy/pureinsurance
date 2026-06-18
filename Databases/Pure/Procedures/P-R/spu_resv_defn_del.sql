SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_resv_defn_del'
GO


CREATE PROCEDURE spu_resv_defn_del
    @ReserveTypeID int
AS


BEGIN
DELETE FROM Reserve_type WHERE Reserve_type_ID=@ReserveTypeID
END
GO


