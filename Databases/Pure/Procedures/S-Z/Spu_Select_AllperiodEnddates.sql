SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Select_AllperiodEnddates'
GO

CREATE procedure Spu_Select_AllperiodEnddates  
As  
Select period_id,convert(varchar,period_end_date,106) from Period   

GO