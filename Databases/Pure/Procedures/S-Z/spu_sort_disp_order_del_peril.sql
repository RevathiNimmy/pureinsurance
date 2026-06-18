SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sort_disp_order_del_peril'
GO


CREATE PROCEDURE spu_sort_disp_order_del_peril
    @Data_id int
AS


begin

Declare @Chkorder int
Declare @PerilTypeid int
select @Periltypeid =Peril_type_id from Peril_data_definition where Peril_data_defn_id=@data_id

select @chkorder =display_order from Peril_data_definition where Peril_data_defn_id=@data_id

update Peril_data_definition

set display_order =display_order-1

where display_order>@chkorder and type<>6 and Peril_type_id =@periltypeid

end
GO


