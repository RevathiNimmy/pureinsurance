/****************************************************************************************

Script to clear down Sirius prior to Data Transfer.

Removes relevant records from other tables dependant on
these accounts.

Tables affected:
	TransMatch
	TransDetail
	Document
	StructureTree
	Mapping
	ElementExtras
	Element

	allocationdetail
	allocation
	cashlistitem
	bankaccount

	insurance_file_system
	Policy_agents
	insurance_file_risk_link
	insurance_file
	party
	account

*****************************************************************************************
1.0	Created.				Richard Hill		24 April 2002
1.1	Updated for Sirius 1.8.5SR10            Peter Rounce		16 September 2002

*****************************************************************************************/

Use Sirius

-- Delete Transaction stuff.
Delete TransMatch
GO
Delete TransDetail
GO
Delete Document
GO
----------------------------

-- Create temp tables to store
CREATE TABLE tmpAccountIds
	(account_id int)
GO
CREATE TABLE tmpMappingIds
	(mapping_id int)
GO
CREATE TABLE tmpElementIds
	(element_id int)
GO
-------------------------------

-- Retrieve all account_ids into temp table other than those of default accounts
INSERT INTO tmpAccountIds
	SELECT account_id
	FROM Account
	WHERE LTRIM(RTRIM(short_code)) NOT IN(	'DATATRANSFERSUSP',
						'VATACCOUNT',
						'RETAINPROF',
						'GLCREDIT',
						'GLDEBIT',
						'N5092',
						'N4092', 'IBA', 'OFFICE')
GO
---------------------------------------------------------------------------------



--Retrieve all mapping and element ids from StructureTree associated with accounts
--to be removed so we can use them once StructureTree records have been removed.				
INSERT INTO tmpMappingIds	
	SELECT mapping_id
	FROM StructureTree
	WHERE account_id IN(	SELECT account_id
				FROM tmpAccountIds)
	AND mapping_id IS NOT NULL
GO

INSERT INTO tmpElementIds	
	SELECT element_id
	FROM StructureTree
	WHERE account_id IN(	SELECT account_id
				FROM tmpAccountIds)
	AND element_id IS NOT NULL
GO
---------------------------------------------------------------------------------



-- Remove all records from StructureTree associated with accounts to be removed.
DELETE StructureTree
WHERE account_id IN(	SELECT account_id
				FROM tmpAccountIds)

GO
--------------------------------------------------------------------------------


-- Delete mapping and element records.-------
DELETE Mapping
WHERE mapping_id IN(	SELECT mapping_id
			FROM tmpMappingIds)
			

GO
DELETE ElementExtras
WHERE element_id IN(	SELECT element_id
			FROM tmpElementIds)
GO
DELETE Element
WHERE element_id IN(	SELECT element_id
			FROM tmpElementIds)
GO
----------------------------------------------

delete allocationdetail
GO
delete allocation
GO
delete cashlistitem
GO

delete bankaccount
where account_id IN(	SELECT account_id
			FROM tmpAccountIds)
GO

delete event_log
go

delete insurance_file_system
go

delete Policy_agents
go

delete insurance_file_risk_link
go

delete insurance_file
go

delete party
go

-- Finally remove Accounts themselves-------
DELETE Account
WHERE account_id IN(	SELECT account_id
			FROM tmpAccountIds)
GO
--------------------------------------------

-- Drop temp tables------
DROP TABLE tmpAccountIds
GO
DROP TABLE tmpMappingIds
GO
DROP TABLE tmpElementIds
GO
-------------------------