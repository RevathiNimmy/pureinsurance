SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sort_disp_order_upd_peril'
GO


CREATE PROCEDURE spu_sort_disp_order_upd_peril
    @Param int,
    @Data_id int
AS


Declare @Chkorder int
Declare @PerilTypeid int

select @chkorder =display_order from Peril_data_definition where Peril_data_defn_id=@data_id

select @PerilTypeid =Peril_Type_id from Peril_data_definition where Peril_data_defn_id=@data_id

if  (@Param>@chkorder)

begin

update Peril_data_definition

set display_order =display_order-1

where display_order>@chkorder and display_order<=@param and type<>6 and Peril_Type_id=@PerilTypeid

end

if (@Param<@chkorder)

begin

update Peril_data_definition

set display_order =display_order+1

where display_order<@chkorder and display_order>=@param  and type<>6 and Peril_Type_id=@PerilTypeid

end
GO


