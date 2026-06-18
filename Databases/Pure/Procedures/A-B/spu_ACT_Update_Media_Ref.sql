SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_ACT_Update_Media_Ref'
GO

CREATE PROCEDURE spu_ACT_Update_Media_Ref
    @outputparam int OUTPUT,
    @cashlistitemid int,
    @pmuserid int, 
    @oldmediaref varchar(30),
    @newmediaref varchar(30),
    @ourref varchar(255),
    @theirref varchar(30)
AS

begin

begin tran

insert into cashlistitem_audit
(pmuser_id,
cashlistitem_id,
old_data,
modified_date)
values
(@pmuserid,
@cashlistitemid,
'Media Reference:' + @oldmediaref,
getdate())

if @@error != 0
begin
rollback tran
set @outputparam = 10
select -1
end

update cashlistitem
set
media_ref = @newmediaref
where
cashlistitem_id = @cashlistitemid

if @@error != 0
begin
rollback tran
set @outputparam = 10
return -1
end

update transdetail
set
comment = @ourref + '/' + @theirref + '/' + @newmediaref,
spare = @newmediaref
where
transdetail_id = (select transdetail_id 
		from cashlistitem 
		where cashlistitem_id = @cashlistitemid)
if @@error != 0
begin
rollback tran
set @outputparam = 10
return -1
end

set @outputparam = 1
commit tran

end
GO
