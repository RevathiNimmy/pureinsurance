DDLDropview 'dashRisksScheme'
go
Create view [dbo].[dashRisksScheme] as
select distinct (r.gis_data_model_id+999990) as NodeId, Null as ParentId, r.Scheme as NodeName, 1 as Level,
Null as ObjectName
from (select m.gis_data_model_id, s.gis_screen_id,sd.gis_property_id,
m.description as Scheme, s.description as Screen, sd.caption as ScreenLabel
from gis_data_model m, 
gis_screen s, 
gis_screen_detail sd,
gis_object o,
gis_property p
where 
s.gis_data_model_id=m.gis_data_model_id and 
s.gis_screen_id=sd.gis_screen_id and sd.gis_property_id is not null and
isnull(m.is_deleted, 0)=0 and
isnull(s.is_deleted, 0)=0 and m.gis_data_model_id=o.gis_data_model_id and
o.gis_object_id = p.gis_object_id and
p.gis_property_id=sd.gis_property_id and
len(caption) > 1  and caption <> '[BLANK]' 
and m.gis_data_model_id in (select distinct gis_data_model_id from gis_screen where gis_screen_id in 
(select distinct gis_screen_id from risk))
--and m.gis_data_model_id=6 --and s.gis_screen_id=469
) r
UNION 
select distinct (r.gis_object_id+888000) as NodeId, (r.gis_data_model_id+999990) as ParentId, Replace(r.object_name, '/', '-') as NodeName, 2 as Level, Null as ObjectName
from (select m.gis_data_model_id, o.gis_object_id,sd.gis_property_id,
m.description as Scheme, s.description as Screen, sd.caption as ScreenLabel, o.object_name,
(o.object_name +'-'+s.description) as ScreenName
from gis_data_model m, 
gis_screen s, 
gis_screen_detail sd,
gis_object o,
gis_property p
where 
s.gis_data_model_id=m.gis_data_model_id and 
s.gis_screen_id=sd.gis_screen_id and 
sd.gis_property_id is not null and
isnull(m.is_deleted, 0)=0 and
isnull(s.is_deleted, 0)=0 and 
m.gis_data_model_id=o.gis_data_model_id and
o.gis_object_id = p.gis_object_id and
p.gis_property_id=sd.gis_property_id and
p.gis_object_id = sd.gis_object_id and
len(caption) > 1 and caption <> '[BLANK]' 
and m.gis_data_model_id in (select distinct gis_data_model_id from gis_screen where gis_screen_id in 
(select distinct gis_screen_id from risk))
--and m.gis_data_model_id=6 --and s.gis_screen_id=469
) r
UNION 
select distinct r.gis_property_id as NodeId, (r.gis_object_id+888000) as ParentId, Replace(r.ScreenLabel, '/', '-') as NodeName, 3 as Level, r.object_name as ObjectName
from (select m.gis_data_model_id, o.gis_object_id,sd.gis_property_id,
m.description as Scheme, s.description as Screen, 

	CASE 
		WHEN sd.caption='[BLANK]' THEN column_name 
		WHEN len(ltrim(sd.caption)) = 0 THEN column_name 
		ELSE 
			REPLACE(REPLACE(REPLACE(REPLACE(sd.caption, CHAR(10) + CHAR(13), ' '),CHAR(10), ' '), CHAR(13), ' '),'''','') 
	END  ScreenLabel,
	
	o.object_name
	
from gis_data_model m, 
gis_screen s, 
gis_screen_detail sd,
gis_object o,
gis_property p
where 
s.gis_data_model_id=m.gis_data_model_id and 
s.gis_screen_id=sd.gis_screen_id and sd.gis_property_id is not null and
isnull(m.is_deleted, 0)=0 and
isnull(s.is_deleted, 0)=0 and m.gis_data_model_id=o.gis_data_model_id and
o.gis_object_id = p.gis_object_id and
p.gis_property_id=sd.gis_property_id and
p.gis_object_id = sd.gis_object_id 
and caption <> '[BLANK]' --len(caption) > 1 and
and m.gis_data_model_id in (select distinct gis_data_model_id from gis_screen where gis_screen_id in 
(select distinct gis_screen_id from risk))
--and m.gis_data_model_id=6 --and s.gis_screen_id=469
) r;

