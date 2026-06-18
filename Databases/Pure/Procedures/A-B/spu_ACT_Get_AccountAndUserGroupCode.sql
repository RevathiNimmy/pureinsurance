SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Get_AccountAndUserGroupCode'
GO


CREATE PROCEDURE spu_ACT_Get_AccountAndUserGroupCode
@accountcode varchar(30) OUTPUT,
@usergroupcode varchar(30) OUTPUT,
@accountid int,
@usergroupid int
   
AS


select @accountcode = short_code from account where account_id = @accountid
select @usergroupcode = code from pmuser_group where pmuser_group_id = @usergroupid
               
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
