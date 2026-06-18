DECLARE	@account_id INT,
	@element_id INT,
	@description VARCHAR(255)

SELECT	@description = 'BARCLAYSBANK',
	@account_id = NULL,
	@element_id = NULL

SELECT	@account_id = account_id
FROM	account
WHERE	short_code = @description

IF @account_id IS NULL
	RETURN

PRINT	'Account = ' + CONVERT(VARCHAR(100), @account_id)

SELECT	@element_id = element_id
FROM	StructureTree
WHERE	account_id = @account_id

PRINT	'Element = ' + CONVERT(VARCHAR(100), (ISNULL(@element_id, 0)))

BEGIN TRANSACTION

PRINT	'Deleting BankAccount'

DELETE	bankaccount
WHERE	account_id = @account_id

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

PRINT	'Deleting Account'

DELETE	account
WHERE	account_id = @account_id

IF @@ERROR <> 0
BEGIN
	ROLLBACK TRANSACTION
	RETURN
END

PRINT	'Deleting StructureTree'

DELETE	structuretree
WHERE	account_id = @account_id

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

	PRINT	'Deleting StructureTree For Element'

	DELETE	structuretree
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

COMMIT TRANSACTION
