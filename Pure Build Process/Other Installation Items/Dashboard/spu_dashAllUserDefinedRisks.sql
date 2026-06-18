DDLDropPROCEDURE spu_dashAllUserDefinedRisks
GO
CREATE  PROCEDURE [dbo].[spu_dashAllUserDefinedRisks]
AS 
DECLARE @sSQL VarChar(4000)
DECLARE @binder_table VarChar(4000)
DECLARE @columndesc VarChar(4000)
DECLARE @columnname VarChar(4000)
DECLARE @tablename VarChar(4000)
DECLARE @screendesc VarChar(4000)
DECLARE @Specials_Type_Reference VarChar(4000)
DECLARE @Specials_Type VarChar(40)
DECLARE @data_type VarChar(40)
DECLARE @description_sql_part varchar(1000)
DECLARE @data_model_desc varchar(400)
DECLARE @gis_data_model_id int
DECLARE @screen_id int
DECLARE @parent_id int
DECLARE @item_top int
DECLARE @item_left int
DECLARE @parent_object_id int
DECLARE @parent_tablename VarChar(4000)
DECLARE @parent_id_name VarChar(4000)
DECLARE @object_name_desc varchar(400)
DECLARE @ud_hier varchar(128)
DECLARE @ud_risks varchar(128)
DECLARE @db_name varchar(128)
DECLARE @gis_property_id int

set nocount on

SELECT top 1 @ud_risks='dashAllUserDefinedRisks_' + database_name  FROM pmproduct
SELECT top 1 @ud_hier='dashAllUserDefinedHier_' + database_name  FROM pmproduct
--SELECT top 1 @ud_risks='spu_dashAllUserDefinedRisks_' + substring(database_name, 7, len(database_name)-6)  FROM pmproduct
--SELECT top 1 @ud_hier='dashUserDefinedHier_' + substring(database_name, 7, len(database_name)-6)  FROM pmproduct
--PRINT 'User Define Hier : ' + @ud_hier
--PRINT 'User Define Risks : ' + @ud_risks

SET @db_name = ''
--Check for spu_dashAllUserDefinedRisks table existence in Target DB
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name='[SiriusMonitorDB]')
BEGIN
	SET @db_name = '[SiriusMonitorDB].dbo.'
	IF EXISTS (SELECT name FROM [SiriusMonitorDB].dbo.sysobjects   WHERE name = @ud_risks AND type = 'U')
	BEGIN 
		select @sSQL = 'drop table [SiriusMonitorDB].dbo.' + @ud_risks
		PRINT 'Monitor SQL : ' + @sSQL
		execute (@sSQL)
	END	
	
	IF EXISTS (SELECT name FROM [SiriusMonitorDB].dbo.sysobjects   WHERE name = @ud_hier AND type = 'U')
	BEGIN 
		select @sSQL = 'drop table [SiriusMonitorDB].dbo.' + @ud_hier
		PRINT 'Monitor SQL : ' + @sSQL
		execute (@sSQL)
	END

END 
ELSE
BEGIN
	IF EXISTS (SELECT name FROM sysobjects WHERE name = @ud_risks AND type = 'U')
	BEGIN 
		select @sSQL = 'drop table ' + @ud_risks
		--PRINT 'Sirius SQL : ' + @sSQL
		execute (@sSQL)
	END

	IF EXISTS (SELECT name FROM sysobjects WHERE name = @ud_hier AND type = 'U')
	BEGIN 
		select @sSQL = 'drop table ' + @ud_hier
		--PRINT 'Sirius SQL : ' + @sSQL
		execute (@sSQL)
	END
END
--PRINT @db_name

SET @ud_risks = @db_name + @ud_risks

PRINT @ud_risks
	
SET @sSQL = 'create table ' + @ud_risks +
'(
	insurance_file_cnt int,
 	policy_binder_id int,
	table_id int,
	screen_level int,
	parent_object_id int null,
    data_model_desc VarChar(400),
    screen_desc VarChar(400),
    field_desc VarChar(400),	
	risk_data VarChar(4000),
	parent_id int, 
	item_top int, 
	item_left int,
	object_name_desc VarChar(400),
	gis_property_id int 
)'
--PRINT 'execute ' + (@sSQL)
execute (@sSQL)

--Hierarchy Table Creation on Target DB
SET @ud_hier = @db_name + @ud_hier
--PRINT @ud_hier

SET @sSQL = 'create table ' + @ud_hier +
'(
    insurance_file_cnt int,
 	gis_policy_link_id int,
	gis_screen_id int,
	screen_level int
)'

--PRINT 'execute ' + (@sSQL)
execute (@sSQL)

--Level 1
SET @sSQL =  'insert into ' + @ud_hier +' select 
		rl.insurance_file_cnt, l.gis_policy_link_id, r.gis_screen_id, 1 as screen_level 
	from 
		(
			SELECT i2.insurance_folder_cnt, MAX(policy_version) as policy_version FROM 	insurance_file i2 GROUP BY  i2.insurance_folder_cnt
		) Policy,
		
		insurance_file i,
		insurance_file_risk_link rl, 		
		risk r, 
		gis_screen s, 
		gis_policy_link l				
where 
	Policy.insurance_folder_cnt = i.insurance_folder_cnt and 
	Policy.policy_version = i.policy_version and
	i.insurance_file_cnt = rl.insurance_file_cnt and
	rl.risk_cnt = r.risk_cnt and	
	r.gis_screen_id = s.gis_screen_id and 
	l.risk_id = r.risk_cnt  '

--Remove on 13th Oct 2011 to include all risks - Start
/*WHERE (i2.insurance_file_status_id IS NULL OR insurance_file_status_id = 1) AND (i2.insurance_file_type_id = 2 OR i2.insurance_file_type_id = 5 OR i2.insurance_file_type_id = 10) */
--Remove on 13th Oct 2011 to include all risks - End

--For S4i this has been comment it on 27th Jan 2011
/*	
select rl.insurance_file_cnt, l.gis_policy_link_id, r.gis_screen_id, 1 as screen_level from gis_policy_link l, risk r, gis_screen s, insurance_file_risk_link rl		
where 
	rl.status_flag=''C'' and
	rl.risk_cnt = r.risk_cnt and	
	r.gis_screen_id = s.gis_screen_id and 
	l.risk_id = r.risk_cnt 
*/	

execute (@sSQL)

--Level 2
SET @sSQL = 'insert into ' + @ud_hier + ' select l.insurance_file_cnt, l.gis_policy_link_id, s.gis_screen_id, screen_level+1  from ' + @ud_hier + ' l,gis_screen  s
where s.parent_id = l.gis_screen_id and screen_level = 1'
execute (@sSQL)

--Level 3
SET @sSQL = 'insert into ' + @ud_hier + ' select l.insurance_file_cnt, l.gis_policy_link_id, s.gis_screen_id, screen_level+1  from ' + @ud_hier + ' l,gis_screen  s
where s.parent_id = l.gis_screen_id and screen_level = 2'
execute (@sSQL)

--Level 4
SET @sSQL = 'insert into ' + @ud_hier + ' select l.insurance_file_cnt, l.gis_policy_link_id, s.gis_screen_id, screen_level+1  from ' + @ud_hier + ' l,gis_screen  s
where s.parent_id = l.gis_screen_id and screen_level = 3'
execute (@sSQL)

--Level 5
SET @sSQL = 'insert into ' + @ud_hier + ' select l.insurance_file_cnt,  l.gis_policy_link_id, s.gis_screen_id, screen_level+1  from ' + @ud_hier + ' l,gis_screen  s
where s.parent_id = l.gis_screen_id and screen_level = 4'
execute (@sSQL)
--PRINT '@sSQL : ' + @sSQL

--Get All Data Model id=6 for House Hold
DECLARE data_model_cursor CURSOR FOR select  
distinct dm.gis_data_model_id
from gis_screen_detail d, gis_screen s,
 gis_object o, gis_property p, gis_data_model dm 
where s.gis_data_model_id = o.gis_data_model_id
and s.gis_screen_id = d.gis_screen_id
and o.gis_object_id = p.gis_object_id
and p.gis_property_id = d.gis_property_id
and d.gis_object_id = p.gis_object_id 
and dm.gis_data_model_id = s.gis_data_model_id
and dm.gis_data_model_type_id<>2

OPEN data_model_cursor
FETCH NEXT FROM data_model_cursor INTO @gis_data_model_id
WHILE @@FETCH_STATUS = 0
BEGIN 	
	SET @binder_table = (select table_name from gis_object where parent_object_id is null and table_name like '%_policy_binder' and gis_data_model_id =  @gis_data_model_id)
	PRINT 'BINDER TABLE : ' + @binder_table
	DECLARE field_cursor CURSOR FOR select 	
	CASE 
		WHEN d.caption='[BLANK]' THEN column_name 
		WHEN len(ltrim(d.caption)) = 0 THEN column_name 
		ELSE 
			REPLACE(REPLACE(REPLACE(REPLACE(d.caption, CHAR(10) + CHAR(13), ' '),CHAR(10), ' '), CHAR(13), ' '),'''','') 
	END caption, 
	column_name, 
	table_name, 
	s.description,
	Specials_Type_Reference,
	Specials_Type,
	data_type,
	dm.description,	
	s.gis_screen_id,
	d.parent_id, 
	d.item_top, 
	d.item_left,
	o.parent_object_id,
	o.object_name,
	p.gis_property_id 
	from gis_screen_detail d, gis_screen s,
	 gis_object o, gis_property p, gis_data_model dm 
	where s.gis_data_model_id =@gis_data_model_id
	and s.gis_screen_id = d.gis_screen_id
	and o.gis_data_model_id = @gis_data_model_id
	and o.gis_object_id = p.gis_object_id
	and p.gis_property_id = d.gis_property_id
	and d.gis_object_id = p.gis_object_id 
	and dm.gis_data_model_id = s.gis_data_model_id	
	
	OPEN field_cursor
	FETCH NEXT FROM field_cursor INTO @columndesc, @columnname, @tablename, @screendesc, @Specials_Type_Reference, @Specials_Type, @data_type, @data_model_desc, @screen_id, @parent_id, @item_top, @item_left, @parent_object_id, @object_name_desc, @gis_property_id 
		PRINT 'PASS'
		WHILE @@FETCH_STATUS = 0
		BEGIN 				
			set @description_sql_part = 's.'+@columnname
			IF @Specials_Type_Reference is not null
				BEGIN
					set @Specials_Type_Reference = rtrim(@Specials_Type_Reference)
					--PRINT '@Specials_Type_Reference : ' + @Specials_Type_Reference
					--PRINT '@Specials_Type : ' + @Specials_Type
					IF @Specials_Type = 3
						set @description_sql_part = '(select name from party where party_cnt = s.'+@columnname+' )'
					IF @Specials_Type = 6
						set @description_sql_part = '(select description from GIS_user_def_detail where GIS_user_def_detail_id = s.'+@columnname+' and GIS_user_def_header_id = '+@Specials_Type_Reference+' )'
					IF @Specials_Type = 2
						if exists(SELECT name FROM syscolumns where id = OBJECT_ID(@Specials_Type_Reference))
							set @description_sql_part = '(select description from '+@Specials_Type_Reference+' where '+@Specials_Type_Reference+'_id = s.'+@columnname+')'
				END
			IF @data_type = 20 --yes/no
				set @description_sql_part = 'CASE s.'+@columnname+' WHEN 0 THEN ''No'' WHEN 1 THEN ''Yes'' WHEN 0 THEN ''Unknown'' END'

--			PRINT 'screen id :' + cast(@screen_id as varchar(10))
--			PRINT 'table_name :' + @tablename
--			PRINT 'column : ' + @columndesc

			select @parent_tablename = table_name from gis_object where gis_object_id =  @parent_object_id
--			Print 'Parent table:' + @parent_tablename
			if @parent_tablename = @binder_table
				set @parent_id_name = 'null'
			else
				set @parent_id_name = @parent_tablename+'_id'
--			Print 'Parent id name: ' + @parent_id_name			
			
			set @sSQL = 'insert into ' + @ud_risks + ' select l.insurance_file_cnt, b.'+@binder_table+'_id, '+@tablename+'_id, l.screen_level, '+@parent_id_name+', '''+@data_model_desc+''', '''+@screendesc+''', '''+@columndesc+''', '+@description_sql_part+', '+cast(@parent_id as varchar(10))+', '+cast(@item_top as varchar(10))+', '+cast(@item_left as varchar(10))+', '''+@object_name_desc+''', '+cast(@gis_property_id as varchar(10))+' from ' + @ud_hier + ' l, '+@binder_table+' b, '+@tablename+' s where l.gis_screen_id='+cast(@screen_id as varchar(10))+' and len(ltrim('+@description_sql_part+')) > 0 and b.gis_policy_link_id = l.gis_policy_link_id and s.'+@binder_table+'_id = b.'+@binder_table+'_id'
			PRINT 'Insert Statement: ' + @sSQL
			IF  exists (select c.name from sysobjects t, syscolumns c where t.xtype='u' and t.name=@tablename and t.id=c.id and c.name=@columnname)
			BEGIN
				execute (@sSQL)
				--PRINT @sSQL
			END
			FETCH NEXT FROM field_cursor INTO @columndesc, @columnname, @tablename, @screendesc, @Specials_Type_Reference,@Specials_Type, @data_type, @data_model_desc, @screen_id, @parent_id, @item_top, @item_left, @parent_object_id, @object_name_desc, @gis_property_id
		END
		CLOSE field_cursor
		DEALLOCATE field_cursor 
 	FETCH NEXT FROM data_model_cursor INTO @gis_data_model_id
END
CLOSE data_model_cursor
DEALLOCATE data_model_cursor
go
EXEC dbo.spu_dashAllUserDefinedRisks
go