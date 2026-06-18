SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_valid_user_groups'
GO

create procedure spu_get_valid_user_groups
as
begin
	declare @now datetime
	select @now  = Getdate()

	select 	UG.pmuser_group_id,
		UG.code,
		UG.description,
		CP.caption
	from	pmuser_group UG left outer join
		pmcaption CP on UG.caption_id = CP.caption_id
	where	datediff(s,UG.effective_date,@now)>0
	and	UG.is_Deleted = 0
	order by UG.code

end


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

