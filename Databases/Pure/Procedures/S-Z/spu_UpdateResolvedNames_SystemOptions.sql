SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_UpdateResolvedNames_SystemOptions'
GO
--Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (6.2.1)
create procedure spu_UpdateResolvedNames_SystemOptions 
@Update_Client varchar(2) = '0' 
as 
Begin
UPDATE 
	Party
SET 
	resolved_name = ltrim(rtrim(pc.party_title_code)) + ' ' + ltrim(rtrim(pc.forename)) + ' ' + ltrim(rtrim(p.name))
FROM
	Party p
INNER JOIN
	Party_personal_client pc
ON
	p.party_cnt = pc.party_cnt

--Reset the system option so the script is not run again
UPDATE 
	System_Options
SET
	value = @Update_Client
WHERE
	option_number  = 5064 --[NEW OPTION NUMBER FOR UPDATE CHECKBOX]

End
--End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (6.2.1)
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


