ddldropprocedure 'spe_Event_FSANote_text_saa'
go

create procedure spe_Event_FSANote_text_saa
	@party_cnt int
as
create table #temp_FSANote(party_cnt int,FSANote_text_id int,text_line varchar(255))

declare FSANote cursor fast_forward for 
	select u.username,el.event_date,el.description from event_log el, pmuser u where el.user_id=u.user_id and el.party_cnt = @party_cnt and el.event_type_id in (select max(event_type_id) from event_type where code = 'N_FSA') order by el.event_date ASC
declare @username varchar(255)
declare @event_date datetime
declare @description varchar(1000)
declare @string varchar(255)
declare @FSANote_text_id integer
declare @header varchar(255)
set @FSANote_text_id = 0
open FSANote
fetch next from FSANote into @username,@event_date,@description
while (@@fetch_status <> -1)
	Begin
		If (@@fetch_Status <> -2)
		Begin
			select @header = '[' + cast(@username as char(10)) 
			select @header = @header + cast(day(@event_date) as varchar(2)) + '/' + cast(month(@event_date) as varchar(2)) + '/' + cast(year(@event_date) as varchar(4)) 
			select @header = @header + ' ' + convert(varchar(10),@event_date,108)
			select @header = @header + ']   ' + char(13) + char(10)

			set @FSANote_text_id = @FSANote_text_id + 1
			insert into #temp_FSANote(party_cnt,FSANote_text_id,text_line) values(@party_cnt,@FSANote_text_id,@header)
			
			if len(substring(@description,1,255)) > 0
			Begin
				set @string = substring(@description,1,255)
				set @FSANote_text_id = @FSANote_text_id + 1
				insert into #temp_FSANote(party_cnt,FSANote_text_id,text_line) values(@party_cnt,@FSANote_text_id,@string)
			end
			if len(substring(@description,256,255)) > 0
			Begin
				set @string = substring(@description,256,255)
				set @FSANote_text_id = @FSANote_text_id + 1
				insert into #temp_FSANote(party_cnt,FSANote_text_id,text_line) values(@party_cnt,@FSANote_text_id,@string)
			end
			if len(substring(@description,511,255)) > 0
			Begin
				set @string = substring(@description,511,255)
				set @FSANote_text_id = @FSANote_text_id + 1
				insert into #temp_FSANote(party_cnt,FSANote_text_id,text_line) values(@party_cnt,@FSANote_text_id,@string)
			end
			if len(substring(@description,766,234)) > 0
			Begin
				set @string = substring(@description,766,255)
				set @FSANote_text_id = @FSANote_text_id + 1
				insert into #temp_FSANote(party_cnt,FSANote_text_id,text_line) values(@party_cnt,@FSANote_text_id,@string)
			end
		End
		Fetch Next From FSANote into @username,@event_date,@description
	End
Close FSANote
Deallocate FSANote

select * from #temp_FSANote

drop table #temp_fsanote
