SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Check_Main_Reserve'
GO


CREATE PROCEDURE spu_Check_Main_Reserve
    @PerilTypeId integer
AS


select count(*)
    from Peril_Type_Reserve_Type
    where Peril_Type_Id = @PerilTypeId
    and is_Main_Reserve = 1
GO


