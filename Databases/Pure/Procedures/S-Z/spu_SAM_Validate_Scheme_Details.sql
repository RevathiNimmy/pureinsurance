SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Validate_Scheme_Details'
GO

CREATE PROCEDURE spu_SAM_Validate_Scheme_Details

@schemeno int, 
@schemeversion int, 
@pfrf_id int

AS

select count(*) numberOfSchemes from pfrf
where schemeno  = @schemeno
and schemeversion = @schemeversion
and pfrf_id = @pfrf_id



GO
