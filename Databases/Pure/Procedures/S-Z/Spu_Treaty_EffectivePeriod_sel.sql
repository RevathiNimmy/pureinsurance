SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'Spu_Treaty_EffectivePeriod_sel'
GO

Create Procedure Spu_Treaty_EffectivePeriod_sel
@treaty_id int
As

Select effective_date,expiry_date from treaty where treaty_id=@treaty_id