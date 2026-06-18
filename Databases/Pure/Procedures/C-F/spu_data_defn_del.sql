SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_data_defn_del'
GO


CREATE PROCEDURE spu_data_defn_del
    @data_defn_id int,
    @Mode bit
AS


BEGIN
If @Mode=0
    Begin
    exec spu_sort_disp_order_del @data_defn_id
    DELETE FROM Risk_data_definition WHERE risk_data_defn_id=@data_defn_id

    End

Else if @Mode=1

    Begin

    exec spu_sort_disp_order_del_peril @data_defn_id
    DELETE FROM Peril_data_definition WHERE peril_data_defn_id=@data_defn_id

    End
END
GO


