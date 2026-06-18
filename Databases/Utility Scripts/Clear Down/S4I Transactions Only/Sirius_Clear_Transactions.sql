
-- *******************************************************************************************
-- Sirius Transaction Cleardown Script
-- *******************************************************************************************/

SET NOCOUNT ON

PRINT '---START---'


if exists(select null from sysobjects where name = 'DDLReSeedIdentities' and type = 'P') begin
    drop procedure DDLReSeedIdentities
end
go
create procedure DDLReSeedIdentities as begin

    declare @sTableName varchar(255)
    set nocount on

    -- Find all tables we want to act on.
    declare curTables cursor fast_forward for
        select name from sysobjects
        where type = 'U' and name <> 'dtproperties'
        order by name

    -- Re-seed all tables with an identity column.
    open curTables
    fetch next from curTables into @sTableName
    while @@fetch_status = 0 begin
        if exists(select null from syscolumns
            where id = object_id(@sTableName)
            and (status & 128) <> 0) begin
            dbcc checkident(@sTableName, reseed, 0)
            dbcc checkident(@sTableName, reseed)
        end
        fetch next from curTables into @sTableName
    end
    close curTables

    -- Destroy the cursor.
    deallocate curTables
end
go

BEGIN TRANSACTION

PRINT 'STATS & TRANS'
DELETE Stats_Detail
DELETE Stats_folder
DELETE Transaction_Export_Detail
DELETE Transaction_Export_Folder
DELETE Transaction_Export_Complete

PRINT 'ACCOUNTS'

DELETE Credit_Control_Item
DELETE Chase_Cycle_Item
DELETE Cheque
DELETE TransMatch
DELETE TransDetail
DELETE Document
DELETE AllocationDetail
DELETE Allocation
DELETE CashListItem_Instalments 
DELETE CashListItem
DELETE CashList

-- Additional Accounts tables
DELETE ACTNumber
DELETE ACTNumber_Pool
DELETE AuditSet
DELETE Batch
DELETE InsurerPayment
DELETE invoice
DELETE invoice_item
DELETE MatchGroup
DELETE Statement
DELETE StatementDetail
DELETE Transaction_Report_Detail
DELETE Transaction_Report_Document
DELETE Suspended_Accounts_Transactions

PRINT 'INSTALMENTS'
DELETE  PFTransaction_ID 
DELETE  PF_Accounts_Transactions

DELETE  Summary_Stats_Agent
DELETE  Summary_Stats_Holder
DELETE  Summary_Stats_Premium
DELETE  Summary_Stats_Day
DELETE  Summary_Stats_Month

PRINT 'WORK TABLES'
DELETE Work_Stats_Detail
DELETE Work_Stats_Folder
DELETE Work_Transaction_Export_Detail
DELETE Work_Transaction_Export_Folder

PRINT 'IDS'

DECLARE	@source_id int,
	@next_id int,
	@table_name varchar(30),
	@gone_in smallint,
	@entityid int

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


-- Re-seeds all identity values

PRINT 'RESEED ALL TABLES'
PRINT ''

EXEC DDLReSeedIdentities

-- DN v0.9
dbcc checkident('GIS_Policy_link', reseed, 1000)
     
--ERR_ROLLBACK:

--Print '**********  FINISHED (ROLLBACK)  **********'
--ROLLBACK TRANSACTION

PRINT ''
PRINT '**********  FINISHED (COMMIT)  **********'
PRINT ''
PRINT ''

COMMIT TRANSACTION

Cleardown_End:
PRINT '---END---'

GO 