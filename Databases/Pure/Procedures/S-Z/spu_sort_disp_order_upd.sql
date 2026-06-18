SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sort_disp_order_upd'
GO


CREATE PROCEDURE spu_sort_disp_order_upd
    @Param int,
    @Data_id int
AS


Declare @Chkorder int
Declare @RiskTypeid int

select @chkorder =display_order from risk_data_definition where risk_data_defn_id=@data_id

select @RiskTypeid=Risk_type_id from risk_data_definition where risk_data_defn_id=@data_id

if  (@Param>@chkorder)

begin

update risk_data_definition

set display_order =display_order-1

where display_order>@chkorder and display_order<=@param and type<>6 and Risk_type_id=@RiskTypeid

end

if (@Param<@chkorder)

begin

update risk_data_definition

set display_order =display_order+1

where display_order<@chkorder and display_order>=@param  and type<>6 and Risk_type_id=@RiskTypeid

end
GO


