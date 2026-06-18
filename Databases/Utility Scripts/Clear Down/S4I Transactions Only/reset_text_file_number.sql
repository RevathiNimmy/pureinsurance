declare	@entityid		int

declare entity_cursor cursor 
for	select entity_type_id
	from text_file_number

open entity_cursor

fetch next from entity_cursor
into @entityid

while @@fetch_status = 0
begin
	update text_file_number
	set next_file_number = 
	(	select isnull(max(file_number), 1)
		from text_file
		where entity_type_id = @entityid
	)
	where entity_type_id = @entityid

	fetch next from entity_cursor
	into @entityid
end

close entity_cursor
deallocate entity_cursor
