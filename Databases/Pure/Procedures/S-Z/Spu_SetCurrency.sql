GO
/****** Object:  StoredProcedure [dbo].[Spu_SetCurrency]    Script Date: 09/13/2012 11:38:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

Execute DDLDropProcedure 'Spu_SetCurrency'
GO

create procedure [dbo].[Spu_SetCurrency]
      @parent_value varchar(20)
AS 
BEGIN
declare @shortName as varchar(20) =''
IF (@parent_value LIKE '%^^%')  
begin
	set @shortName = SUBSTRING ( @parent_value , charindex('^^', @parent_value)+2,LEN(@parent_value) )
	set @parent_value =  SUBSTRING ( @parent_value , 0, charindex('^^', @parent_value))       
end 
if @parent_value = 'System'
begin
 select distinct(c.description) from Currency c inner join PMSystem s  on c.currency_id = s.currency_id
end
else if @parent_value = 'Account' and @shortName <>''
begin
	select distinct(c.description) 
		from Currency c   
			inner join  party p on p.currency_id = c.currency_id
				where shortname =@shortName
end
else if @parent_value = 'Account' or @parent_value = 'transaction' or  @parent_value = '<ALL>' or @parent_value = 'Base'
begin
		SELECT '<ALL>' AS Description
		UNION
		select distinct(c.description) from Currency c
end
END

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
