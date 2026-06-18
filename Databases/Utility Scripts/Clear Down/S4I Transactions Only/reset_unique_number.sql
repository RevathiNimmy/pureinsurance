DECLARE	@source_id int,
	@next_id int,
	@table_name varchar(30),
	@gone_in smallint,
	@entityid int

DECLARE	address_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	address

OPEN	address_cursor

FETCH NEXT FROM address_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(address_id)
	FROM	address
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Address_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM address_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Address_%'
END

CLOSE address_cursor
DEALLOCATE address_cursor

DECLARE	accumulation_cursor CURSOR FOR
	SELECT	DISTINCT accumulation_id
	FROM	accumulation

OPEN	accumulation_cursor

FETCH NEXT FROM accumulation_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(accumulation_id)
	FROM	accumulation

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Accumulation'
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM accumulation_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name = 'Accumulation'
END

CLOSE accumulation_cursor
DEALLOCATE accumulation_cursor

DECLARE	contact_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	contact

OPEN	contact_cursor

FETCH NEXT FROM contact_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(contact_id)
	FROM	contact
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Contact_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM contact_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Contact_%'
END

CLOSE contact_cursor
DEALLOCATE contact_cursor

DECLARE	party_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	party

OPEN	party_cursor

FETCH NEXT FROM party_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(party_id)
	FROM	party
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Party_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM party_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Party_%'
END

CLOSE party_cursor
DEALLOCATE party_cursor

DECLARE	insurance_folder_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	insurance_folder

OPEN	insurance_folder_cursor

FETCH NEXT FROM insurance_folder_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(insurance_folder_id)
	FROM	insurance_folder
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Insurance_Folder_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM insurance_folder_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Insurance_Folder_%'
END

CLOSE insurance_folder_cursor
DEALLOCATE insurance_folder_cursor

DECLARE	insurance_file_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	insurance_file

OPEN	insurance_file_cursor

FETCH NEXT FROM insurance_file_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(insurance_file_id)
	FROM	insurance_file
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Insurance_File_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM insurance_file_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Insurance_File_%'
END

CLOSE insurance_file_cursor
DEALLOCATE insurance_file_cursor

-- Alix - 23/06/2003

DECLARE	risk_folder_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	risk_folder

OPEN	risk_folder_cursor

FETCH NEXT FROM risk_folder_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(risk_folder_cnt)
	FROM	risk_folder
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Risk_Folder_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM risk_folder_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Risk_Folder_%'
END

CLOSE risk_folder_cursor
DEALLOCATE risk_folder_cursor

DECLARE	Transaction_Export_Folder_cursor CURSOR FOR
	SELECT	DISTINCT source_id
	FROM	Transaction_Export_Folder

OPEN	Transaction_Export_Folder_cursor

FETCH NEXT FROM Transaction_Export_Folder_cursor INTO @source_id

SET @gone_in = 0

WHILE @@FETCH_STATUS = 0
BEGIN

	SET @gone_in = 1

	SELECT	@next_id = MAX(Transaction_Export_Folder_cnt)
	FROM	Transaction_Export_Folder
	WHERE	source_id = @source_id

	IF @next_id IS NULL
		SELECT	@next_id = 0

	SELECT	@next_id = @next_id + 1

	SELECT	@table_name = 'Transaction_Export_Folder_' + convert(varchar(10), @source_id)
	
	DELETE
	FROM	unique_number
	WHERE	table_name = @table_name

        INSERT INTO Unique_Number
        ( table_name, next_number )
        VALUES
        ( @table_name, @next_id )

	FETCH NEXT FROM Transaction_Export_Folder_cursor INTO @source_id

END

IF @gone_in = 0
BEGIN
	UPDATE Unique_Number 
	SET next_number=1
	WHERE table_name like 'Transaction_Export_Folder_%'
END

CLOSE Transaction_Export_Folder_cursor
DEALLOCATE Transaction_Export_Folder_cursor

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
