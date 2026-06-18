SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_nexus_scheme_extra'
GO


CREATE PROCEDURE spu_SAM_nexus_scheme_extra
    @gis_scheme_id integer
AS
	select 
		MaximumPeriodTempMta,
		MaximumPeriodPerPolicyPeriod,
		MaximumNoOfTempMta
	from 
		gis_scheme 
	where 
		gis_scheme_id=@gis_scheme_id
go
