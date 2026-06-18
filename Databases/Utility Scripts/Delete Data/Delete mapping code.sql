DECLARE	@mapping_id INT,
	@element_id INT,
	@description VARCHAR(255)

SELECT	@description = 'Agents',
	@mapping_id = NULL,
	@element_id = NULL

SELECT	@mapping_id = mapping_id
FROM	Mapping
WHERE	description = @description

IF @mapping_id IS NULL
	RETURN

PRINT	'Mapping = ' + CONVERT(VARCHAR(100), @mapping_id)

SELECT	@element_id = element_id
FROM	StructureTree
WHERE	mapping_id = @mapping_id

PRINT	'Element = ' + CONVERT(VARCHAR(100), (ISNULL(@element_id, 0)))

BEGIN TRANSACTION

PRINT	'Deleting BankAccount'

DELETE	bankaccount
WHERE	account_id IN (
	SELECT	account_id
	FROM	structuretree
	WHERE	parent_node_id = (
		SELECT	node_id
		FROM	structuretree
		WHERE	mapping_id = @mapping_id
		)
	)

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

PRINT	'Deleting Account'

DELETE	account
WHERE	account_id IN (
	SELECT	account_id
	FROM	structuretree
	WHERE	parent_node_id = (
		SELECT	node_id
		FROM	structuretree
		WHERE	mapping_id = @mapping_id
		)
	)

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

PRINT	'Deleting StructureTree Parents'

DELETE	structuretree
WHERE	parent_node_id = (
	SELECT	node_id
	FROM	structuretree
	WHERE	mapping_id = @mapping_id
	)

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

PRINT	'Deleting StructureTree'

DELETE	StructureTree
WHERE	mapping_id = @mapping_id

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

IF @element_id IS NOT NULL
BEGIN
	PRINT	'Deleting Element Extras'

	DELETE	ElementExtras
	WHERE	element_id = @element_id

	IF @@ERROR <> 0
	BEGIN
		ROLLBACK TRANSACTION
		RETURN
	END

	PRINT	'Deleting Element'

	DELETE	Element
	WHERE	element_id = @element_id

	IF @@ERROR <> 0
	BEGIN
		ROLLBACK TRANSACTION
		RETURN
	END
END

PRINT	'Deleting Mapping'

DELETE	Mapping
WHERE	mapping_id = @mapping_id

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

COMMIT TRANSACTION
