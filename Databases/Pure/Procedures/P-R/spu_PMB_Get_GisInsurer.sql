if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[spu_PMB_Get_GISInsurer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[spu_PMB_Get_GISInsurer]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spu_PMB_Get_GISInsurer 
		@gis_scheme_id int,
		@source_id int
AS
BEGIN

if exists(select null from gis_scheme s
		join gis_insurer g on g.gis_insurer_id = s.gis_insurer_id
		join party p on p.party_cnt = g.party_cnt
		where s.gis_scheme_id = @gis_scheme_id and p.source_id=@source_id)
begin
	select 
		p.party_cnt, 
		p.name 
	from 
		gis_scheme s
		join gis_insurer g on g.gis_insurer_id = s.gis_insurer_id
		join party p on p.party_cnt = g.party_cnt
	where 
		s.gis_scheme_id = @gis_scheme_id and 
		p.source_id=@source_id
end
else
begin
	select 
		p.party_cnt, 
		p.name 
	from 
		gis_scheme s
		join gis_insurer g on g.gis_insurer_id = s.gis_insurer_id
		join party p on p.abi_code_on_81 = g.abi_81_insurer
	where 
		s.gis_scheme_id = @gis_scheme_id and 
		p.source_id=@source_id
end

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

