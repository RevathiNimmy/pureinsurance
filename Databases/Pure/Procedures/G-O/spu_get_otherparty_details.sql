SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_otherparty_details'
GO

CREATE PROCEDURE spu_get_otherparty_details

AS

BEGIN

 select  p.party_cnt,
  p.shortname,
  p.resolved_name,
  ph.forename,
  ph.initials,
  ph.department_id,
  ph.party_title_code,
  p.currency_id,
  p.name,
  ph.commission_cnt
 from  party p
 join  party_type pt
 on  p.party_type_id = pt.party_type_id
 and pt.code LIKE 'OT%'
 left outer join  party_handler ph
 on ph.party_cnt = p.party_cnt

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
