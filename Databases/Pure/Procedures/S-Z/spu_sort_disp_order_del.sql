SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_sort_disp_order_del'
GO


CREATE PROCEDURE spu_sort_disp_order_del
    @Data_id int
AS


begin

Declare @Chkorder int
Declare @RiskTypeid int

select @Risktypeid =Risk_type_id from risk_data_definition where risk_data_defn_id=@data_id

select @chkorder =display_order from risk_data_definition where risk_data_defn_id=@data_id

update risk_data_definition

set display_order =display_order-1

where display_order>@chkorder and type<>6 and Risk_type_id=@RiskTypeid

end
GO


