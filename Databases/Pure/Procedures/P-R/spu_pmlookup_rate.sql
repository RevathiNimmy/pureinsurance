EXECUTE DDLDropProcedure spu_pmlookup_rate
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

CREATE   PROCEDURE spu_pmlookup_rate  
 @scheme_id char(100),
 @calculation_name char(100),
 @execution_date datetime, /* a valid date or null */
 @lookup1 char(50),
 @lookup2 char(50),
 @lookup3 char(50)
--@return_value integer OUTPUT,
 --@rate numeric OUTPUT
AS

declare @return_value int
declare @rate numeric(19,4)

/* 
/
/ Return values 
/  1 scheme id is invalid
/  2 gis_rate_type and/or calculation name is invalid
/  3 rate not found
/ 10/20/30 lookup 1/2/3 required but not provided
/ 11/21/31 code not found for lookup 1/2/3
/ 12/22/32 group not found for lookup 1/2/3 code
/ 13/23/33 lookup 1/2/3 provided but not required 
/
*/


/* internal variables */
declare @gis_scheme_id  integer
declare @gis_rate_type integer
declare @gis_List_type_lookup1 integer /* holds the list type code id */
declare @gis_List_type_lookup2 integer
declare @gis_List_type_lookup3 integer
declare @gis_rate_lookup1 integer /* hold the value to lookup in gis_rate_items */
declare @gis_rate_lookup2 integer
declare @gis_rate_lookup3 integer
declare @temp_result as integer /* working varable */

/* set some sensible default retun values */
select @return_value=0
select @rate=0

/* set default values */
select @gis_rate_lookup1=null
select @gis_rate_lookup2=null
select @gis_rate_lookup3=null

/* check if date has been passed, if not set to now */
if isdate(@execution_date) =0 
   select @execution_date=getdate()

/* get the gis_scheme_id. Can be passed as a id or string */
if isnumeric(@scheme_id) = 0 /* check for name being passed rather than id */
    select @gis_scheme_id=gis_scheme_id from gis_scheme 
        where scheme_desc=@scheme_id 
        and datediff(day,start_date,@execution_date) >=0/* existing GIS scheme table */
else
    select @gis_scheme_id=gis_scheme_id from gis_scheme 
        where gis_scheme_id=@scheme_id 
        and datediff(day,start_date,@execution_date) >=0/* existing GIS scheme table */

if @gis_scheme_id is null
begin
    select @return_value=1 /* error: scheme id was invalid */
    goto exit_proc
end

print 'SchemeID ' +cast (@gis_scheme_id as varchar(10))

/* get the lookup types we will be using */
select @gis_rate_type=gis_rate_type_id, @gis_List_type_lookup1=gis_list_type_lookup1, @gis_List_type_lookup2=gis_list_type_lookup2 ,  @gis_List_type_lookup3=gis_list_type_lookup3  
    from gis_rate_type 
    where gis_scheme_id = @gis_scheme_id
    and description=@calculation_name

IF @gis_List_type_lookup1 is null
begin
    select @return_value=2 /* error: gis_rate_type/calculation name invalid */
    goto exit_proc
end


/* test if we have values for each lookup required */
IF @gis_List_type_lookup1 is not null and @lookup1 is null
    begin
    select @return_value=10 /* error: lookup 1 required */
    goto exit_proc
    end
else if @gis_List_type_lookup1 is null and @lookup1 is not null
    begin
    select @return_value=13 /* error: lookup 1 not required */
    goto exit_proc
    end
else if @gis_List_type_lookup1 is not null  and @lookup1 is not null
    /* we have the data so get the code associated with it */
    begin
    select @temp_result=g2.gis_list_items_id 
           from gis_list_type_usage as g1 
           inner join gis_list_items as g2 on g1.gis_list_items_id=g2.gis_list_items_id 
           where g1.gis_list_type_id=@gis_List_type_lookup1 and g2.code=@lookup1
           and datediff(day,effective_date,@execution_date) >=0
           /*and g1.is_deleted=0*/

    if @temp_result is null
        begin
        select @return_value=11 /* error: code not found for lookup 1 */
        goto exit_proc
    end
    /* now we have the code, get the group it code belongs to */

--    select @gis_rate_lookup1=gis_list_grouping_id
--        from gis_list_grouping_items 
--        where gis_scheme_id=@gis_scheme_id 
--        and gis_list_items_id=@temp_result

    select @gis_rate_lookup1 = glgi.gis_list_grouping_id
      from gis_list_grouping_items glgi
      join gis_list_grouping glg on glgi.gis_list_grouping_id = glg.gis_list_grouping_id
      where glg.gis_scheme_id = @gis_scheme_id
      and glg.gis_list_type_id = @gis_list_type_lookup1
      and glgi.gis_list_items_id = @temp_result
      	

    if @gis_rate_lookup1 is null
        begin
        select @return_value=12 /* error: group not found for lookup 1 code */
        goto exit_proc
    end
end

IF @gis_List_type_lookup2 is not null and @lookup2 is null
    begin
    select @return_value=20 /* error: lookup 2 required */
    goto exit_proc
    end
else if @gis_List_type_lookup2 is null and @lookup2 is not null
    begin
    select @return_value=23 /* error: lookup 2 not required */
    goto exit_proc
    end
else if @gis_List_type_lookup2 is not null  and @lookup2 is not null
    /* we have the data so get the code associated with it */
    begin
    select @temp_result=g2.gis_list_items_id 
           from gis_list_type_usage as g1 
           inner join gis_list_items as g2 on g1.gis_list_items_id=g2.gis_list_items_id 
           where g1.gis_list_type_id=@gis_List_type_lookup2 and g2.code=@lookup2
           and datediff(day,effective_date,@execution_date) >=0
           /*and g1.is_deleted=0*/
    if @temp_result is null
        begin
        select @return_value=21 /* error: code not found for lookup 2 */
        goto exit_proc
    end
    /* now we have the code, get the group it code belongs to */
--    select @gis_rate_lookup2=gis_list_grouping_id
--        from gis_list_grouping_items 
--        where gis_scheme_id=@gis_scheme_id 
--        and gis_list_items_id=@temp_result
    select @gis_rate_lookup2 = glgi.gis_list_grouping_id
      from gis_list_grouping_items glgi
      join gis_list_grouping glg on glgi.gis_list_grouping_id = glg.gis_list_grouping_id
      where glg.gis_scheme_id = @gis_scheme_id
      and glg.gis_list_type_id = @gis_list_type_lookup2
      and glgi.gis_list_items_id = @temp_result

    if @gis_rate_lookup2 is null
        begin
        select @return_value=22 /* error: group not found for lookup 1 code */
        goto exit_proc
   end
end


IF @gis_List_type_lookup3 is not null and @lookup3 is null
    begin
    select @return_value=30 /* error: lookup 3 required */
    goto exit_proc
    end
else if @gis_List_type_lookup3 is null and @lookup3 is not null
    begin
    select @return_value=33 /* error: lookup 3 not required */
    goto exit_proc
    end
else if @gis_List_type_lookup3 is not null  and @lookup3 is not null
    /* we have the data so get the code associated with it */
    begin
    select @temp_result=g2.gis_list_items_id 
           from gis_list_type_usage as g1 
           inner join gis_list_items as g2 on g1.gis_list_items_id=g2.gis_list_items_id 
           where g1.gis_list_type_id=@gis_List_type_lookup3 and g2.code=@lookup3
           and datediff(day,effective_date,@execution_date) >=0
           /*and g1.is_deleted=0*/
    if @temp_result is null
        begin
        select @return_value=31 /* error: code not found for lookup 3 */
        goto exit_proc
    end
    /* now we have the code, get the group it code belongs to */
--    select @gis_rate_lookup3=gis_list_grouping_id
--        from gis_list_grouping_items 
--        where gis_scheme_id=@gis_scheme_id
--        and gis_list_items_id=@temp_result
    select @gis_rate_lookup3 = glgi.gis_list_grouping_id
      from gis_list_grouping_items glgi
      join gis_list_grouping glg on glgi.gis_list_grouping_id = glg.gis_list_grouping_id
      where glg.gis_scheme_id = @gis_scheme_id
      and glg.gis_list_type_id = @gis_list_type_lookup3
      and glgi.gis_list_items_id = @temp_result

    if @gis_rate_lookup3 is null
        begin
        select @return_value=32 /* error: group not found for lookup 3 code */
        goto exit_proc
        end
    end


/* determine if we need to do a lookup on 1,2 or 3 parameters */

if @gis_rate_lookup3 is not null 
    begin
    select @rate=rate from gis_rate_items
        where gis_rate_type_id=@gis_rate_type
        and lookup3=@gis_rate_lookup3
        and lookup2=@gis_rate_lookup2
        and lookup1=@gis_rate_lookup1
    end
else if @gis_rate_lookup2 is not null 
    begin
    select @rate=rate from gis_rate_items
        where gis_rate_type_id=@gis_rate_type
        and lookup2=@gis_rate_lookup2
        and lookup1=@gis_rate_lookup1
    end




else
    begin
    select @rate=rate from gis_rate_items
        where gis_rate_type_id=@gis_rate_type
        and lookup1=@gis_rate_lookup1
	
    end



if @rate is null
    begin
    select @return_value=3 /* error: rate not found */
    goto exit_proc
    end
else

--return values as recordset
select @return_value,@Rate

return

exit_proc:
select @return_value,@Rate


GO
