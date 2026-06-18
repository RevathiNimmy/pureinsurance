SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_pmuser_group_info'
GO

CREATE PROCEDURE spu_get_pmuser_group_info      
    @user_id integer,      
    @effective_date datetime,    
    @search_type tinyint = 0    
    
      
AS      
      
BEGIN      
 select pg.pmuser_group_id, pg.code, pg.description, isnull(pgu.user_id, 0), isnull(pgu.is_supervisor, 0), is_sys_admin_group      
 from pmuser_group pg      
 left outer join pmuser_group_user pgu      
 on pgu.pmuser_group_id = pg.pmuser_group_id      
 and pgu.user_id = @user_id      
 where pg.effective_date <= @effective_date      
 and pg.is_deleted = 0     
 and isnull(pgu.user_id, 0) >  Case when @search_type =  1 then  0 else  -1  End    
     
      
END
