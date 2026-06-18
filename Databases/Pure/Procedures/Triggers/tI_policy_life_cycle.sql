SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO

execute DDLDropTrigger 'tI_policy_life_cycle'
go
create trigger tI_policy_life_cycle on policy_life_cycle for INSERT as
-- INSERT trigger on policy_life_cycle
begin
  declare  @numrows int,
           @nullcnt int,
           @validcnt int,
           @errno   int,
           @errmsg  varchar(255)
  select @numrows = @@rowcount
  -- GIS_Scheme R/9 policy_life_cycle ON CHILD INSERT RESTRICT
  if
    -- %ChildFK(" or",update)
    update(gis_scheme_id)
  begin
    select @nullcnt = 0
    select @validcnt = count(*)
      from inserted,GIS_Scheme
        where
          -- %JoinFKPK(inserted,GIS_Scheme)
          inserted.gis_scheme_id = GIS_Scheme.gis_scheme_id
    -- %NotnullFK(inserted," is null","select @nullcnt = count(*) from inserted where"," and")

    if @validcnt + @nullcnt != @numrows
    begin
      select @errno  = 30002,
             @errmsg = 'Cannot INSERT policy_life_cycle because GIS_Scheme does not exist.'
      goto error
    end
  end
  return
error:
    raiserror (@errmsg, 16, 1)
    rollback transaction
end

GO

